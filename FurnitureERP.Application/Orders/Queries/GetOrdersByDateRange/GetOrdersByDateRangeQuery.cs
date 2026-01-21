using FurnitureERP.Application.Orders.DTOs;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetOrdersByDateRange;

public record GetOrdersByDateRangeQuery(
    DateTime StartDate,
    DateTime EndDate
) : IRequest<IEnumerable<OrderDto>>;
