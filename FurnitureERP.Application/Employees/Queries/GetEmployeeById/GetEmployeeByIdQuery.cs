using FurnitureERP.Application.Employees.DTOs;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeDto>;
