using MediatR;

namespace FurnitureERP.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    string CustomerName,
    string CustomerPhone,
    string DeliveryAddress,
    DateTime? ExpectedCompletionDate = null,
    string? CustomerEmail = null,
    string? Notes = null
) : IRequest<int>;
