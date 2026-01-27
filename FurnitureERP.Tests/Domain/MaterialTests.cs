using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Tests.Domain;

public class MaterialTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateMaterial()
    {
        var material = new Material("Oak Wood", "High quality oak", "Wood", "m²", 100m, 50m, 10m, "Supplier A");

        material.Name.Should().Be("Oak Wood");
        material.Description.Should().Be("High quality oak");
        material.Category.Should().Be("Wood");
        material.Unit.Should().Be("m²");
        material.PricePerUnit.Should().Be(100m);
        material.CurrentStock.Should().Be(50m);
        material.MinimumStock.Should().Be(10m);
        material.Supplier.Should().Be("Supplier A");
        material.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowException()
    {
        var action = () => new Material("", "Description", "Category", "pcs", 10m, 100m, 10m, "Supplier");

        action.Should().Throw<InvalidMaterialDataException>();
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowException()
    {
        var action = () => new Material("Name", "Description", "Category", "pcs", -10m, 100m, 10m, "Supplier");

        action.Should().Throw<InvalidMaterialDataException>();
    }

    [Fact]
    public void Create_WithNegativeStock_ShouldThrowException()
    {
        var action = () => new Material("Name", "Description", "Category", "pcs", 10m, -100m, 10m, "Supplier");

        action.Should().Throw<InvalidMaterialDataException>();
    }

    [Fact]
    public void UpdateStock_WithPositiveQuantity_ShouldIncreaseStock()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        material.UpdateStock(20m);

        material.CurrentStock.Should().Be(70m);
    }

    [Fact]
    public void UpdateStock_WithNegativeQuantity_ShouldDecreaseStock()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        material.UpdateStock(-20m);

        material.CurrentStock.Should().Be(30m);
    }

    [Fact]
    public void UpdateStock_WhenResultNegative_ShouldThrowException()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        var action = () => material.UpdateStock(-100m);

        action.Should().Throw<InvalidMaterialDataException>();
    }

    [Fact]
    public void RequiresPurchase_WhenBelowMinimum_ShouldReturnTrue()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 5m, 10m, "Supplier");

        material.RequiresPurchase().Should().BeTrue();
    }

    [Fact]
    public void RequiresPurchase_WhenAboveMinimum_ShouldReturnFalse()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        material.RequiresPurchase().Should().BeFalse();
    }

    [Fact]
    public void ValidateStock_WhenSufficient_ShouldNotThrow()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        var action = () => material.ValidateStock(30m);

        action.Should().NotThrow();
    }

    [Fact]
    public void ValidateStock_WhenInsufficient_ShouldThrowException()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        var action = () => material.ValidateStock(100m);

        action.Should().Throw<InvalidMaterialDataException>();
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldDeactivate()
    {
        var material = new Material("Wood", "Description", "Category", "m²", 100m, 50m, 10m, "Supplier");

        material.Deactivate();

        material.IsActive.Should().BeFalse();
    }
}
