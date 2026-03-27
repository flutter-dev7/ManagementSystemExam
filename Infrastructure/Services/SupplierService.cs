using System;
using Domain.DTOs.Suppliers;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SupplierService : ISupplierService
{
    private readonly AppDbContext _context;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(AppDbContext context, ILogger<SupplierService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<GetSupplierDto>> GetAllSuppliersAsync()
    {
        _logger.LogInformation("Start in GetAllSuppliers");

        var suppliers = await _context.Suppliers
        .Select(s => new GetSupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Phone = s.Phone
        }).ToListAsync();

        _logger.LogInformation("Finish in GetAllSuppliers");
        return suppliers;
    }

    public async Task<GetSupplierDto?> GetSupplierByIdAsync(int id)
    {
        _logger.LogInformation("Start in GetSupplierById with Id = {Id}", id);

        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            _logger.LogWarning("Supplier with Id = {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Finish in GetSupplierById with Id = {Id}", id);
        return new GetSupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Phone = supplier.Phone
        };
    }

    public async Task<bool> CreateSupplierAsync(CreateSupplierDto request)
    {
        _logger.LogInformation("Start in CreateSupplier");
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be empty");

            if (!request.Phone.StartsWith("+"))
                throw new ArgumentException("Phone must be start with +");

            var supplier = new Supplier
            {
                Name = request.Name,
                Phone = request.Phone
            };

            _context.Suppliers.Add(supplier);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in CreateSupplier");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateSupplier");
            throw;
        }
    }

    public async Task<bool> UpdateSupplierAsync(int id, UpdateSupplierDto request)
    {
        _logger.LogInformation("Start in UpdateSupplier with Id = {Id}", id);
        try
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"Supplier with Id = {id} not found for update"); ;
            }

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Name cannot be empty");

            if (!request.Phone.StartsWith("+"))
                throw new ArgumentException("Phone must be start with +");

            supplier.Name = request.Name;
            supplier.Phone = request.Phone;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in UpdateSupplier with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateSupplier with Id = {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        _logger.LogInformation("Start in DeleteSupplier");

        try
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier with Id = {Id} not found for update", id);
                throw new KeyNotFoundException($"Supplier with Id = {id} not found for update");
            }

            _context.Remove(supplier);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Finish in DeleteSupplier with Id = {Id}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteSupplier with Id = {Id}", id);
            throw;
        }
    }

}
