using MediatR;

namespace FurnitureERP.Application.Materials.Commands.CreateMaterial;

public record CreateMaterialCommand(
    string Name,
    string Description,
    string Category,
    string Unit,
    decimal PricePerUnit,
    decimal CurrentStock,
    decimal MinimumStock,
    string Supplier
) : IRequest<int>;
