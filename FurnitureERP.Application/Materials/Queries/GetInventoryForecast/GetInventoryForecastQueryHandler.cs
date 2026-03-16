using FurnitureERP.Application.Materials.DTOs;
using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetInventoryForecast;

public class GetInventoryForecastQueryHandler : IRequestHandler<GetInventoryForecastQuery, IEnumerable<InventoryForecastDto>>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public GetInventoryForecastQueryHandler(
        IMaterialRepository materialRepository,
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<IEnumerable<InventoryForecastDto>> Handle(
        GetInventoryForecastQuery request,
        CancellationToken cancellationToken)
    {
        var materials = await Task.Run(() => _materialRepository.GetActiveMaterials().ToList(), cancellationToken);
        var activeOrders = await Task.Run(() => _orderRepository.GetActiveOrders().ToList(), cancellationToken);

        var requiredByMaterial = await CalculateRequiredMaterialsAsync(activeOrders, cancellationToken);

        return materials.Select(m =>
        {
            var required = requiredByMaterial.GetValueOrDefault(m.Id, 0m);
            var shortfall = Math.Max(0m, required - m.CurrentStock);
            var recommended = Math.Max(0m, required + m.MinimumStock - m.CurrentStock);

            return new InventoryForecastDto(
                m.Id,
                m.Name,
                m.Category,
                m.Unit,
                m.Supplier,
                m.CurrentStock,
                m.MinimumStock,
                required,
                shortfall,
                recommended
            );
        })
        .OrderByDescending(f => f.HasShortfall)
        .ThenByDescending(f => f.BelowMinimum)
        .ThenBy(f => f.MaterialName)
        .ToList();
    }

    private async Task<Dictionary<int, decimal>> CalculateRequiredMaterialsAsync(
        IEnumerable<Order> activeOrders,
        CancellationToken cancellationToken)
    {
        var result = new Dictionary<int, decimal>();
        var allOrderItems = activeOrders.SelectMany(o => o.OrderItems).ToList();

        if (allOrderItems.Count == 0)
            return result;

        var productIds = allOrderItems.Select(i => i.ProductId).Distinct().ToList();
        var productTasks = productIds.Select(id => _productRepository.GetByIdAsync(id, cancellationToken));
        var products = await Task.WhenAll(productTasks);
        var productMap = products.Where(p => p != null).ToDictionary(p => p!.Id);

        foreach (var item in allOrderItems)
        {
            if (!productMap.TryGetValue(item.ProductId, out var product))
                continue;

            foreach (var bom in product.MaterialBoms)
            {
                var needed = bom.QuantityWithWastage * item.Quantity;
                result[bom.MaterialId] = result.GetValueOrDefault(bom.MaterialId) + needed;
            }
        }

        return result;
    }
}
