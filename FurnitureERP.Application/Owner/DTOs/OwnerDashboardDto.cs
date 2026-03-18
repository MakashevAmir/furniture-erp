namespace FurnitureERP.Application.Owner.DTOs;

public record OwnerDashboardDto(
    decimal Revenue,
    decimal MaterialCosts,
    decimal LaborCosts,
    decimal NetProfit,
    int CompletedOrdersCount,
    int TotalOrdersCount,
    List<TopProductDto> TopProducts,
    List<EmployeeWorkDto> EmployeeWork,
    int Year,
    int Month);

public record TopProductDto(
    string ProductName,
    string Article,
    int TotalQuantity,
    decimal TotalRevenue);

public record EmployeeWorkDto(
    string DisplayName,
    string Position,
    decimal TotalHours,
    decimal LaborCost);
