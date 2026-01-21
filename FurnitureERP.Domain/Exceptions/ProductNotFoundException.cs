namespace FurnitureERP.Domain.Exceptions;

public class ProductNotFoundException : DomainException
{
    public ProductNotFoundException(int productId)
        : base($"Výrobek s identifikátorem {productId} nebyl nalezen")
    {
        ProductId = productId;
    }

    public ProductNotFoundException(string article)
        : base($"Výrobek s kódem '{article}' nebyl nalezen")
    {
        Article = article;
    }

    public int? ProductId { get; }

    public string? Article { get; }
}
