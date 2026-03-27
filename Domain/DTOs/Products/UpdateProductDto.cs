using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs.Products;

public class UpdateProductDto
{
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
}
