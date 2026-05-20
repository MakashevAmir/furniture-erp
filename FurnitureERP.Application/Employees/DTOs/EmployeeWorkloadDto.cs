namespace FurnitureERP.Application.Employees.DTOs;

public record EmployeeWorkloadDto(
    int EmployeeId,
    string FullName,
    string Position,
    decimal HourlyRate,
    bool IsActive,
    List<EmployeeOrderAssignmentDto> ActiveOrders
)
{
    public decimal TotalActiveHours => ActiveOrders.Sum(a => a.TotalHours);
}

public record EmployeeOrderAssignmentDto(
    int OrderId,
    string OrderNumber,
    string CustomerName,
    string ProductName,
    decimal HoursPerUnit,
    int Quantity,
    decimal TotalHours,
    int PoolSize,
    DateTime? ExpectedCompletionDate
)
{
    // Přepočítaný podíl hodin zaměstnance z celkového poolu dané pozice.
    public decimal FairShareHours => PoolSize > 1 ? Math.Round(TotalHours / PoolSize, 1) : TotalHours;
}
