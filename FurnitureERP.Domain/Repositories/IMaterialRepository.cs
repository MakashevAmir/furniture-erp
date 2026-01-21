using FurnitureERP.Domain.Aggregates.Materials;

namespace FurnitureERP.Domain.Repositories;

public interface IMaterialRepository
{
    Task<Material?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Material?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    IQueryable<Material> GetActiveMaterials();

    IQueryable<Material> GetLowStockMaterials();

    IQueryable<Material> GetMaterialsByCategory(string category);

    Task AddAsync(Material material, CancellationToken cancellationToken = default);

    void Update(Material material);

    void Delete(Material material);
}
