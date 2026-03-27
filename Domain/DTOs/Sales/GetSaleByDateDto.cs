using System;

namespace Domain.DTOs.Sales;

public class GetSaleByDateDto
{
    public int SaleId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public DateTime SaleDate { get; set; }
}
