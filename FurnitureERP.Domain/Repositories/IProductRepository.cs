using FurnitureERP.Domain.Aggregates.Products;

namespace FurnitureERP.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Product?> GetByArticleAsync(string article, CancellationToken cancellationToken = default);

    IQueryable<Product> GetActiveProducts();

    Task AddAsync(Product product, CancellationToken cancellationToken = default);

    void Update(Product product);

    void Delete(Product product);
}
