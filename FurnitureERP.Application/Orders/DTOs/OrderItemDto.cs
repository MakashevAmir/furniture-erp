namespace FurnitureERP.Application.Orders.DTOs;

public record OrderItemDto(
    int ProductId,
    string ProductName,
    string ProductArticle,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal,
    string Notes
);
