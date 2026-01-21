using AutoMapper;
using FurnitureERP.Application.Orders.DTOs;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetOrdersByDateRange;

public class GetOrdersByDateRangeQueryHandler : IRequestHandler<GetOrdersByDateRangeQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersByDateRangeQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<IEnumerable<OrderDto>> Handle(GetOrdersByDateRangeQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.EndDate < request.StartDate)
            throw new ArgumentException("Konečné datum nemůže být před počátečním datem");

        var orders = _orderRepository
            .GetOrdersByDateRange(request.StartDate, request.EndDate)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return Task.FromResult(_mapper.Map<IEnumerable<OrderDto>>(orders));
    }
}
