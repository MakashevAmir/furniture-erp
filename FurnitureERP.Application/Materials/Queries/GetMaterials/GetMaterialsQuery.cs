using FurnitureERP.Application.Materials.DTOs;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetMaterials;

public record GetMaterialsQuery : IRequest<IEnumerable<MaterialDto>>;
