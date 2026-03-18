using FurnitureERP.Domain.Aggregates.Materials;

namespace FurnitureERP.Application.Materials.DTOs;

public record StockTransactionDto(
    int Id,
    int MaterialId,
    string MaterialName,
    string MaterialUnit,
    decimal Quantity,
    decimal StockBefore,
    decimal StockAfter,
    StockTransactionType Type,
    string TypeLabel,
    string? Reference,
    string Notes,
    DateTime TransactionDate);
