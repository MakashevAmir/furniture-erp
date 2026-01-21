namespace FurnitureERP.Domain.Exceptions;

public class InvalidEmployeeDataException : DomainException
{
    public InvalidEmployeeDataException(string message) : base(message)
    {
    }

    public InvalidEmployeeDataException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
