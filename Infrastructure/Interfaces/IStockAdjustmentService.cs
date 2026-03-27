using System;
using Domain.DTOs.Products;
using Domain.DTOs.StockAdjustments;

namespace Infrastructure.Interfaces;

public interface IStockAdjustmentService
{
    Task<IEnumerable<GetStockAdjustmentDto>> GetAllStockAdjustmentsAsync();
    Task<GetStockAdjustmentDto?> GetStockAdjustmentByIdAsync(int id);
    Task<bool> CreateStockAdjustmentAsync(CreateStockAdjustmentDto request);
    Task<bool> UpdateStockAdjustmentAsync(int id, UpdateStockAdjustmentDto request);
    Task<bool> DeleteStockAdjustmentAsync(int id);

    Task<IEnumerable<GetStockAdjustmentHistoryDto>> GetStockAdjustmentsHistory(int productId);
}
