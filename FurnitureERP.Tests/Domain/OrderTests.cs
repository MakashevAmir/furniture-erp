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

    [Fact]
    public void UpdateCustomerInfo_WhenPending_ShouldUpdateFields()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");

        order.UpdateCustomerInfo("Jane Smith", "+420987654321", "Brno 2", "jane@email.com", "Updated notes", DateTime.UtcNow.AddDays(10));

        order.CustomerName.Should().Be("Jane Smith");
        order.CustomerPhone.Should().Be("+420987654321");
        order.DeliveryAddress.Should().Be("Brno 2");
        order.CustomerEmail.Should().Be("jane@email.com");
        order.Notes.Should().Be("Updated notes");
        order.ExpectedCompletionDate.Should().NotBeNull();
    }

    [Fact]
    public void UpdateCustomerInfo_WhenNotPending_ShouldThrowException()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();

        var action = () => order.UpdateCustomerInfo("Jane Smith", "+420987654321", "Brno 2");

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateCustomerInfo_WithEmptyName_ShouldThrowException()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");

        var action = () => order.UpdateCustomerInfo("", "+420987654321", "Brno 2");

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void RemoveOrderItem_WhenPending_ShouldRemoveItem()
    {
        var order = CreateOrderWithItem();

        order.RemoveOrderItem(1);

        order.OrderItems.Should().BeEmpty();
        order.TotalAmount.Should().Be(0m);
    }

    [Fact]
    public void RemoveOrderItem_WhenNotPending_ShouldThrowException()
    {
        var order = CreateOrderWithItem();
        order.ConfirmOrder();

        var action = () => order.RemoveOrderItem(1);

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void RemoveOrderItem_WithNonExistentProductId_ShouldThrowException()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");

        var action = () => order.RemoveOrderItem(999);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void AddOrderItem_WhenPending_ShouldRecalculateTotalAmount()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");
        var item1 = new OrderItem(1, 1, "Table", "TBL-001", 2, 1500m);
        var item2 = new OrderItem(1, 2, "Chair", "CHR-001", 4, 500m);

        order.AddOrderItem(item1);
        order.AddOrderItem(item2);

        order.TotalAmount.Should().Be(5000m); // 2*1500 + 4*500
    }

    [Fact]
    public void AddOrderItem_DuplicateProduct_ShouldThrowException()
    {
        var order = new Order("ORD-001", "John Doe", "+420123456789", "Prague 1");
        var item1 = new OrderItem(1, 1, "Table", "TBL-001", 2, 1500m);
        var item2 = new OrderItem(1, 1, "Table", "TBL-001", 1, 1500m);

        order.AddOrderItem(item1);
        var action = () => order.AddOrderItem(item2);

        action.Should().Throw<InvalidOrderDataException>();
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
