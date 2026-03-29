using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Common.Services;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, ConfirmOrderResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMaterialRepository materialRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ConfirmOrderResult> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new NotFoundException($"Objednávka s ID {request.OrderId} nebyla nalezena");

        var (materialsAvailable, shortageDetails) =
            await CheckMaterialAvailabilityAsync(order.OrderItems, cancellationToken);

        var productionDays = await CalculateMaxProductionDaysAsync(order.OrderItems, cancellationToken);

        var today = DateTime.UtcNow.Date;
        var startDate = materialsAvailable ? today : WorkingDaysCalculator.GetNextDeliveryMonday(today);
        var completionDate = WorkingDaysCalculator.AddWorkingDays(startDate, productionDays);

        order.ConfirmOrder(completionDate);

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ConfirmOrderResult(materialsAvailable, completionDate, shortageDetails);
    }

    private async Task<(bool materialsAvailable, string? shortageDetails)> CheckMaterialAvailabilityAsync(
        IEnumerable<Domain.Aggregates.Orders.OrderItem> orderItems,
        CancellationToken cancellationToken)
    {
        var requiredByMaterial = new Dictionary<int, decimal>();

        foreach (var item in orderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
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

    private async Task<int> CalculateMaxProductionDaysAsync(
        IEnumerable<Domain.Aggregates.Orders.OrderItem> orderItems,
        CancellationToken cancellationToken)
    {
        var maxDays = 1;
        foreach (var item in orderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product != null && product.ProductionDays > maxDays)
                maxDays = product.ProductionDays;
        }
        return maxDays;
    }
}
