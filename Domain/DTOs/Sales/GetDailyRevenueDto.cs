using System;

namespace Domain.DTOs.Sales;

public class GetDailyRevenueDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
}
