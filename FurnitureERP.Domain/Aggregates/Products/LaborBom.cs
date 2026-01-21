using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Products;

public class LaborBom : Entity
{
    public int ProductId { get; private set; }

    public int? EmployeeId { get; private set; }

    public string Position { get; private set; }

    public decimal HoursRequired { get; private set; }

    public string Description { get; private set; }

    public int? SequenceNumber { get; private set; }

    private LaborBom()
    {
        Position = string.Empty;
        Description = string.Empty;
    }

    public LaborBom(
        int productId,
        string position,
        decimal hoursRequired,
        string description,
        int? employeeId = null,
        int? sequenceNumber = null)
    {
        if (productId < 0)
            throw new InvalidProductDataException($"Neplatný identifikátor výrobku: {productId}");

        if (string.IsNullOrWhiteSpace(position))
            throw new InvalidProductDataException("Pozice pro provedení práce nesmí být prázdná");

        if (hoursRequired <= 0)
            throw new InvalidProductDataException(
                $"Požadovaný počet hodin musí být větší než 0, získáno: {hoursRequired}");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidProductDataException("Popis práce nesmí být prázdný");

        if (employeeId.HasValue && employeeId.Value <= 0)
            throw new InvalidProductDataException($"Neplatný identifikátor zaměstnance: {employeeId}");

        if (sequenceNumber.HasValue && sequenceNumber.Value < 0)
            throw new InvalidProductDataException($"Pořadové číslo nesmí být záporné: {sequenceNumber}");

        ProductId = productId;
        Position = position.Trim();
        HoursRequired = hoursRequired;
        Description = description.Trim();
        EmployeeId = employeeId;
        SequenceNumber = sequenceNumber;
    }

    public void UpdateHoursRequired(decimal hoursRequired)
    {
        if (hoursRequired <= 0)
            throw new InvalidProductDataException(
                $"Požadovaný počet hodin musí být větší než 0, získáno: {hoursRequired}");

        HoursRequired = hoursRequired;
        MarkAsUpdated();
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidProductDataException("Popis práce nesmí být prázdný");

        Description = description.Trim();
        MarkAsUpdated();
    }

    public void UpdatePosition(string position)
    {
        if (string.IsNullOrWhiteSpace(position))
            throw new InvalidProductDataException("Pozice pro provedení práce nesmí být prázdná");

        Position = position.Trim();
        MarkAsUpdated();
    }

    public void AssignEmployee(int employeeId)
    {
        if (employeeId <= 0)
            throw new InvalidProductDataException($"Neplatný identifikátor zaměstnance: {employeeId}");

        EmployeeId = employeeId;
        MarkAsUpdated();
    }

    public void UnassignEmployee()
    {
        EmployeeId = null;
        MarkAsUpdated();
    }

    public void SetSequenceNumber(int sequenceNumber)
    {
        if (sequenceNumber < 0)
            throw new InvalidProductDataException($"Pořadové číslo nesmí být záporné: {sequenceNumber}");

        SequenceNumber = sequenceNumber;
        MarkAsUpdated();
    }

    public decimal CalculateLaborCost(decimal hourlyRate)
    {
        if (hourlyRate < 0)
            throw new InvalidProductDataException(
                $"Hodinová sazba nesmí být záporná, získáno: {hourlyRate}");

        return HoursRequired * hourlyRate;
    }
}
