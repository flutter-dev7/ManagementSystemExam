using System;
using Domain.DTOs.Sales;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/sales")]
public class SaleController : ControllerBase
{
    private readonly ISaleService _service;

    public SaleController(ISaleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var sales = await _service.GetAllSalesAsync();

        return Ok(sales);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var sale = await _service.GetSaleByIdAsync(id);

        if (sale == null)
            return NotFound();

        return Ok(sale);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateSaleDto request)
    {
        var res = await _service.CreateSaleAsync(request);

        return Created("", res);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateSaleDto request)
    {
        var res = await _service.UpdateSaleAsync(id, request);

        if (!res)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var res = await _service.DeleteSaleAsync(id);

        if (!res)
            return NotFound();

        return NoContent();
    }
}
