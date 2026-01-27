using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOrder()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1", null, "john@email.com", "Test order");

        order.OrderNumber.Should().Be("ORD-001");
        order.CustomerName.Should().Be("John Doe");
        order.CustomerPhone.Should().Be("+420123456789");
        order.DeliveryAddress.Should().Be("Prague 1");
        order.CustomerEmail.Should().Be("john@email.com");
        order.Notes.Should().Be("Test order");
        order.Status.Should().Be(OrderStatus.Pending);
        order.TotalAmount.Should().Be(0m);
    }

    [Fact]
    public void Create_WithEmptyOrderNumber_ShouldThrowException()
    {
        var action = () => new Order("", "John Doe", "+420123456789", "Prague 1");

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithEmptyCustomerName_ShouldThrowException()
    {
        var action = () => new Order("ORD-001", "", "+420123456789", "Prague 1");

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithEmptyPhone_ShouldThrowException()
    {
        var action = () => new Order("ORD-001", "John Doe", "", "Prague 1");

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithEmptyAddress_ShouldThrowException()
    {
        var action = () => new Order("ORD-001", "John Doe", "+420123456789", "");

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void ConfirmOrder_WhenPending_ShouldChangeStatusToInProduction()
    {
        var order = CreateOrderWithItem();

        order.ConfirmOrder();

        order.Status.Should().Be(OrderStatus.InProduction);
    }

    [Fact]
    public void ConfirmOrder_WhenNoItems_ShouldThrowException()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");

        var action = () => order.ConfirmOrder();

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void ConfirmOrder_WhenNotPending_ShouldThrowException()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();

        var action = () => order.ConfirmOrder();

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void CompleteOrder_WhenInProduction_ShouldChangeStatusToCompleted()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();

        order.CompleteOrder();

        order.Status.Should().Be(OrderStatus.Completed);
        order.ActualCompletionDate.Should().NotBeNull();
    }

    [Fact]
    public void CompleteOrder_WhenPending_ShouldThrowException()
    {
        var order = CreateOrderWithItem();

        var action = () => order.CompleteOrder();

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void CancelOrder_WhenPending_ShouldChangeStatusToCancelled()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");

        order.CancelOrder("Customer request");

        order.Status.Should().Be(OrderStatus.Cancelled);
        order.Notes.Should().Contain("Customer request");
    }

    [Fact]
    public void CancelOrder_WhenCompleted_ShouldThrowException()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();
        order.CompleteOrder();

        var action = () => order.CancelOrder("Test");

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkAsDelivered_WhenCompleted_ShouldChangeStatusToDelivered()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();
        order.CompleteOrder();

        order.MarkAsDelivered();

        order.Status.Should().Be(OrderStatus.Delivered);
    }

    [Fact]
    public void MarkAsDelivered_WhenNotCompleted_ShouldThrowException()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();

        var action = () => order.MarkAsDelivered();

        action.Should().Throw<DomainException>();
    }

    private static Order CreateOrderWithItem()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");
        var orderItem = new OrderItem(1, 1, "Table", "TBL-001", 2, 1500m);

        var orderItemsField = typeof(Order).GetField("_orderItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var orderItems = (List<OrderItem>)orderItemsField!.GetValue(order)!;
        orderItems.Add(orderItem);

        return order;
    }
}
