using FurnitureERP.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureERP.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Article)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.Category)
            .HasMaxLength(100);

        builder.Property(p => p.BasePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.SalePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.Article)
            .IsUnique()
            .HasDatabaseName("IX_Products_Article");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");

        builder.HasIndex(p => p.Category)
            .HasDatabaseName("IX_Products_Category");

        builder.HasMany(p => p.MaterialBoms)
            .WithOne()
            .HasForeignKey(mb => mb.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.MaterialBoms)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(p => p.LaborBoms)
            .WithOne()
            .HasForeignKey(lb => lb.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.LaborBoms)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(p => p.DomainEvents);
    }
}
