using MediatR;

namespace FurnitureERP.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(
    int OrderId,
    string CustomerName,
    string CustomerPhone,
    string DeliveryAddress,
    string? CustomerEmail = null,
    string Notes = "",
    DateTime? ExpectedCompletionDate = null
) : IRequest;
