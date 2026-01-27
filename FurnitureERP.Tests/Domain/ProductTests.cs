using FurnitureERP.Domain.Aggregates.Products;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateProduct()
    {
        var product = new Product("Table", "Wooden table", "Furniture", "TBL-001", 1000m, 1500m);

        product.Name.Should().Be("Table");
        product.Description.Should().Be("Wooden table");
        product.Category.Should().Be("Furniture");
        product.Article.Should().Be("TBL-001");
        product.BasePrice.Should().Be(1000m);
        product.SalePrice.Should().Be(1500m);
        product.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowException()
    {
        var action = () => new Product("", "Description", "Category", "ART-001", 100m, 150m);

        action.Should().Throw<InvalidProductDataException>();
    }

    [Fact]
    public void Create_WithNegativeBasePrice_ShouldThrowException()
    {
        var action = () => new Product("Name", "Description", "Category", "ART-001", -100m, 150m);

        action.Should().Throw<InvalidProductDataException>();
    }

    [Fact]
    public void Create_WithZeroSalePrice_ShouldThrowException()
    {
        var action = () => new Product("Name", "Description", "Category", "ART-001", 100m, 0m);

        action.Should().Throw<InvalidProductDataException>();
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateProduct()
    {
        var product = new Product("Table", "Old description", "Furniture", "TBL-001", 1000m, 1500m);

        product.Update("Updated Table", "New description", "New Category", 1200m, 1800m);

        product.Name.Should().Be("Updated Table");
        product.Description.Should().Be("New description");
        product.Category.Should().Be("New Category");
        product.BasePrice.Should().Be(1200m);
        product.SalePrice.Should().Be(1800m);
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldDeactivate()
    {
        var product = new Product("Table", "Description", "Category", "TBL-001", 1000m, 1500m);

        product.Deactivate();

        product.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_ShouldThrowException()
    {
        var product = new Product("Table", "Description", "Category", "TBL-001", 1000m, 1500m);
        product.Deactivate();

        var action = () => product.Deactivate();

        action.Should().Throw<DomainException>();
    }

    [Fact]
    public void Activate_WhenInactive_ShouldActivate()
    {
        var product = new Product("Table", "Description", "Category", "TBL-001", 1000m, 1500m);
        product.Deactivate();

        product.Activate();

        product.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateBasePrice_WithValidPrice_ShouldUpdate()
    {
        var product = new Product("Table", "Description", "Category", "TBL-001", 1000m, 1500m);

        product.UpdateBasePrice(1200m);

        product.BasePrice.Should().Be(1200m);
    }

    [Fact]
    public void UpdateSalePrice_WithValidPrice_ShouldUpdate()
    {
        var product = new Product("Table", "Description", "Category", "TBL-001", 1000m, 1500m);

        product.UpdateSalePrice(2000m);

        product.SalePrice.Should().Be(2000m);
    }
}
