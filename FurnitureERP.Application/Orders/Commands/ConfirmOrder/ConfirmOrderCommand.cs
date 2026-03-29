using MediatR;

namespace FurnitureERP.Application.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommand(int OrderId) : IRequest<ConfirmOrderResult>;

public record ConfirmOrderResult(
    bool MaterialsAvailable,
    DateTime CalculatedCompletionDate,
    string? MaterialShortageDetails = null);
