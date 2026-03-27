using System;

namespace Domain.DTOs.Sales;

public class GetSaleInProductDto
{
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}
