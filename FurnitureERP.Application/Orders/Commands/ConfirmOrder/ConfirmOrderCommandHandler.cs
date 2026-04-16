using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Common.Services;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Aggregates.Products;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, ConfirmOrderResult>
{
    private const decimal WorkingHoursPerDay = 8m;
    private const decimal FallbackHoursPerUnit = 8m;

    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMaterialRepository materialRepository,
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ConfirmOrderResult> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new NotFoundException($"Objednávka s ID {request.OrderId} nebyla nalezena");

        var products = await LoadProductsForOrderAsync(order.OrderItems, cancellationToken);

        var (materialsAvailable, shortageDetails) =
            await CheckMaterialAvailabilityAsync(order.OrderItems, products, cancellationToken);

        // Kapacita pool per pozice: pozice → počet aktivních zaměstnanců × 8h/den
        var positionCapacity = _employeeRepository
            .GetActiveEmployees()
            .GroupBy(e => e.Position)
            .ToDictionary(g => g.Key, g => g.Count() * WorkingHoursPerDay);

        // Backlog: hodiny zakázek aktuálně ve výrobě per pozice
        var backlogHoursByPosition = await CalculateBacklogByPositionAsync(
            request.OrderId, cancellationToken);

        // Nová zakázka: hodiny per pozice
        var newOrderHoursByPosition = CalculateHoursByPosition(order.OrderItems, products);

        var today = DateTime.UtcNow.Date;
        var materialStart = materialsAvailable
            ? today
            : WorkingDaysCalculator.GetNextDeliveryMonday(today);

        // Výsledný počet dní s paralelním/sekvenčním modelem
        var productionDays = CalculateScheduledDays(
            newOrderHoursByPosition, backlogHoursByPosition, positionCapacity);

        var completionDate = WorkingDaysCalculator.AddWorkingDays(materialStart, productionDays);

        order.ConfirmOrder(completionDate);
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ConfirmOrderResult(materialsAvailable, completionDate, shortageDetails);
    }

    /// <summary>
    /// Paralelní/sekvenční model: pokud zbývající kapacita v době backlogu pokryje novou zakázku,
    /// pracuje se paralelně. Jinak se zbytek zařadí do fronty za backlog.
    /// </summary>
    private static int CalculateScheduledDays(
        Dictionary<string, decimal> newOrderHours,
        Dictionary<string, decimal> backlogHours,
        Dictionary<string, decimal> positionCapacity)
    {
        var maxDays = 0;

        foreach (var (position, newHours) in newOrderHours)
        {
            if (newHours <= 0) continue;

            var capacity = positionCapacity.GetValueOrDefault(position, WorkingHoursPerDay);
            var backlog = backlogHours.GetValueOrDefault(position, 0m);
            var backlogDays = (int)Math.Ceiling(backlog / capacity);
            var spareCapacity = backlogDays * capacity - backlog;

            int positionDays;
            if (newHours <= spareCapacity)
                positionDays = Math.Max(backlogDays, (int)Math.Ceiling(newHours / capacity));
            else
                positionDays = backlogDays + (int)Math.Ceiling((newHours - spareCapacity) / capacity);

            if (positionDays > maxDays)
                maxDays = positionDays;
        }

        return Math.Max(1, maxDays);
    }

    /// <summary>
    /// Vypočítá počet pracovních dní pro daný soubor hodin per pozice (bottleneck model).
    /// Výsledek = max přes všechny pozice ceil(hodiny / kapacita poolu).
    /// </summary>
    private static int CalculateBottleneckDays(
        Dictionary<string, decimal> hoursByPosition,
        Dictionary<string, decimal> positionCapacity)
    {
        var maxDays = 0;

        foreach (var (position, hours) in hoursByPosition)
        {
            if (hours <= 0) continue;
            var capacity = positionCapacity.GetValueOrDefault(position, WorkingHoursPerDay);
            var days = (int)Math.Ceiling(hours / capacity);
            if (days > maxDays)
                maxDays = days;
        }

        return maxDays;
    }

    /// <summary>
    /// Hodiny nové zakázky seskupené podle pozice z LaborBomů.
    /// Pokud produkt nemá LaborBomy, použijeme zálohu jako "Obecná výroba".
    /// </summary>
    private static Dictionary<string, decimal> CalculateHoursByPosition(
        IEnumerable<OrderItem> orderItems,
        Dictionary<int, Product> products)
    {
        var hoursByPosition = new Dictionary<string, decimal>();

        foreach (var item in orderItems)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                continue;

            if (product.LaborBoms.Any())
            {
                foreach (var lb in product.LaborBoms)
                {
                    var key = lb.Position;
                    hoursByPosition[key] =
                        hoursByPosition.GetValueOrDefault(key) + lb.HoursRequired * item.Quantity;
                }
            }
            else
            {
                // Záloha pro produkty bez LaborBomů
                var fallbackHours = product.ProductionDays * FallbackHoursPerUnit * item.Quantity;
                hoursByPosition["Obecná výroba"] =
                    hoursByPosition.GetValueOrDefault("Obecná výroba") + fallbackHours;
            }
        }

        return hoursByPosition;
    }

    /// <summary>
    /// Sečte hodiny všech zakázek InProduction (kromě aktuální) per pozice.
    /// </summary>
    private async Task<Dictionary<string, decimal>> CalculateBacklogByPositionAsync(
        int excludeOrderId,
        CancellationToken cancellationToken)
    {
        var inProductionOrders = _orderRepository
            .GetOrdersByStatus(OrderStatus.InProduction)
            .Where(o => o.Id != excludeOrderId)
            .ToList();

        var backlog = new Dictionary<string, decimal>();

        foreach (var activeOrder in inProductionOrders)
        {
            var products = await LoadProductsForOrderAsync(activeOrder.OrderItems, cancellationToken);
            var hours = CalculateHoursByPosition(activeOrder.OrderItems, products);
            foreach (var (position, h) in hours)
                backlog[position] = backlog.GetValueOrDefault(position) + h;
        }

        return backlog;
    }

    private async Task<Dictionary<int, Product>> LoadProductsForOrderAsync(
        IEnumerable<OrderItem> orderItems,
        CancellationToken cancellationToken)
    {
        var products = new Dictionary<int, Product>();
        foreach (var item in orderItems)
        {
            if (!products.ContainsKey(item.ProductId))
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                if (product != null)
                    products[item.ProductId] = product;
            }
        }
        return products;
    }

    private async Task<(bool materialsAvailable, string? shortageDetails)> CheckMaterialAvailabilityAsync(
        IEnumerable<OrderItem> orderItems,
        Dictionary<int, Product> products,
        CancellationToken cancellationToken)
    {
        var requiredByMaterial = new Dictionary<int, decimal>();

        foreach (var item in orderItems)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                continue;

            foreach (var bom in product.MaterialBoms)
            {
                var totalRequired = bom.QuantityWithWastage * item.Quantity;
                requiredByMaterial[bom.MaterialId] =
                    requiredByMaterial.GetValueOrDefault(bom.MaterialId) + totalRequired;
            }
        }

        if (requiredByMaterial.Count == 0)
            return (true, null);

        var shortages = new List<string>();
        foreach (var (materialId, requiredQty) in requiredByMaterial)
        {
            var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);
            if (material == null)
                continue;

            if (material.CurrentStock < requiredQty)
            {
                shortages.Add(
                    $"{material.Name}: potřeba {requiredQty:N2} {material.Unit}, " +
                    $"na skladě {material.CurrentStock:N2} {material.Unit}");
            }
        }

        return shortages.Count == 0
            ? (true, null)
            : (false, string.Join("; ", shortages));
    }
}
