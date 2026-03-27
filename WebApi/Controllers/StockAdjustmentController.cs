using System;
using Domain.DTOs.StockAdjustments;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/adjustments")]
public class StockAdjustmentController : ControllerBase
{
    private readonly IStockAdjustmentService _service;

    public StockAdjustmentController(IStockAdjustmentService service)
    {
        _service = service;
    }

     [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var adjustments = await _service.GetAllStockAdjustmentsAsync();

        return Ok(adjustments);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var adjustment = await _service.GetStockAdjustmentByIdAsync(id);

        if (adjustment == null)
            return NotFound();

        return Ok(adjustment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateStockAdjustmentDto request)
    {
        var res = await _service.CreateStockAdjustmentAsync(request);

        return Created("", res);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateStockAdjustmentDto request)
    {
        var res = await _service.UpdateStockAdjustmentAsync(id, request);

        if (!res)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var res = await _service.DeleteStockAdjustmentAsync(id);

        if (!res)
            return NotFound();

        return NoContent();
    }
}
