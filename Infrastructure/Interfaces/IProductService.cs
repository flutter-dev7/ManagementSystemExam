using System;
using Domain.DTOs.Products;

namespace Infrastructure.Interfaces;

public interface IProductService
{
    Task<IEnumerable<GetProductDto>> GetAllProductsAsync();
    Task<GetProductDto?> GetProductByIdAsync(int id);
    Task<bool> CreateProductAsync(CreateProductDto request);
    Task<bool> UpdateProductAsync(int id, UpdateProductDto request);
    Task<bool> DeleteProductAsync(int id);
}
