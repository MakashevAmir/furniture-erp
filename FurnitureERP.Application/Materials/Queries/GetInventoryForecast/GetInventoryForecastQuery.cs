using FurnitureERP.Application.Materials.DTOs;
using MediatR;

namespace FurnitureERP.Application.Materials.Queries.GetInventoryForecast;

public record GetInventoryForecastQuery() : IRequest<IEnumerable<InventoryForecastDto>>;
