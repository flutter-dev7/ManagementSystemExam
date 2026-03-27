using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTOs.Sales;

public class UpdateSaleDto
{
    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    public int QuantitySold { get; set; }
}
