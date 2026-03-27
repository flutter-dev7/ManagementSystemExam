using System;
using Domain.DTOs.Products;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetProductDto>> GetAllProductsAsync()
    {
        _logger.LogInformation("Start in GetAllProducts");

        var products = await _context.Products
        .Select(p => new GetProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            QuantityInStock = p.QuantityInStock,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            SupplierId = p.SupplierId,
            SupplierName = p.Supplier.Name
        }).ToListAsync();

        _logger.LogInformation("Finish in GetAllProducts");
        return products;
    }

    public async Task<GetProductDto?> GetProductByIdAsync(int id)
    {
        _logger.LogInformation("Start in GetProductById with Id = {Id}", id);

        var product = await _context.Products
        .Include(p => p.Category)
        .Include(p => p.Supplier)
        .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with Id = {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Finish in GetProductById with Id = {Id}", id);
        return new GetProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            SupplierId = product.SupplierId,
            SupplierName = product.Supplier.Name
        };
    }

    public async Task<bool> CreateProductAsync(CreateProductDto request)
    {
        _logger.LogInformation("Start in CreateProduct");
        try
        {
            var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId);

            if (!categoryExists)
                throw new KeyNotFoundException("Category not found");

            var supplierExists = await _context.Suppliers
            .AnyAsync(s => s.Id == request.SupplierId);

            if (!supplierExists)
                throw new KeyNotFoundException("Supplier not found");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be empty");

            if (request.Price <= 0)
                throw new ArgumentException("Price must be greater than 0");

            if (request.QuantityInStock < 0)
                throw new ArgumentException("Quantity in stock cannot be negative");

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                QuantityInStock = request.QuantityInStock,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in CreateProduct");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create Product");
            throw;
        }
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductDto request)
    {
        _logger.LogInformation("Start in UpdateProduct");
        try
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"Product with Id = {id} not found for update");
            }

            var categoryExists = await _context.Categories
           .AnyAsync(c => c.Id == request.CategoryId);

            if (!categoryExists)
                throw new KeyNotFoundException("Category not found");

            var supplierExists = await _context.Suppliers
            .AnyAsync(s => s.Id == request.SupplierId);

            if (!supplierExists)
                throw new KeyNotFoundException("Supplier not found");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be empty");

            if (request.Price <= 0)
                throw new ArgumentException("Price must be greater than 0");

            if (request.QuantityInStock < 0)
                throw new ArgumentException("Quantity in stock cannot be negative");

            product.Name = request.Name;
            product.Price = request.Price;
            product.QuantityInStock = request.QuantityInStock;
            request.CategoryId = request.CategoryId;
            request.SupplierId = request.SupplierId;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in UpdateCategory with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Update Product with Id = {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        _logger.LogInformation("Start in DeleteProduct");
        try
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with Id = {Id} not found for delete", id);
                throw new KeyNotFoundException($"Product with Id = {id} not found for delete");
            }

            _context.Remove(product);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in DeleteProduct with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Delete Product with Id = {Id}", id);
            throw;
        }
    }

    public async Task<List<GetProductLowStock>> GetProductsLowStock()
    {
        _logger.LogInformation("Start in GetProductsLowStock");

        var products = await _context.Products
        .Where(p => p.QuantityInStock < 5)
        .Select(p => new GetProductLowStock
        {
            Id = p.Id,
            Name = p.Name,
            QuantityInStock = p.QuantityInStock
        }).ToListAsync();

        _logger.LogInformation("Finish in GetProductsLowStock");
        return products;
    }

    public async Task<GetProductStatistic> GetProductsStatistics()
    {
        _logger.LogInformation("Start in GetProductsStatistics");

        var totalProducts = await _context.Products.CountAsync();

        var averagePrice = await _context.Products.AverageAsync(p => p.Price);

        var totalSold = await _context.Sales.SumAsync(s => s.QuantitySold);

        var product = new GetProductStatistic
        {
            TotalProducts = totalProducts,
            AveragePrice = averagePrice,
            TotalSold = totalProducts
        };

        _logger.LogInformation("Finish in GetProductsStatistics");
        return product;
    }
}
