using FurnitureERP.Application.Employees.DTOs;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployees;

public record GetEmployeesQuery() : IRequest<IEnumerable<EmployeeDto>>;
