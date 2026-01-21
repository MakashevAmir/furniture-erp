using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Employees;

public class EmployeeCreatedEvent : IDomainEvent
{
    public int EmployeeId { get; }

    public string FullName { get; }

    public string Position { get; }

    public DateTime OccurredOn { get; }

    public EmployeeCreatedEvent(int employeeId, string fullName, string position)
    {
        EmployeeId = employeeId;
        FullName = fullName;
        Position = position;
        OccurredOn = DateTime.UtcNow;
    }
}
