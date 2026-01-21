namespace FurnitureERP.Domain.Exceptions;

public class MaterialNotFoundException : DomainException
{
    public MaterialNotFoundException(int materialId)
        : base($"Materiál s identifikátorem {materialId} nebyl nalezen")
    {
    }

    public MaterialNotFoundException(string message) : base(message)
    {
    }
}
