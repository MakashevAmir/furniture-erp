using FurnitureERP.Domain.Aggregates.Materials;
using FurnitureERP.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FurnitureERP.Infrastructure.Persistence.Repositories;

public class MaterialRepository : IMaterialRepository
{
    private readonly ApplicationDbContext _context;

    public MaterialRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Material?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Material?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Name == name.Trim(), cancellationToken);
    }

    public IQueryable<Material> GetActiveMaterials()
    {
        return _context.Materials
            .AsNoTracking()
            .Where(m => m.IsActive);
    }

    public IQueryable<Material> GetMaterialsByCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return Enumerable.Empty<Material>().AsQueryable();

        return _context.Materials
            .AsNoTracking()
            .Where(m => m.Category == category.Trim() && m.IsActive);
    }

    public IQueryable<Material> GetLowStockMaterials()
    {
        return _context.Materials
            .AsNoTracking()
            .Where(m => m.IsActive && m.CurrentStock < m.MinimumStock);
    }

    public async Task AddAsync(Material material, CancellationToken cancellationToken = default)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material));

        await _context.Materials.AddAsync(material, cancellationToken);
    }

    public void Update(Material material)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material));

        _context.Materials.Update(material);
    }

    public void Delete(Material material)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material));

        _context.Materials.Remove(material);
    }
}
