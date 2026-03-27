using System;
using Domain.DTOs.Suppliers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _service;

    public SupplierController(ISupplierService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var suppliers = await _service.GetAllSuppliersAsync();

        return Ok(suppliers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var supplier = await _service.GetSupplierByIdAsync(id);

        if (supplier == null)
            return NotFound();

        return Ok(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSupplierDto request)
    {
        var res = await _service.CreateSupplierAsync(request);

        return Created("", res);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateSupplierDto request)
    {
        var res = await _service.UpdateSupplierAsync(id, request);

        if (!res)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var res = await _service.DeleteSupplierAsync(id);

        if (!res)
            return NotFound();

        return NoContent();
    }

    [HttpGet("with-products")]
    public async Task<IActionResult> GetWithProducts()
    {
        var suppliers = await _service.GetWithProducts();

        return Ok(suppliers);
    }
}
