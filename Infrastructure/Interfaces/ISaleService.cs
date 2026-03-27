using System;
using Domain.DTOs.Sales;

namespace Infrastructure.Interfaces;

public interface ISaleService
{
    Task<IEnumerable<GetSaleDto>> GetAllSalesAsync();
    Task<GetSaleDto?> GetSaleByIdAsync(int id);
    Task<bool> CreateSaleAsync(CreateSaleDto request);
    Task<bool> UpdateSaleAsync(int id, UpdateSaleDto request);
    Task<bool> DeleteSaleAsync(int id);

    Task<IEnumerable<GetSaleByDateDto>> GetSalesByDateAsync(DateTime fromDate, DateTime toDate);
    Task<IEnumerable<GetTopProductDto>> GetTopProductsAsync();
    Task<IEnumerable<GetDailyRevenueDto>> GetDailyRevenueAsync();
    Task<GetDashboardStatisticsDto> GetDashboardStatisticsAsync();
}
