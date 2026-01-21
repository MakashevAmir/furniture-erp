using MediatR;

namespace FurnitureERP.Application.Orders.Commands.AddOrderItem;

public record AddOrderItemCommand(
    int OrderId,
    int ProductId,
    int Quantity,
    decimal? UnitPrice = null,
    string? Notes = null
) : IRequest;
