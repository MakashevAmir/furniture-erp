using MediatR;

namespace FurnitureERP.Application.Orders.Commands.RemoveOrderItem;

public record RemoveOrderItemCommand(int OrderId, int ProductId) : IRequest;
