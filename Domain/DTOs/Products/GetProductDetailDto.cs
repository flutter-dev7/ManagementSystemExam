using System;
using Domain.DTOs.Sales;
using Domain.DTOs.StockAdjustments;

namespace Domain.DTOs.Products;

public class GetProductDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public List<GetSaleInProductDto> Sales { get; set; } = new();
    public List<GetStockAdjustmentHistoryDto> Adjustments { get; set; } = new();
}
