using MediatR;

namespace FurnitureERP.Application.Materials.Commands.UpdateMaterial;

public record UpdateMaterialCommand(
    int Id,
    string Name,
    string Description,
    string Category,
    string Unit,
    decimal PricePerUnit,
    string Supplier,
    decimal CurrentStock,
    decimal MinimumStock,
    bool IsActive
) : IRequest;
