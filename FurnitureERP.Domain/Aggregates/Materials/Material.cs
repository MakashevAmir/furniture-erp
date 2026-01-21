using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Materials;

public class Material : AggregateRoot
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public string Category { get; private set; }

    public string Unit { get; private set; }

    public decimal PricePerUnit { get; private set; }

    public decimal CurrentStock { get; private set; }

    public decimal MinimumStock { get; private set; }

    public string Supplier { get; private set; }

    public bool IsActive { get; private set; }

    private Material()
    {
        Name = string.Empty;
        Description = string.Empty;
        Category = string.Empty;
        Unit = string.Empty;
        Supplier = string.Empty;
    }

    public Material(
        string name,
        string description,
        string category,
        string unit,
        decimal pricePerUnit,
        decimal currentStock,
        decimal minimumStock,
        string supplier)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidMaterialDataException("Název materiálu nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidMaterialDataException("Popis materiálu nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(category))
            throw new InvalidMaterialDataException("Kategorie materiálu nesmí být prázdná");

        if (string.IsNullOrWhiteSpace(unit))
            throw new InvalidMaterialDataException("Měrná jednotka nesmí být prázdná");

        if (pricePerUnit < 0)
            throw new InvalidMaterialDataException($"Cena za jednotku nesmí být záporná, získáno: {pricePerUnit}");

        if (currentStock < 0)
            throw new InvalidMaterialDataException($"Současný stav nesmí být záporný, získáno: {currentStock}");

        if (minimumStock < 0)
            throw new InvalidMaterialDataException($"Minimální stav nesmí být záporný, získáno: {minimumStock}");

        if (string.IsNullOrWhiteSpace(supplier))
            throw new InvalidMaterialDataException("Dodavatel nesmí být prázdný");

        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        Unit = unit.Trim();
        PricePerUnit = pricePerUnit;
        CurrentStock = currentStock;
        MinimumStock = minimumStock;
        Supplier = supplier.Trim();
        IsActive = true;

        AddDomainEvent(new MaterialCreatedEvent(Id, Name, Category));
    }

    public void Update(
        string name,
        string description,
        string category,
        string unit,
        decimal pricePerUnit,
        string supplier)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidMaterialDataException("Název materiálu nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidMaterialDataException("Popis materiálu nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(category))
            throw new InvalidMaterialDataException("Kategorie materiálu nesmí být prázdná");

        if (string.IsNullOrWhiteSpace(unit))
            throw new InvalidMaterialDataException("Měrná jednotka nesmí být prázdná");

        if (pricePerUnit < 0)
            throw new InvalidMaterialDataException($"Cena za jednotku nesmí být záporná, získáno: {pricePerUnit}");

        if (string.IsNullOrWhiteSpace(supplier))
            throw new InvalidMaterialDataException("Dodavatel nesmí být prázdný");

        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        Unit = unit.Trim();
        PricePerUnit = pricePerUnit;
        Supplier = supplier.Trim();

        MarkAsUpdated();
    }

    public void UpdateStock(decimal quantity)
    {
        var newStock = CurrentStock + quantity;

        if (newStock < 0)
            throw new InvalidMaterialDataException(
                $"Nedostatek materiálu '{Name}' na skladě. Současný stav: {CurrentStock} {Unit}, požadováno: {Math.Abs(quantity)} {Unit}");

        CurrentStock = newStock;
        MarkAsUpdated();

        if (RequiresPurchase())
        {
            AddDomainEvent(new MaterialStockLowEvent(Id, Name, CurrentStock, MinimumStock, Unit));
        }
    }

    public void SetMinimumStock(decimal minimumStock)
    {
        if (minimumStock < 0)
            throw new InvalidMaterialDataException($"Minimální stav nesmí být záporný, získáno: {minimumStock}");

        MinimumStock = minimumStock;
        MarkAsUpdated();

        if (RequiresPurchase())
        {
            AddDomainEvent(new MaterialStockLowEvent(Id, Name, CurrentStock, MinimumStock, Unit));
        }
    }

    public bool RequiresPurchase()
    {
        return CurrentStock < MinimumStock;
    }

    public void ValidateStock(decimal requiredQuantity)
    {
        if (requiredQuantity < 0)
            throw new InvalidMaterialDataException($"Požadované množství nesmí být záporné, získáno: {requiredQuantity}");

        if (CurrentStock < requiredQuantity)
            throw new InvalidMaterialDataException(
                $"Nedostatek materiálu '{Name}' na skladě. Dostupné: {CurrentStock} {Unit}, požadováno: {requiredQuantity} {Unit}");
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException($"Materiál '{Name}' je již deaktivován");

        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException($"Materiál '{Name}' je již aktivní");

        IsActive = true;
        MarkAsUpdated();
    }
}
