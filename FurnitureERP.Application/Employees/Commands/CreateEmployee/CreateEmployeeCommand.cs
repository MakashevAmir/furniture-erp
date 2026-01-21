using MediatR;

namespace FurnitureERP.Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Position,
    decimal HourlyRate
) : IRequest<int>;
