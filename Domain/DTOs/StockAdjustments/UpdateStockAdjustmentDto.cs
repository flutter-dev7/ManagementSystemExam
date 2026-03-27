using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs.StockAdjustments;

public class UpdateStockAdjustmentDto
{
    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    public int AdjustmentAmount { get; set; }

    [MaxLength(150)]
    public string Reason { get; set; } = string.Empty;
}
