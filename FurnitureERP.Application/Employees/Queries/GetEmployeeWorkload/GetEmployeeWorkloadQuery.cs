using FurnitureERP.Application.Employees.DTOs;
using MediatR;

namespace FurnitureERP.Application.Employees.Queries.GetEmployeeWorkload;

public record GetEmployeeWorkloadQuery(int EmployeeId) : IRequest<EmployeeWorkloadDto>;
