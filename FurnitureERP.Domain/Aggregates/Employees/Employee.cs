using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Employees;

public class Employee : AggregateRoot
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Position { get; private set; }

    public decimal HourlyRate { get; private set; }

    public bool IsActive { get; private set; }

    public string FullName => $"{LastName} {FirstName}";

    private Employee()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Position = string.Empty;
    }

    public Employee(string firstName, string lastName, string position, decimal hourlyRate)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new InvalidEmployeeDataException("Jméno zaměstnance nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new InvalidEmployeeDataException("Příjmení zaměstnance nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(position))
            throw new InvalidEmployeeDataException("Pozice zaměstnance nesmí být prázdná");

        if (hourlyRate <= 0)
            throw new InvalidEmployeeDataException($"Hodinová sazba musí být větší než 0, získáno: {hourlyRate}");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Position = position.Trim();
        HourlyRate = hourlyRate;
        IsActive = true;

        AddDomainEvent(new EmployeeCreatedEvent(Id, FullName, Position));
    }

    public void Update(string firstName, string lastName, string position, decimal hourlyRate)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new InvalidEmployeeDataException("Jméno zaměstnance nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new InvalidEmployeeDataException("Příjmení zaměstnance nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(position))
            throw new InvalidEmployeeDataException("Pozice zaměstnance nesmí být prázdná");

        if (hourlyRate <= 0)
            throw new InvalidEmployeeDataException($"Hodinová sazba musí být větší než 0, získáno: {hourlyRate}");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Position = position.Trim();
        HourlyRate = hourlyRate;

        MarkAsUpdated();
    }

    public void UpdateHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate <= 0)
            throw new InvalidEmployeeDataException($"Hodinová sazba musí být větší než 0, získáno: {hourlyRate}");

        HourlyRate = hourlyRate;
        MarkAsUpdated();
    }

    public decimal CalculateLaborCost(decimal hours)
    {
        if (hours < 0)
            throw new InvalidEmployeeDataException($"Počet hodin nesmí být záporný, získáno: {hours}");

        return hours * HourlyRate;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException($"Zaměstnanec '{FullName}' je již deaktivován");

        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException($"Zaměstnanec '{FullName}' je již aktivní");

        IsActive = true;
        MarkAsUpdated();
    }
}
