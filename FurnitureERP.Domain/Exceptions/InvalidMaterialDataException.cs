namespace FurnitureERP.Domain.Exceptions;

public class InvalidMaterialDataException : DomainException
{
    public InvalidMaterialDataException(string message) : base(message)
    {
    }

    public InvalidMaterialDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
