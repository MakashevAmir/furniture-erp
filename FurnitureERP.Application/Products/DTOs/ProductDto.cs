namespace FurnitureERP.Application.Products.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string Description,
    string Category,
    string Article,
    decimal BasePrice,
    decimal SalePrice,
    bool IsActive,
    List<MaterialBomDto>? MaterialBoms = null,
    List<LaborBomDto>? LaborBoms = null
);

public class MaterialBomDto
{
    public int Id { get; set; }
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal WastagePercentage { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class LaborBomDto
{
    public int Id { get; set; }
    public string Position { get; set; } = string.Empty;
    public decimal Hours { get; set; }
    public string Description { get; set; } = string.Empty;
}
