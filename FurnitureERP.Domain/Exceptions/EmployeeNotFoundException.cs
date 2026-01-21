namespace FurnitureERP.Domain.Exceptions;

public class EmployeeNotFoundException : DomainException
{
    public EmployeeNotFoundException(int employeeId)
        : base($"Zaměstnanec s identifikátorem {employeeId} nebyl nalezen")
    {
    }

    public EmployeeNotFoundException(string message) : base(message)
    {
    }
}
