using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Exceptions;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand>
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

    public async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new NotFoundException($"Objednávka s ID {request.OrderId} nebyla nalezena");

        await CheckMaterialAvailabilityAsync(order.OrderItems, cancellationToken);

        order.ConfirmOrder();

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task CheckMaterialAvailabilityAsync(
        IEnumerable<Domain.Aggregates.Orders.OrderItem> orderItems,
        CancellationToken cancellationToken)
    {
        // Aggregate total required quantity per material across all order items
        var requiredByMaterial = new Dictionary<int, decimal>();

        var productTasks = orderItems
            .Select(item => _productRepository.GetByIdAsync(item.ProductId, cancellationToken))
            .ToList();

        var products = await Task.WhenAll(productTasks);

        foreach (var (orderItem, product) in orderItems.Zip(products))
        {
            if (product == null)
                continue;

            foreach (var bom in product.MaterialBoms)
            {
                var totalRequired = bom.QuantityWithWastage * orderItem.Quantity;
                requiredByMaterial[bom.MaterialId] = requiredByMaterial.GetValueOrDefault(bom.MaterialId) + totalRequired;
            }
        }

        if (requiredByMaterial.Count == 0)
            return;

        // Load all needed materials in parallel
        var materialTasks = requiredByMaterial.Keys
            .Select(id => _materialRepository.GetByIdAsync(id, cancellationToken))
            .ToList();

        var materials = await Task.WhenAll(materialTasks);

        var shortages = new List<string>();

        foreach (var (materialId, requiredQty) in requiredByMaterial)
        {
            var material = materials.FirstOrDefault(m => m?.Id == materialId);
            if (material == null)
                continue;

            if (material.CurrentStock < requiredQty)
            {
                shortages.Add(
                    $"{material.Name}: potřeba {requiredQty:N2} {material.Unit}, " +
                    $"na skladě {material.CurrentStock:N2} {material.Unit}");
            }
        }

        if (shortages.Count > 0)
        {
            var detail = string.Join("; ", shortages);
            throw new DomainException($"Nedostatek materiálů pro výrobu: {detail}");
        }
    }
}
