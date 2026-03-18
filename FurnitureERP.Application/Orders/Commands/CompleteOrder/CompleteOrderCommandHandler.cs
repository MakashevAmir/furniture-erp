using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.CompleteOrder;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IStockTransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IMaterialRepository materialRepository,
        IStockTransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _materialRepository = materialRepository ?? throw new ArgumentNullException(nameof(materialRepository));
        _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new NotFoundException($"Objednávka s ID {request.OrderId} nebyla nalezena");

        var requiredByMaterial = await CalculateRequiredMaterialsAsync(order.OrderItems, cancellationToken);

        await DeductMaterialsAsync(requiredByMaterial, order.OrderNumber, cancellationToken);

        order.CompleteOrder();

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Dictionary<int, decimal>> CalculateRequiredMaterialsAsync(
        IEnumerable<Domain.Aggregates.Orders.OrderItem> orderItems,
        CancellationToken cancellationToken)
    {
        var required = new Dictionary<int, decimal>();

        foreach (var item in orderItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product == null)
                continue;

            foreach (var bom in product.MaterialBoms)
            {
                var qty = bom.QuantityWithWastage * item.Quantity;
                required[bom.MaterialId] = required.GetValueOrDefault(bom.MaterialId) + qty;
            }
        }

        return required;
    }

    private async Task DeductMaterialsAsync(
        Dictionary<int, decimal> requiredByMaterial,
        string orderNumber,
        CancellationToken cancellationToken)
    {
        foreach (var (materialId, quantity) in requiredByMaterial)
        {
            var material = await _materialRepository.GetByIdAsync(materialId, cancellationToken);
            if (material == null)
                continue;

            var transaction = new StockTransaction(
                material.Id,
                material.Name,
                material.Unit,
                -quantity,
                material.CurrentStock,
                StockTransactionType.OrderConsumption,
                reference: orderNumber);

            material.UpdateStock(-quantity);

            _materialRepository.Update(material);
            await _transactionRepository.AddAsync(transaction, cancellationToken);
        }
    }
}
