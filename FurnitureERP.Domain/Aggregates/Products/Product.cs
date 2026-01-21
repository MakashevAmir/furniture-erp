using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Products;

public class Product : AggregateRoot
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public string Category { get; private set; }

    public string Article { get; private set; }

    public decimal BasePrice { get; private set; }

    public decimal SalePrice { get; private set; }

    public bool IsActive { get; private set; }

    private readonly List<MaterialBom> _materialBoms = new();
    private readonly List<LaborBom> _laborBoms = new();

    public IReadOnlyCollection<MaterialBom> MaterialBoms => _materialBoms.AsReadOnly();

    public IReadOnlyCollection<LaborBom> LaborBoms => _laborBoms.AsReadOnly();

    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
        Category = string.Empty;
        Article = string.Empty;
    }

    public Product(string name, string description, string category, string article, decimal basePrice, decimal salePrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidProductDataException("Název výrobku nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidProductDataException("Popis výrobku nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(category))
            throw new InvalidProductDataException("Kategorie výrobku nesmí být prázdná");

        if (string.IsNullOrWhiteSpace(article))
            throw new InvalidProductDataException("Kód výrobku nesmí být prázdný");

        if (basePrice < 0)
            throw new InvalidProductDataException($"Základní cena nesmí být záporná, získáno: {basePrice}");

        if (salePrice <= 0)
            throw new InvalidProductDataException($"Prodejní cena musí být větší než 0, získáno: {salePrice}");

        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        Article = article.Trim();
        BasePrice = basePrice;
        SalePrice = salePrice;
        IsActive = true;

        AddDomainEvent(new ProductCreatedEvent(Id, Name, Article));
    }

    public void Update(string name, string description, string category, decimal basePrice, decimal salePrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidProductDataException("Název výrobku nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidProductDataException("Popis výrobku nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(category))
            throw new InvalidProductDataException("Kategorie výrobku nesmí být prázdná");

        if (basePrice < 0)
            throw new InvalidProductDataException($"Základní cena nesmí být záporná, získáno: {basePrice}");

        if (salePrice <= 0)
            throw new InvalidProductDataException($"Prodejní cena musí být větší než 0, získáno: {salePrice}");

        Name = name.Trim();
        Description = description.Trim();
        Category = category.Trim();
        BasePrice = basePrice;
        SalePrice = salePrice;

        MarkAsUpdated();
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException($"Výrobek '{Name}' je již deaktivován");

        IsActive = false;
        MarkAsUpdated();
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException($"Výrobek '{Name}' je již aktivní");

        IsActive = true;
        MarkAsUpdated();
    }

    public void UpdateArticle(string article)
    {
        if (string.IsNullOrWhiteSpace(article))
            throw new InvalidProductDataException("Kód výrobku nesmí být prázdný");

        Article = article.Trim();
        MarkAsUpdated();
    }

    public void UpdateBasePrice(decimal basePrice)
    {
        if (basePrice < 0)
            throw new InvalidProductDataException($"Základní cena nesmí být záporná, získáno: {basePrice}");

        BasePrice = basePrice;
        MarkAsUpdated();
    }

    public void UpdateSalePrice(decimal salePrice)
    {
        if (salePrice < 0)
            throw new InvalidProductDataException($"Prodejní cena nesmí být záporná, získáno: {salePrice}");

        SalePrice = salePrice;
        MarkAsUpdated();
    }

    public void AddMaterialBom(MaterialBom materialBom)
    {
        if (materialBom == null)
            throw new InvalidProductDataException("Specifikace materiálu nesmí být null");

        if (_materialBoms.Any(mb => mb.MaterialId == materialBom.MaterialId))
            throw new InvalidProductDataException(
                $"Materiál s identifikátorem {materialBom.MaterialId} již přidán do specifikace výrobku");

        _materialBoms.Add(materialBom);
        MarkAsUpdated();
    }

    public void RemoveMaterialBom(int materialId)
    {
        var materialBom = _materialBoms.FirstOrDefault(mb => mb.MaterialId == materialId);
        if (materialBom == null)
            throw new InvalidProductDataException(
                $"Materiál s identifikátorem {materialId} nebyl nalezen ve specifikaci výrobku");

        _materialBoms.Remove(materialBom);
        MarkAsUpdated();
    }

    public void AddLaborBom(LaborBom laborBom)
    {
        if (laborBom == null)
            throw new InvalidProductDataException("Specifikace pracovních nákladů nesmí být null");

        _laborBoms.Add(laborBom);
        MarkAsUpdated();
    }

    public void RemoveLaborBom(int laborBomId)
    {
        var laborBom = _laborBoms.FirstOrDefault(lb => lb.Id == laborBomId);
        if (laborBom == null)
            throw new InvalidProductDataException(
                $"Specifikace pracovních nákladů s identifikátorem {laborBomId} nebyla nalezena");

        _laborBoms.Remove(laborBom);
        MarkAsUpdated();
    }

    public decimal CalculateTotalMaterialCost(Func<int, decimal> getMaterialPrice)
    {
        if (getMaterialPrice == null)
            throw new ArgumentNullException(nameof(getMaterialPrice));

        return _materialBoms.Sum(mb => mb.CalculateMaterialCost(getMaterialPrice(mb.MaterialId)));
    }

    public decimal CalculateTotalLaborCost(Func<LaborBom, decimal> getHourlyRate)
    {
        if (getHourlyRate == null)
            throw new ArgumentNullException(nameof(getHourlyRate));

        return _laborBoms.Sum(lb => lb.CalculateLaborCost(getHourlyRate(lb)));
    }

    public decimal CalculateTotalProductionCost(
        Func<int, decimal> getMaterialPrice,
        Func<LaborBom, decimal> getHourlyRate)
    {
        var materialCost = CalculateTotalMaterialCost(getMaterialPrice);
        var laborCost = CalculateTotalLaborCost(getHourlyRate);
        return materialCost + laborCost;
    }

    public decimal CalculateTotalCost(
        Func<int, decimal> getMaterialPrice,
        Func<LaborBom, decimal> getHourlyRate)
    {
        var productionCost = CalculateTotalProductionCost(getMaterialPrice, getHourlyRate);
        return productionCost + BasePrice;
    }
}
