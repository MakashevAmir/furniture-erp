using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class Order : AggregateRoot
{
    public string OrderNumber { get; private set; }

    public string CustomerName { get; private set; }

    public string CustomerPhone { get; private set; }

    public string? CustomerEmail { get; private set; }

    public string DeliveryAddress { get; private set; }

    public DateTime OrderDate { get; private set; }

    public DateTime? ExpectedCompletionDate { get; private set; }

    public DateTime? ActualCompletionDate { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalAmount { get; private set; }

    public string Notes { get; private set; }

    private readonly List<OrderItem> _orderItems = new();

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order()
    {
        OrderNumber = string.Empty;
        CustomerName = string.Empty;
        CustomerPhone = string.Empty;
        DeliveryAddress = string.Empty;
        Notes = string.Empty;
    }

    public Order(
        string orderNumber,
        string customerName,
        string customerPhone,
        string deliveryAddress,
        DateTime? expectedCompletionDate = null,
        string? customerEmail = null,
        string notes = "")
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new InvalidOrderDataException("Číslo objednávky nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new InvalidOrderDataException("Jméno zákazníka nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(customerPhone))
            throw new InvalidOrderDataException("Telefon zákazníka nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(deliveryAddress))
            throw new InvalidOrderDataException("Adresa dodání nesmí být prázdná");

        OrderNumber = orderNumber.Trim();
        CustomerName = customerName.Trim();
        CustomerPhone = customerPhone.Trim();
        CustomerEmail = customerEmail?.Trim();
        DeliveryAddress = deliveryAddress.Trim();
        OrderDate = DateTime.UtcNow;
        ExpectedCompletionDate = expectedCompletionDate;
        Status = OrderStatus.Pending;
        TotalAmount = 0;
        Notes = notes?.Trim() ?? string.Empty;

        AddDomainEvent(new OrderCreatedEvent(Id, OrderNumber, CustomerName));
    }

    public void AddOrderItem(OrderItem orderItem)
    {
        if (orderItem == null)
            throw new InvalidOrderDataException("Položka objednávky nesmí být null");

        if (Status != OrderStatus.Pending)
            throw new DomainException($"Nelze přidávat položky do objednávky se stavem {Status}");

        if (_orderItems.Any(oi => oi.ProductId == orderItem.ProductId))
            throw new InvalidOrderDataException($"Výrobek s ID {orderItem.ProductId} je již přidán do objednávky");

        _orderItems.Add(orderItem);
        RecalculateTotalAmount();
        MarkAsUpdated();
    }

    public void RemoveOrderItem(int productId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Nelze odstraňovat položky z objednávky se stavem {Status}");

        var item = _orderItems.FirstOrDefault(oi => oi.ProductId == productId);
        if (item == null)
            throw new InvalidOrderDataException($"Položka s ProductId {productId} nebyla nalezena v objednávce");

        _orderItems.Remove(item);
        RecalculateTotalAmount();
        MarkAsUpdated();
    }

    public void ConfirmOrder(DateTime? calculatedCompletionDate = null)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Nelze potvrdit objednávku se stavem {Status}");

        if (!_orderItems.Any())
            throw new DomainException("Nelze potvrdit objednávku bez položek");

        if (calculatedCompletionDate.HasValue)
            ExpectedCompletionDate = calculatedCompletionDate.Value;

        Status = OrderStatus.InProduction;
        MarkAsUpdated();
        AddDomainEvent(new OrderConfirmedEvent(Id, OrderNumber));
    }

    public void CompleteOrder()
    {
        if (Status != OrderStatus.InProduction)
            throw new DomainException($"Nelze dokončit objednávku se stavem {Status}");

        Status = OrderStatus.Completed;
        ActualCompletionDate = DateTime.UtcNow;
        MarkAsUpdated();
        AddDomainEvent(new OrderCompletedEvent(Id, OrderNumber, ActualCompletionDate.Value));
    }

    public void CancelOrder(string reason)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Delivered)
            throw new DomainException($"Nelze zrušit objednávku se stavem {Status}");

        Status = OrderStatus.Cancelled;
        Notes = string.IsNullOrEmpty(Notes) ? reason : $"{Notes}\nDůvod zrušení: {reason}";
        MarkAsUpdated();
        AddDomainEvent(new OrderCancelledEvent(Id, OrderNumber, reason));
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Completed)
            throw new DomainException($"Nelze dodat objednávku se stavem {Status}");

        Status = OrderStatus.Delivered;
        MarkAsUpdated();
        AddDomainEvent(new OrderDeliveredEvent(Id, OrderNumber, DateTime.UtcNow));
    }

    public void UpdateCustomerInfo(
        string customerName,
        string customerPhone,
        string deliveryAddress,
        string? customerEmail = null,
        string notes = "",
        DateTime? expectedCompletionDate = null)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Nelze upravovat objednávku se stavem {Status}");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new InvalidOrderDataException("Jméno zákazníka nesmí být prázdné");

        if (string.IsNullOrWhiteSpace(customerPhone))
            throw new InvalidOrderDataException("Telefon zákazníka nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(deliveryAddress))
            throw new InvalidOrderDataException("Adresa dodání nesmí být prázdná");

        CustomerName = customerName.Trim();
        CustomerPhone = customerPhone.Trim();
        CustomerEmail = customerEmail?.Trim();
        DeliveryAddress = deliveryAddress.Trim();
        Notes = notes?.Trim() ?? string.Empty;
        ExpectedCompletionDate = expectedCompletionDate;
        MarkAsUpdated();
    }

    public void UpdateExpectedCompletionDate(DateTime newDate)
    {
        if (newDate < OrderDate)
            throw new InvalidOrderDataException("Datum dokončení nesmí být dříve než datum vytvoření objednávky");

        ExpectedCompletionDate = newDate;
        MarkAsUpdated();
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = _orderItems.Sum(oi => oi.Subtotal);
    }
}

public enum OrderStatus
{
    Pending = 0,

    InProduction = 1,

    Completed = 2,

    Delivered = 3,

    Cancelled = 4
}
