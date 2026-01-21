using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class OrderConfirmedEvent : IDomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public DateTime OccurredOn { get; }

    public OrderConfirmedEvent(int orderId, string orderNumber)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        OccurredOn = DateTime.UtcNow;
    }
}
