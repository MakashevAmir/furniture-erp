using FurnitureERP.Domain.Aggregates.Orders;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Tests.Domain;

public class OrderItemTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateOrderItem()
    {
        var orderItem = new OrderItem(1, 10, "Dining Table", "TBL-001", 2, 1500m, "Special finish");

        orderItem.OrderId.Should().Be(1);
        orderItem.ProductId.Should().Be(10);
        orderItem.ProductName.Should().Be("Dining Table");
        orderItem.ProductArticle.Should().Be("TBL-001");
        orderItem.Quantity.Should().Be(2);
        orderItem.UnitPrice.Should().Be(1500m);
        orderItem.Notes.Should().Be("Special finish");
    }

    [Fact]
    public void Create_WithInvalidOrderId_ShouldThrowException()
    {
        var action = () => new OrderItem(0, 10, "Table", "TBL-001", 2, 1500m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithInvalidProductId_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 0, "Table", "TBL-001", 2, 1500m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithEmptyProductName_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 10, "", "TBL-001", 2, 1500m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithEmptyArticle_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 10, "Table", "", 2, 1500m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithZeroQuantity_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 10, "Table", "TBL-001", 0, 1500m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithNegativeQuantity_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 10, "Table", "TBL-001", -1, 1500m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithZeroPrice_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 10, "Table", "TBL-001", 2, 0m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowException()
    {
        var action = () => new OrderItem(1, 10, "Table", "TBL-001", 2, -100m);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void Subtotal_ShouldCalculateCorrectly()
    {
        var orderItem = new OrderItem(1, 10, "Table", "TBL-001", 3, 1000m);

        orderItem.Subtotal.Should().Be(3000m);
    }

    [Fact]
    public void Subtotal_WithSingleItem_ShouldEqualUnitPrice()
    {
        var orderItem = new OrderItem(1, 10, "Table", "TBL-001", 1, 1500m);

        orderItem.Subtotal.Should().Be(1500m);
    }

    [Fact]
    public void UpdateQuantity_WithValidQuantity_ShouldUpdate()
    {
        var orderItem = new OrderItem(1, 10, "Table", "TBL-001", 2, 1500m);

        orderItem.UpdateQuantity(5);

        orderItem.Quantity.Should().Be(5);
        orderItem.Subtotal.Should().Be(7500m);
    }

    [Fact]
    public void UpdateQuantity_WithZero_ShouldThrowException()
    {
        var orderItem = new OrderItem(1, 10, "Table", "TBL-001", 2, 1500m);

        var action = () => orderItem.UpdateQuantity(0);

        action.Should().Throw<InvalidOrderDataException>();
    }

    [Fact]
    public void UpdateNotes_WithValidNotes_ShouldUpdate()
    {
        var orderItem = new OrderItem(1, 10, "Table", "TBL-001", 2, 1500m);

        orderItem.UpdateNotes("Updated notes");

        orderItem.Notes.Should().Be("Updated notes");
    }

    [Fact]
    public void UpdateNotes_WithNull_ShouldSetEmpty()
    {
        var orderItem = new OrderItem(1, 10, "Table", "TBL-001", 2, 1500m, "Initial notes");

        orderItem.UpdateNotes(null!);

        orderItem.Notes.Should().BeEmpty();
    }
}
