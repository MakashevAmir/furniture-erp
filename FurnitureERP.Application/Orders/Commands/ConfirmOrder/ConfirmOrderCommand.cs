using MediatR;

namespace FurnitureERP.Application.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommand(int OrderId) : IRequest;
