using System;

namespace Domain.DTOs.StockAdjustments;

public class GetStockAdjustmentDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int AdjustmentAmount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime AdjustmentDate { get; set; }
}
