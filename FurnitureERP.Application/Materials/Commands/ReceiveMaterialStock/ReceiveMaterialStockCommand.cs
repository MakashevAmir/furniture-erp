using MediatR;

namespace FurnitureERP.Application.Materials.Commands.ReceiveMaterialStock;

public record ReceiveMaterialStockCommand(
    int MaterialId,
    decimal Quantity,
    string? Notes = null) : IRequest;
