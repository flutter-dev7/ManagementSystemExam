using System;
using Domain.DTOs.Suppliers;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<GetSupplierDto>> GetAllSuppliersAsync();
    Task<GetSupplierDto?> GetSupplierByIdAsync(int id);
    Task<bool> CreateSupplierAsync(CreateSupplierDto request);
    Task<bool> UpdateSupplierAsync(int id, UpdateSupplierDto request);
    Task<bool> DeleteSupplierAsync(int id); 

    Task<List<GetWithProductDto>> GetWithProducts();
}
