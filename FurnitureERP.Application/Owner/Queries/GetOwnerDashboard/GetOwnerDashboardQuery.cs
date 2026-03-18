using FurnitureERP.Application.Owner.DTOs;
using MediatR;

namespace FurnitureERP.Application.Owner.Queries.GetOwnerDashboard;

public record GetOwnerDashboardQuery(int Year, int Month) : IRequest<OwnerDashboardDto>;
