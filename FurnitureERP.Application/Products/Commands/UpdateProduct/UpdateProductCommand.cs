using MediatR;
using FurnitureERP.Application.Products.Commands.CreateProduct;

namespace FurnitureERP.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int Id,
    string Name,
    string Description,
    string Category,
    string Article,
    decimal BasePrice,
    decimal SalePrice,
    bool IsActive,
    List<CreateMaterialBomDto>? Materials = null,
    List<CreateLaborBomDto>? LaborOperations = null
) : IRequest;
