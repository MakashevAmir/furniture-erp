using MediatR;

namespace FurnitureERP.Application.Orders.Commands.CompleteOrder;

public record CompleteOrderCommand(int OrderId) : IRequest;
