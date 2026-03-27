using System;
using Domain.DTOs.Categories;
using Domain.DTOs.Products;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetCategoryDto>> GetAllCategoriesAsync()
    {
        _logger.LogInformation("Start in GetAllCategories");

        var categories = await _context.Categories
        .Select(c => new GetCategoryDto
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();

        _logger.LogInformation("Finish in GetAllCategories");
        return categories;
    }

    public async Task<GetCategoryDto?> GetCategoryByIdAsync(int id)
    {
        _logger.LogInformation("Start in GetCategoryById with Id = {Id}", id);

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            _logger.LogWarning("Category with Id = {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Finish in GetCategoryById with Id = {Id}", id);
        return new GetCategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public async Task<bool> CreateCategoryAsync(CreateCategoryDto request)
    {
        _logger.LogInformation("Start in CreateCategory");
        try
        {
            if (string.IsNullOrEmpty(request.Name))
                throw new ArgumentException("Name cannot be Empty");

            var category = new Category
            {
                Name = request.Name
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in CreateCategory");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCategory");
            throw;
        }
    }

    public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto request)
    {
        _logger.LogInformation("Start in UpdateCategory with Id = {Id}", id);
        try
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"Category with Id = {id} not found for update");
            }

            if (string.IsNullOrEmpty(request.Name))
                throw new ArgumentException("Name cannot be Empty");

            category.Name = request.Name;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in UpdateCategory with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateCategory with Id = {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        _logger.LogInformation("Start in DeleteCategory with Id = {Id}", id);

        try
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category with Id = {Id} not found for delete", id);
                throw new KeyNotFoundException($"Category with Id = {id} not found for delete");
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in DeleteCategory with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteCategory with Id = {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetCategoryWithProductsDto>> GetCategoriesWithProducts()
    {
        _logger.LogInformation("Start in GetCategoriesWithProducts");

        var categories = await _context.Categories
        .Select(c => new GetCategoryWithProductsDto
        {
            CategoryId = c.Id,
            CategoryName = c.Name,
            Products =  c.Products
            .Select(p => new GetProductInCategoryDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList()
        }).ToListAsync();

        return categories;
    }
}
