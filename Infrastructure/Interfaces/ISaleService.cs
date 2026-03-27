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
}
