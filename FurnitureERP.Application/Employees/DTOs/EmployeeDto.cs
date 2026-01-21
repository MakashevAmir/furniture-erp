namespace FurnitureERP.Application.Employees.DTOs;

public record EmployeeDto(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    string Position,
    decimal HourlyRate,
    bool IsActive
);
