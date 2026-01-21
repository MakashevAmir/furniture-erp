using MediatR;

namespace FurnitureERP.Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    int Id,
    string FirstName,
    string LastName,
    string Position,
    decimal HourlyRate,
    bool IsActive
) : IRequest;
