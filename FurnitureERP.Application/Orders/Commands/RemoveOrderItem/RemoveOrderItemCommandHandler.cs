using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.RemoveOrderItem;

public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveOrderItemCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new NotFoundException($"Objednávka s ID {request.OrderId} nebyla nalezena");

        order.RemoveOrderItem(request.ProductId);

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
