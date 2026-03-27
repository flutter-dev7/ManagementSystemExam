using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Sale
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    public int QuantitySold { get; set; }

    public DateTime SaleDate { get; set; }

    // navigation property
    public Product Product { get; set; } = null!;
}
