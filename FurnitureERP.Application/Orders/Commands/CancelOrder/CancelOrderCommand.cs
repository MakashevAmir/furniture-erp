using MediatR;

namespace FurnitureERP.Application.Orders.Commands.CancelOrder;

public record CancelOrderCommand(
    int OrderId,
    string Reason
) : IRequest;
