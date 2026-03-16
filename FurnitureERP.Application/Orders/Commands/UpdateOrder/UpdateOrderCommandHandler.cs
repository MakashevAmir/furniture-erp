using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new NotFoundException($"Objednávka s ID {request.OrderId} nebyla nalezena");

        order.UpdateCustomerInfo(
            request.CustomerName,
            request.CustomerPhone,
            request.DeliveryAddress,
            request.CustomerEmail,
            request.Notes ?? string.Empty,
            request.ExpectedCompletionDate
        );

        _orderRepository.Update(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
