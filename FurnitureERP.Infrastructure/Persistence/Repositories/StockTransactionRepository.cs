using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Repositories;

namespace FurnitureERP.Infrastructure.Persistence.Repositories;

public class StockTransactionRepository : IStockTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public StockTransactionRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(StockTransaction transaction, CancellationToken cancellationToken = default)
    {
        await _context.StockTransactions.AddAsync(transaction, cancellationToken);
    }

    public IQueryable<StockTransaction> GetAll()
        => _context.StockTransactions;

    public IQueryable<StockTransaction> GetByMaterial(int materialId)
        => _context.StockTransactions.Where(t => t.MaterialId == materialId);
}
