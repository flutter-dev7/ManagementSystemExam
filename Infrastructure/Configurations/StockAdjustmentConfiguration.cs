using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
{
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.HasKey(st => st.Id);

        builder.Property(st => st.AdjustmentAmount)
        .IsRequired();

        builder.Property(st => st.Reason)
        .HasMaxLength(200);

        builder.HasOne(st => st.Product)
        .WithMany(p => p.StockAdjustments)
        .HasForeignKey(st => st.ProductId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
