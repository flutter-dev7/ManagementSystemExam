using System;

namespace Domain.DTOs.Sales;

public class GetSaleDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public DateTime SaleDate { get; set; }
}
