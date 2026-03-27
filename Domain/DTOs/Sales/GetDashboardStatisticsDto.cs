using System;

namespace Domain.DTOs.Sales;

public class GetDashboardStatisticsDto
{
    public int TotalProducts { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalSales { get; set; }
}
