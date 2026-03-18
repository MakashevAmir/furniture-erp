using FurnitureERP.Application.Materials.DTOs;
using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Repositories;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetStockHistory;

public class GetStockHistoryQueryHandler : IRequestHandler<GetStockHistoryQuery, IEnumerable<StockTransactionDto>>
{
    private readonly IStockTransactionRepository _repository;

    public GetStockHistoryQueryHandler(IStockTransactionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<IEnumerable<StockTransactionDto>> Handle(GetStockHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetAll();

        if (request.MaterialId.HasValue)
            query = query.Where(t => t.MaterialId == request.MaterialId.Value);

        if (request.DateFrom.HasValue)
            query = query.Where(t => t.TransactionDate >= request.DateFrom.Value);

        if (request.DateTo.HasValue)
            query = query.Where(t => t.TransactionDate < request.DateTo.Value.AddDays(1));

        var result = query
            .OrderByDescending(t => t.TransactionDate)
            .Select(t => new StockTransactionDto(
                t.Id,
                t.MaterialId,
                t.MaterialName,
                t.MaterialUnit,
                t.Quantity,
                t.StockBefore,
                t.StockAfter,
                t.Type,
                GetTypeLabel(t.Type),
                t.Reference,
                t.Notes,
                t.TransactionDate))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }

    private static string GetTypeLabel(StockTransactionType type) => type switch
    {
        StockTransactionType.Purchase => "Příjem",
        StockTransactionType.OrderConsumption => "Spotřeba zakázky",
        StockTransactionType.ManualAdjustment => "Ruční úprava",
        StockTransactionType.InitialStock => "Počáteční stav",
        _ => type.ToString()
    };
}
