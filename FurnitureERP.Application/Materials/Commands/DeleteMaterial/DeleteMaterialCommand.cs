using MediatR;

namespace FurnitureERP.Application.Materials.Commands.DeleteMaterial;

public record DeleteMaterialCommand(int Id) : IRequest;
