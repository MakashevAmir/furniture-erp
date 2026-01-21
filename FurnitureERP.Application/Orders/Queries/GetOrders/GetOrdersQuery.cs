using FurnitureERP.Application.Orders.DTOs;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery() : IRequest<IEnumerable<OrderDto>>;
