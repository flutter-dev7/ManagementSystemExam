using System;
using Domain.DTOs.Sales;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SaleService : ISaleService
{
    private readonly AppDbContext _context;
    private readonly ILogger<SaleService> _logger;

    public SaleService(AppDbContext context, ILogger<SaleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetSaleDto>> GetAllSalesAsync()
    {
        _logger.LogInformation("Start in GetAllSales");

        var sales = await _context.Sales
        .Select(s => new GetSaleDto
        {
            Id = s.Id,
            ProductId = s.ProductId,
            ProductName = s.Product.Name,
            QuantitySold = s.QuantitySold,
            SaleDate = s.SaleDate
        }).ToListAsync();

        _logger.LogInformation("Finish in GetAllSales");
        return sales;
    }

    public async Task<GetSaleDto?> GetSaleByIdAsync(int id)
    {
        _logger.LogInformation("Start in GetSaleById with Id = {Id}", id);

        var sale = await _context.Sales
        .Include(s => s.Product)
        .FirstOrDefaultAsync(s => s.Id == id);

        if (sale == null)
        {
            _logger.LogWarning("Sale with Id = {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Finish in GetSaleById");
        return new GetSaleDto
        {
            Id = sale.Id,
            ProductId = sale.ProductId,
            ProductName = sale.Product.Name,
            QuantitySold = sale.QuantitySold,
            SaleDate = sale.SaleDate
        };
    }

    public async Task<bool> CreateSaleAsync(CreateSaleDto request)
    {
        _logger.LogInformation("Start in CreateSale");
        try
        {
            var product = await _context.Products
             .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (request.QuantitySold <= 0)
                throw new ArgumentException("QuantitySold must be greater than 0");

            product.QuantityInStock -= request.QuantitySold;

            var sale = new Sale
            {
                ProductId = request.ProductId,
                QuantitySold = request.QuantitySold,
                SaleDate = DateTime.UtcNow
            };

            _context.Sales.Add(sale);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in CreateSale");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateSale");
            throw;
        }
    }

    public async Task<bool> UpdateSaleAsync(int id, UpdateSaleDto request)
    {
        _logger.LogInformation("Start in UpdateSale with Id = {Id}", id);
        try
        {
            var sale = await _context.Sales
            .Include(s => s.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                _logger.LogWarning("Sale with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"Sale with Id = {id} not found for update");
            }

            var product = await _context.Products
             .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (request.QuantitySold <= 0)
                throw new ArgumentException("QuantitySold must be greater than 0");

            sale.Product.QuantityInStock += sale.QuantitySold;

            if (product.QuantityInStock < request.QuantitySold)
                throw new ArgumentException("Not enough stock.");

            product.QuantityInStock -= request.QuantitySold;

            sale.ProductId = request.ProductId;
            sale.QuantitySold = request.QuantitySold;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in UpdateSale with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateSale with Id = {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteSaleAsync(int id)
    {
        _logger.LogInformation("Start in DeleteSale with Id = {Id}", id);
        try
        {
            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
            {
                _logger.LogWarning("Sale with Id = {Id} not found for delete", id);
                throw new KeyNotFoundException($"Sale with Id = {id} not found for delete");
            }

            _context.Sales.Remove(sale);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in DeleteSale with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteSale with Id = {Id}", id);
            throw;
        }
    }
}
