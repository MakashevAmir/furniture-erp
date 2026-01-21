namespace FurnitureERP.Domain.Exceptions;

public class OrderNotFoundException : DomainException
{
    public OrderNotFoundException(int orderId)
        : base($"Objednávka s ID {orderId} nebyla nalezena")
    {
    }

    public OrderNotFoundException(string orderNumber)
        : base($"Objednávka s číslem '{orderNumber}' nebyla nalezena")
    {
    }
}
