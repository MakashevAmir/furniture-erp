using FurnitureERP.Domain.Common;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class OrderCreatedEvent : IDomainEvent
{
    public int OrderId { get; }
    public string OrderNumber { get; }
    public string CustomerName { get; }
    public DateTime OccurredOn { get; }

    public OrderCreatedEvent(int orderId, string orderNumber, string customerName)
    {
        OrderId = orderId;
        OrderNumber = orderNumber;
        CustomerName = customerName;
        OccurredOn = DateTime.UtcNow;
    }
}
