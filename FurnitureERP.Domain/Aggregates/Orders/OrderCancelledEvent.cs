using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class OrderCancelledEvent : IDomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public string Reason { get; }
    public DateTime OccurredOn { get; }

    public OrderCancelledEvent(int orderId, string orderNumber, string reason)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        Reason = reason;
        OccurredOn = DateTime.UtcNow;
    }
}
