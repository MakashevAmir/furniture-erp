using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Materials;

public class StockTransaction : Entity
{
    public int MaterialId { get; private set; }

    public string MaterialName { get; private set; }

    public string MaterialUnit { get; private set; }

    public decimal Quantity { get; private set; }

    public decimal StockBefore { get; private set; }

    public decimal StockAfter { get; private set; }

    public StockTransactionType Type { get; private set; }

    public string? Reference { get; private set; }

    public string Notes { get; private set; }

    public DateTime TransactionDate { get; private set; }

    private StockTransaction()
    {
        MaterialName = string.Empty;
        MaterialUnit = string.Empty;
        Notes = string.Empty;
    }

    public StockTransaction(
        int materialId,
        string materialName,
        string materialUnit,
        decimal quantity,
        decimal stockBefore,
        StockTransactionType type,
        string? reference = null,
        string notes = "")
    {
        if (materialId <= 0)
            throw new DomainException($"Neplatné ID materiálu: {materialId}");

        if (string.IsNullOrWhiteSpace(materialName))
            throw new DomainException("Název materiálu nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(materialUnit))
            throw new DomainException("Jednotka materiálu nesmí být prázdná");

        if (stockBefore + quantity < 0)
            throw new InvalidMaterialDataException(
                $"Stav skladu nemůže být záporný. Před: {stockBefore}, změna: {quantity}");

        MaterialId = materialId;
        MaterialName = materialName.Trim();
        MaterialUnit = materialUnit.Trim();
        Quantity = quantity;
        StockBefore = stockBefore;
        StockAfter = stockBefore + quantity;
        Type = type;
        Reference = reference?.Trim();
        Notes = notes?.Trim() ?? string.Empty;
        TransactionDate = DateTime.UtcNow;
    }
}

public enum StockTransactionType
{
    Purchase = 0,
    OrderConsumption = 1,
    ManualAdjustment = 2,
    InitialStock = 3
}
