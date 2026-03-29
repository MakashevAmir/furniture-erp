using MediatR;

namespace FurnitureERP.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    string Category,
    string Article,
    decimal BasePrice,
    decimal SalePrice,
    int ProductionDays = 1,
    List<CreateMaterialBomDto>? Materials = null,
    List<CreateLaborBomDto>? LaborOperations = null
) : IRequest<int>;

public record CreateMaterialBomDto(
    int MaterialId,
    decimal Quantity,
    decimal WastagePercentage,
    string Notes
);

public record CreateLaborBomDto(
    string Position,
    decimal Hours,
    string Description
);
