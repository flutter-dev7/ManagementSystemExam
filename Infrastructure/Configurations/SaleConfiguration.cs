using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.QuantitySold)
        .IsRequired();

        builder.HasOne(s => s.Product)
        .WithMany(p => p.Sales)
        .HasForeignKey(s => s.ProductId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
