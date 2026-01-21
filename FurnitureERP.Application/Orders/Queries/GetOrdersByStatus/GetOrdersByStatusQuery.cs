using FurnitureERP.Application.Orders.DTOs;
using FurnitureERP.Domain.Aggregates.Orders;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetOrdersByStatus;

public record GetOrdersByStatusQuery(OrderStatus Status) : IRequest<IEnumerable<OrderDto>>;
