using FurnitureERP.Application.Common.Exceptions;
using FurnitureERP.Application.Employees.DTOs;
using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployeeWorkload;

public class GetEmployeeWorkloadQueryHandler : IRequestHandler<GetEmployeeWorkloadQuery, EmployeeWorkloadDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public GetEmployeeWorkloadQueryHandler(
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<EmployeeWorkloadDto> Handle(GetEmployeeWorkloadQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
            throw new NotFoundException($"Zaměstnanec s ID {request.EmployeeId} nebyl nalezen");

        // Velikost poolu: počet aktivních zaměstnanců se stejnou pozicí
        var poolSize = _employeeRepository
            .GetActiveEmployees()
            .Count(e => e.Position == employee.Position);

        poolSize = Math.Max(1, poolSize);

        var inProductionOrders = _orderRepository
            .GetOrdersByStatus(OrderStatus.InProduction)
            .ToList();

        var assignments = new List<EmployeeOrderAssignmentDto>();

        foreach (var order in inProductionOrders)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                if (product == null)
                    continue;

                // LaborBomy pro pozici tohoto zaměstnance
                // (práce se dělí mezi všechny zaměstnance se stejnou pozicí)
                var employeeLaborBoms = product.LaborBoms
                    .Where(lb => lb.Position == employee.Position)
                    .ToList();

                if (!employeeLaborBoms.Any())
                    continue;

                var hoursPerUnit = employeeLaborBoms.Sum(lb => lb.HoursRequired);
                var totalHours = hoursPerUnit * item.Quantity;

                assignments.Add(new EmployeeOrderAssignmentDto(
                    OrderId: order.Id,
                    OrderNumber: order.OrderNumber,
                    CustomerName: order.CustomerName,
                    ProductName: item.ProductName,
                    HoursPerUnit: Math.Round(hoursPerUnit / poolSize, 2),
                    Quantity: item.Quantity,
                    TotalHours: totalHours,
                    PoolSize: poolSize,
                    ExpectedCompletionDate: order.ExpectedCompletionDate
                ));
            }
        }

        return new EmployeeWorkloadDto(
            EmployeeId: employee.Id,
            FullName: employee.FullName,
            Position: employee.Position,
            HourlyRate: employee.HourlyRate,
            IsActive: employee.IsActive,
            ActiveOrders: assignments.OrderBy(a => a.ExpectedCompletionDate).ToList()
        );
    }
}
