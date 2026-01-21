using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Materials;

public class MaterialCreatedEvent : IDomainEvent
{
    public int MaterialId { get; }

    public string Name { get; }

    public string Category { get; }

    public DateTime OccurredOn { get; }

    public MaterialCreatedEvent(int materialId, string name, string category)
    {
        MaterialId = materialId;
        Name = name;
        Category = category;
        OccurredOn = DateTime.UtcNow;
    }
}
