using System;

namespace Domain.DTOs.Sales;

public class GetTopProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalSold { get; set; }
}
