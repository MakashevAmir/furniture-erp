using FurnitureERP.Application.Materials.DTOs;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetLowStockMaterials;

public record GetLowStockMaterialsQuery : IRequest<IEnumerable<MaterialDto>>;
