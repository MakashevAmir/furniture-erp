using MediatR;

namespace FurnitureERP.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(int OrderId) : IRequest;
