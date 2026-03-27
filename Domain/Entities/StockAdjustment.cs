using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class StockAdjustment
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    public int AdjustmentAmount { get; set; }

    [MaxLength(150)]
    public string Reason { get; set; } = string.Empty;

    public DateTime AdjustmentDate { get; set; }

    // navigation property
    public Product Product { get; set; } = null!;
}
