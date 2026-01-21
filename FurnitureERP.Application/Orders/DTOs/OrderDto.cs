using FurnitureERP.Domain.Aggregates.Orders;

namespace FurnitureERP.Application.Orders.DTOs;

public record OrderDto(
    int Id,
    string OrderNumber,
    string CustomerName,
    string CustomerPhone,
    string? CustomerEmail,
    string DeliveryAddress,
    DateTime OrderDate,
    DateTime? ExpectedCompletionDate,
    DateTime? ActualCompletionDate,
    OrderStatus Status,
    decimal TotalAmount,
    string Notes,
    List<OrderItemDto> OrderItems
);
