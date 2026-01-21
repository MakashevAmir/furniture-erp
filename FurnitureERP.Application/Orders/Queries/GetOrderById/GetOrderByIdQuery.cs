using FurnitureERP.Application.Orders.DTOs;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(int OrderId) : IRequest<OrderDto?>;
