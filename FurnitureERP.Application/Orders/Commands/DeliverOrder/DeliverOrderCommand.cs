using MediatR;

namespace FurnitureERP.Application.Orders.Commands.DeliverOrder;

public record DeliverOrderCommand(int OrderId) : IRequest;
