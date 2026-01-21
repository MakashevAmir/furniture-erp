namespace FurnitureERP.Domain.Exceptions;

public class InvalidOrderDataException : DomainException
{
    public InvalidOrderDataException(string message) : base(message)
    {
    }

    public InvalidOrderDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
