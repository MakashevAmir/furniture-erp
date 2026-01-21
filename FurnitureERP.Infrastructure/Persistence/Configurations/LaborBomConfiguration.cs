using FurnitureERP.Domain.Aggregates.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FurnitureERP.Infrastructure.Persistence.Configurations;

public class LaborBomConfiguration : IEntityTypeConfiguration<LaborBom>
{
    public void Configure(EntityTypeBuilder<LaborBom> builder)
    {
        builder.ToTable("LaborBoms");

        builder.HasKey(lb => lb.Id);

        builder.Property(lb => lb.ProductId)
            .IsRequired();

        builder.Property(lb => lb.EmployeeId)
            .IsRequired(false); 

        builder.Property(lb => lb.Position)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(lb => lb.HoursRequired)
            .HasPrecision(18, 4)
            .IsRequired();

        builder.Property(lb => lb.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(lb => lb.SequenceNumber)
            .IsRequired(false); 

        builder.Property(lb => lb.CreatedAt)
            .IsRequired();

        builder.Property(lb => lb.UpdatedAt)
            .IsRequired();

        builder.HasIndex(lb => lb.ProductId)
            .HasDatabaseName("IX_LaborBoms_ProductId");

        builder.HasIndex(lb => lb.EmployeeId)
            .HasDatabaseName("IX_LaborBoms_EmployeeId");

        builder.HasIndex(lb => lb.Position)
            .HasDatabaseName("IX_LaborBoms_Position");

        builder.HasIndex(lb => new { lb.ProductId, lb.SequenceNumber })
            .HasDatabaseName("IX_LaborBoms_ProductId_SequenceNumber");
    }
}
