using FurnitureERP.Application.Interfaces;
using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var orderNumber = await GenerateOrderNumberAsync(cancellationToken);

        var order = new Order(
            orderNumber,
            request.CustomerName,
            request.CustomerPhone,
            request.DeliveryAddress,
            request.ExpectedCompletionDate,
            request.CustomerEmail,
            request.Notes ?? string.Empty
        );

        await _orderRepository.AddAsync(order, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }

    private async Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken)
    {
        var year = DateTime.UtcNow.Year;
        var lastOrder = await Task.Run(() =>
            _orderRepository.GetAll()
                .Where(o => o.OrderNumber.StartsWith($"ORD-{year}"))
                .OrderByDescending(o => o.OrderNumber)
                .FirstOrDefault(), cancellationToken);

        if (lastOrder == null)
            return $"ORD-{year}-001";

        var lastNumberPart = lastOrder.OrderNumber.Split('-').Last();
        if (int.TryParse(lastNumberPart, out int lastNumber))
        {
            var nextNumber = lastNumber + 1;
            return $"ORD-{year}-{nextNumber:D3}";
        }

        return $"ORD-{year}-001";
    }
}
