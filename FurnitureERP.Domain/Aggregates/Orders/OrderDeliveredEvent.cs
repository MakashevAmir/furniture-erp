using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class OrderDeliveredEvent : IDomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public DateTime DeliveryDate { get; }
    public DateTime OccurredOn { get; }

    public OrderDeliveredEvent(int orderId, string orderNumber, DateTime deliveryDate)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        DeliveryDate = deliveryDate;
        OccurredOn = DateTime.UtcNow;
    }
}
