namespace FurnitureERP.Application.Materials.DTOs;

public record InventoryForecastDto(
    int MaterialId,
    string MaterialName,
    string Category,
    string Unit,
    string Supplier,
    decimal CurrentStock,
    decimal MinimumStock,
    decimal RequiredForOrders,
    decimal ShortfallForOrders,
    decimal RecommendedPurchase
)
{
    public bool HasShortfall => ShortfallForOrders > 0;
    public bool BelowMinimum => CurrentStock < MinimumStock;
    public bool NeedsAttention => HasShortfall || BelowMinimum;
}
