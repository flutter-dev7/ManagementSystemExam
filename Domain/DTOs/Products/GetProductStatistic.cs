using System;

namespace Domain.DTOs.Products;

public class GetProductStatistic
{
    public int TotalProducts { get; set; }
    public decimal AveragePrice { get; set; }
    public int TotalSold { get; set; }
}
