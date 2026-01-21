namespace FurnitureERP.Domain.Exceptions;

public class InvalidProductDataException : DomainException
{
    public InvalidProductDataException(string message) : base(message)
    {
    }

    public InvalidProductDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
