using FurnitureERP.Domain.Aggregates.Materials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureERP.Infrastructure.Persistence.Configurations;

public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        builder.ToTable("StockTransactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.MaterialId).IsRequired();

        builder.Property(t => t.MaterialName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.MaterialUnit)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.Quantity)
            .IsRequired()
            .HasPrecision(18, 4);

        builder.Property(t => t.StockBefore)
            .IsRequired()
            .HasPrecision(18, 4);

        builder.Property(t => t.StockAfter)
            .IsRequired()
            .HasPrecision(18, 4);

        builder.Property(t => t.Type).IsRequired();

        builder.Property(t => t.Reference).HasMaxLength(50);

        builder.Property(t => t.Notes).HasMaxLength(500);

        builder.Property(t => t.TransactionDate).IsRequired();

        builder.HasIndex(t => t.MaterialId);
        builder.HasIndex(t => t.TransactionDate);
    }
}
