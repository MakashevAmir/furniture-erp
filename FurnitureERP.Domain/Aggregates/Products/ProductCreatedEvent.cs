using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Products;

public class ProductCreatedEvent : IDomainEvent
{
    public int ProductId { get; }

    public string Name { get; }

    public string Article { get; }

    public DateTime OccurredOn { get; }

    public ProductCreatedEvent(int productId, string name, string article)
    {
        ProductId = productId;
        Name = name;
        Article = article;
        OccurredOn = DateTime.UtcNow;
    }
}
