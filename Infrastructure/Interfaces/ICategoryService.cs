using System;
using Domain.DTOs.Categories;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync();
    Task<GetCategoryDto?> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(CreateCategoryDto request);
    Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto request);
    Task<bool> DeleteCategoryAsync(int id);

    Task<IEnumerable<GetCategoryWithProductsDto>> GetCategoriesWithProducts();
}
