using System;
using Domain.DTOs.Products;
using Domain.DTOs.StockAdjustments;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class StockAdjustmentService : IStockAdjustmentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StockAdjustmentService> _logger;

    public StockAdjustmentService(AppDbContext context, ILogger<StockAdjustmentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetStockAdjustmentDto>> GetAllStockAdjustmentsAsync()
    {
        _logger.LogInformation("Start in GetAllAsync");

        var adjustments = await _context.StockAdjustments
            .Select(st => new GetStockAdjustmentDto
            {
                Id = st.Id,
                ProductId = st.ProductId,
                ProductName = st.Product.Name,
                AdjustmentAmount = st.AdjustmentAmount,
                Reason = st.Reason,
                AdjustmentDate = st.AdjustmentDate
            }).ToListAsync();

        _logger.LogInformation("Finish in GetAllAsync");
        return adjustments;
    }

    public async Task<GetStockAdjustmentDto?> GetStockAdjustmentByIdAsync(int id)
    {
        _logger.LogInformation("Start in GetByIdAsync with Id = {Id}", id);

        var adjustment = await _context.StockAdjustments
            .Include(a => a.Product)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (adjustment == null)
        {
            _logger.LogWarning("Stock adjustment with Id = {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Finish in GetByIdAsync");
        return new GetStockAdjustmentDto
        {
            Id = adjustment.Id,
            ProductId = adjustment.ProductId,
            ProductName = adjustment.Product.Name,
            AdjustmentAmount = adjustment.AdjustmentAmount,
            Reason = adjustment.Reason,
            AdjustmentDate = adjustment.AdjustmentDate
        };
    }

    public async Task<bool> CreateStockAdjustmentAsync(CreateStockAdjustmentDto request)
    {
        _logger.LogInformation("Start in CreateStockAdjustmnet");
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (product.QuantityInStock + request.AdjustmentAmount < 0)
                throw new ArgumentException("Not enough stock for adjustment");

            product.QuantityInStock += request.AdjustmentAmount;

            var adjustment = new StockAdjustment
            {
                ProductId = request.ProductId,
                AdjustmentAmount = request.AdjustmentAmount,
                Reason = request.Reason,
                AdjustmentDate = DateTime.UtcNow
            };

            _context.Add(adjustment);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in CreateStockAdjustmnet");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateStockAdjustmnet");
            throw;
        }
    }

    public async Task<bool> UpdateStockAdjustmentAsync(int id, UpdateStockAdjustmentDto request)
    {
        _logger.LogInformation("Start in UpdateStockAdjustment with Id = {Id}", id);
        try
        {
            var adjustment = await _context.StockAdjustments
            .Include(st => st.Product)
            .FirstOrDefaultAsync(st => st.Id == id);

            if (adjustment == null)
            {
                _logger.LogWarning("StockAdjustment with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"StockAdjustment with Id = {id} not found for update");
            }

            adjustment.Product.QuantityInStock -= adjustment.AdjustmentAmount;

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

            if (product == null)
                throw new KeyNotFoundException("Product not found");

            if (product.QuantityInStock + request.AdjustmentAmount < 0)
                throw new ArgumentException("Not enough stock for adjustment");

            product.QuantityInStock += request.AdjustmentAmount;

            adjustment.ProductId = request.ProductId;
            adjustment.AdjustmentAmount = request.AdjustmentAmount;
            adjustment.Reason = request.Reason;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in UpdateStockAdjustment with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateSale with Id = {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteStockAdjustmentAsync(int id)
    {
        _logger.LogInformation("Start in DeleteStockAdjustment with Id = {Id}", id);
        try
        {
            var adjustment = await _context.StockAdjustments.FindAsync(id);

            if (adjustment == null)
            {
                _logger.LogWarning("StockAdjustment with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"StockAdjustment with Id = {id} not found for update");
            }

            _context.Remove(adjustment);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in DeleteStockAdjuetment with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteStockAdjuetment with Id = {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetStockAdjustmentHistoryDto>> GetStockAdjustmentsHistory(int productId)
    {
        _logger.LogInformation("Start in GetStockAdjustmentsHistory");

        var adjustments = await _context.StockAdjustments
        .Where(st => st.ProductId == productId)
        .Select(s => new GetStockAdjustmentHistoryDto
        {
            AdjustmentDate = s.AdjustmentDate,
            Amount = s.AdjustmentAmount,
            Reason = s.Reason
        }).ToListAsync();

        return adjustments;
    }
}
