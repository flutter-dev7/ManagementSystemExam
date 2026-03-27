using System;

namespace Domain.DTOs.StockAdjustments;

public class GetStockAdjustmentHistoryDto
{
    public DateTime AdjustmentDate { get; set; }
    public int Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
}
