using FurnitureERP.Application.Employees.DTOs;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployeesByPosition;

public record GetEmployeesByPositionQuery(string Position) : IRequest<IEnumerable<EmployeeDto>>;
