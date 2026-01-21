using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Materials;

public class MaterialStockLowEvent : IDomainEvent
{
    public int MaterialId { get; }

    public string Name { get; }

    public decimal CurrentStock { get; }

    public decimal MinimumStock { get; }

    public string Unit { get; }

    public DateTime OccurredOn { get; }

    public MaterialStockLowEvent(int materialId, string name, decimal currentStock, decimal minimumStock, string unit)
    {
        MaterialId = materialId;
        Name = name;
        CurrentStock = currentStock;
        MinimumStock = minimumStock;
        Unit = unit;
        OccurredOn = DateTime.UtcNow;
    }
}
