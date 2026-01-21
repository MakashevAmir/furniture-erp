namespace FurnitureERP.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
