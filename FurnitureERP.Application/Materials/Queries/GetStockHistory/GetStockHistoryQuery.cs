using FurnitureERP.Application.Materials.DTOs;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetStockHistory;

public record GetStockHistoryQuery(
    int? MaterialId = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null) : IRequest<IEnumerable<StockTransactionDto>>;
