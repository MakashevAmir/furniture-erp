using FurnitureERP.Domain.Aggregates.Materials;

namespace FurnitureERP.Domain.Repositories;

public interface IStockTransactionRepository
{
    Task AddAsync(StockTransaction transaction, CancellationToken cancellationToken = default);

    IQueryable<StockTransaction> GetAll();

    IQueryable<StockTransaction> GetByMaterial(int materialId);
}
