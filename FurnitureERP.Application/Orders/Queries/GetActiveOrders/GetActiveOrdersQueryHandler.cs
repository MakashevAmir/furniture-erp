using AutoMapper;
using FurnitureERP.Application.Orders.DTOs;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Orders.Queries.GetActiveOrders;

public class GetActiveOrdersQueryHandler : IRequestHandler<GetActiveOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetActiveOrdersQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<IEnumerable<OrderDto>> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var orders = _orderRepository
            .GetActiveOrders()
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return Task.FromResult(_mapper.Map<IEnumerable<OrderDto>>(orders));
    }
}
