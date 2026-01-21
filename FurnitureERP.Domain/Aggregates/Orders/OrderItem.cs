using FurnitureERP.Domain.Common;
using FurnitureERP.Domain.Exceptions;

namespace FurnitureERP.Domain.Aggregates.Orders;

public class OrderItem : Entity
{
    public int OrderId { get; private set; }

    public int ProductId { get; private set; }

    public string ProductName { get; private set; }

    public string ProductArticle { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Subtotal => Quantity * UnitPrice;

    public string Notes { get; private set; }

    private OrderItem()
    {
        ProductName = string.Empty;
        ProductArticle = string.Empty;
        Notes = string.Empty;
    }

    public OrderItem(
        int orderId,
        int productId,
        string productName,
        string productArticle,
        int quantity,
        decimal unitPrice,
        string notes = "")
    {
        if (orderId <= 0)
            throw new InvalidOrderDataException($"Neplatné ID objednávky: {orderId}");

        if (productId <= 0)
            throw new InvalidOrderDataException($"Neplatné ID výrobku: {productId}");

        if (string.IsNullOrWhiteSpace(productName))
            throw new InvalidOrderDataException("Název výrobku nesmí být prázdný");

        if (string.IsNullOrWhiteSpace(productArticle))
            throw new InvalidOrderDataException("Kód výrobku nesmí být prázdný");

        if (quantity <= 0)
            throw new InvalidOrderDataException($"Množství musí být větší než 0, získáno: {quantity}");

        if (unitPrice <= 0)
            throw new InvalidOrderDataException($"Cena musí být větší než 0, získáno: {unitPrice}");

        OrderId = orderId;
        ProductId = productId;
        ProductName = productName.Trim();
        ProductArticle = productArticle.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        Notes = notes?.Trim() ?? string.Empty;
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new InvalidOrderDataException($"Množství musí být větší než 0, získáno: {newQuantity}");

        Quantity = newQuantity;
        MarkAsUpdated();
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes?.Trim() ?? string.Empty;
        MarkAsUpdated();
    }
}
