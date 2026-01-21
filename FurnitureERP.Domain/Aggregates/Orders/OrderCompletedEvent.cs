using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class OrderCompletedEvent : IDomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public DateTime CompletionDate { get; }
    public DateTime OccurredOn { get; }

    public OrderCompletedEvent(int orderId, string orderNumber, DateTime completionDate)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CompletionDate = completionDate;
        OccurredOn = DateTime.UtcNow;
    }
}
