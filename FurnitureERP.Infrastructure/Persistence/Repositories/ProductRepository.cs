using FurnitureERP.Domain.Aggregates.Products;
using FurnitureERP.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.MaterialBoms)
            .Include(p => p.LaborBoms)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetByArticleAsync(string article, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(article))
            return null;

        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Article == article.Trim(), cancellationToken);
    }

    public IQueryable<Product> GetActiveProducts()
    {
        return _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        await _context.Products.AddAsync(product, cancellationToken);
    }

    public void Update(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));

        _context.Products.Remove(product);
    }
}
