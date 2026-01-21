using FurnitureERP.Application.Materials.DTOs;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetMaterialById;

public record GetMaterialByIdQuery(int Id) : IRequest<MaterialDto?>;
