using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    [ForeignKey("Supplier")]
    public int SupplierId { get; set; }

    // navigation property
    public Category Category { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
}
