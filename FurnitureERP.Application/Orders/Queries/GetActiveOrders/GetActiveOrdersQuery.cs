using FurnitureERP.Application.Orders.DTOs;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetActiveOrders;

public record GetActiveOrdersQuery() : IRequest<IEnumerable<OrderDto>>;
