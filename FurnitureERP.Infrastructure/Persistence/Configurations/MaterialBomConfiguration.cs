using FurnitureERP.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureERP.Infrastructure.Persistence.Configurations;

public class MaterialBomConfiguration : IEntityTypeConfiguration<MaterialBom>
{
    public void Configure(EntityTypeBuilder<MaterialBom> builder)
    {
        builder.ToTable("MaterialBoms");

        builder.HasKey(mb => mb.Id);

        builder.Property(mb => mb.ProductId)
            .IsRequired();

        builder.Property(mb => mb.MaterialId)
            .IsRequired();

        builder.Property(mb => mb.QuantityRequired)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(mb => mb.WastagePercentage)
            .HasPrecision(5, 2)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(mb => mb.Notes)
            .HasMaxLength(500);

        builder.Property(mb => mb.CreatedAt)
            .IsRequired();

        builder.Property(mb => mb.UpdatedAt)
            .IsRequired();

        builder.Ignore(mb => mb.QuantityWithWastage);

        builder.HasIndex(mb => mb.ProductId)
            .HasDatabaseName("IX_MaterialBoms_ProductId");

        builder.HasIndex(mb => mb.MaterialId)
            .HasDatabaseName("IX_MaterialBoms_MaterialId");

        builder.HasIndex(mb => new { mb.ProductId, mb.MaterialId })
            .HasDatabaseName("IX_MaterialBoms_ProductId_MaterialId");
    }
}
