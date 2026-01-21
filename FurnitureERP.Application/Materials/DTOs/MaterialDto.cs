namespace FurnitureERP.Application.Materials.DTOs;

public record MaterialDto(
    int Id,
    string Name,
    string Description,
    string Category,
    string Unit,
    decimal PricePerUnit,
    decimal CurrentStock,
    decimal MinimumStock,
    string Supplier,
    bool IsActive
);
