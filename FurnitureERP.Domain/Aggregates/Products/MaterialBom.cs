using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Products;

public class MaterialBom : Entity
{
    public int ProductId { get; private set; }

    public int MaterialId { get; private set; }

    public decimal QuantityRequired { get; private set; }

    public decimal WastagePercentage { get; private set; }

    public string Notes { get; private set; }

    public decimal QuantityWithWastage => QuantityRequired * (1 + WastagePercentage / 100m);

    private MaterialBom()
    {
        Notes = string.Empty;
    }

    public MaterialBom(
        int productId,
        int materialId,
        decimal quantityRequired,
        decimal wastagePercentage = 0,
        string notes = "")
    {
        if (productId < 0)
            throw new InvalidProductDataException($"Neplatný identifikátor výrobku: {productId}");

        if (materialId <= 0)
            throw new InvalidProductDataException($"Neplatný identifikátor materiálu: {materialId}");

        if (quantityRequired <= 0)
            throw new InvalidProductDataException(
                $"Požadované množství materiálu musí být větší než 0, získáno: {quantityRequired}");

        if (wastagePercentage < 0 || wastagePercentage > 100)
            throw new InvalidProductDataException(
                $"Procento odpadu musí být od 0 do 100, získáno: {wastagePercentage}");

        ProductId = productId;
        MaterialId = materialId;
        QuantityRequired = quantityRequired;
        WastagePercentage = wastagePercentage;
        Notes = notes?.Trim() ?? string.Empty;
    }

    public void UpdateQuantity(decimal quantityRequired)
    {
        if (quantityRequired <= 0)
            throw new InvalidProductDataException(
                $"Požadované množství materiálu musí být větší než 0, získáno: {quantityRequired}");

        QuantityRequired = quantityRequired;
        MarkAsUpdated();
    }

    public void UpdateWastagePercentage(decimal wastagePercentage)
    {
        if (wastagePercentage < 0 || wastagePercentage > 100)
            throw new InvalidProductDataException(
                $"Procento odpadu musí být od 0 do 100, získáno: {wastagePercentage}");

        WastagePercentage = wastagePercentage;
        MarkAsUpdated();
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes?.Trim() ?? string.Empty;
        MarkAsUpdated();
    }

    public decimal CalculateMaterialCost(decimal pricePerUnit)
    {
        if (pricePerUnit < 0)
            throw new InvalidProductDataException(
                $"Cena za jednotku materiálu nesmí být záporná, získáno: {pricePerUnit}");

        return QuantityWithWastage * pricePerUnit;
    }
}
