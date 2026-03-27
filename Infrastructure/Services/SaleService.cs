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

    public async Task<IEnumerable<GetSaleByDateDto>> GetSalesByDateAsync(DateTime fromDate, DateTime toDate)
    {
        _logger.LogInformation("Start in GetSalesByDate from {fromDate} to {toDate}", fromDate, toDate);

        fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
        toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

        var sales = await _context.Sales
        .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate)
        .Select(s => new GetSaleByDateDto
        {
            SaleId = s.Id,
            ProductName = s.Product.Name,
            QuantitySold = s.QuantitySold,
            SaleDate = s.SaleDate
        }).ToListAsync();

        _logger.LogInformation("Finish in GetSalesByDate from {fromDate} to {toDate}", fromDate, toDate);
        return sales;
    }

    public async Task<IEnumerable<GetTopProductDto>> GetTopProductsAsync()
    {
        _logger.LogInformation("Start in GetTopProducts");

        var sales = await _context.Sales
        .GroupBy(s => s.Product.Name)
        .Select(s => new GetTopProductDto
        {
            ProductName = s.Key,
            TotalSold = s.Sum(t => t.QuantitySold)
        }).
        OrderByDescending(t => t.TotalSold)
        .ToListAsync();

        return sales;
    }

    public async Task<IEnumerable<GetDailyRevenueDto>> GetDailyRevenueAsync()
    {
        _logger.LogInformation("Start in GetDailyRevenueAsync");

        var fromDate = DateTime.UtcNow.AddDays(-7);

        var reports = await _context.Sales
        .GroupBy(s => s.SaleDate.Date)
        .Select(g => new GetDailyRevenueDto
        {
            Date = g.Key,
            Revenue = g.Sum(x => x.QuantitySold * x.Product.Price)
        }).OrderBy(x => x.Date).ToListAsync();

        return reports;
    }

    public async Task<GetDashboardStatisticsDto> GetDashboardStatisticsAsync()
    {
        _logger.LogInformation("Start in GetDashboardStatisticsAsync");

        var totalProducts = await _context.Products.CountAsync();

        var totalSales = await _context.Sales.CountAsync();

        var result = new GetDashboardStatisticsDto
        {
            TotalProducts = totalProducts,
            TotalSales = totalSales,
            TotalRevenue = await _context.Sales
            .SumAsync(s => s.QuantitySold * s.Product.Price)
        };

        _logger.LogInformation("Finish in GetDashboardStatisticsAsync");
        return result;
    }
}
