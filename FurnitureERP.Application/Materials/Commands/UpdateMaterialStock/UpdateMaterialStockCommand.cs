using MediatR;

namespace FurnitureERP.Application.Materials.Commands.UpdateMaterialStock;

public record UpdateMaterialStockCommand(
    int Id,
    decimal Quantity
) : IRequest;
