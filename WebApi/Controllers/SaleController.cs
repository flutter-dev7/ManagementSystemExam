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

    [HttpGet("by-date/fromdate={fromDate:datetime}&toDate={toDate:datetime}")]
    public async Task<IActionResult> GetSalesByDate(DateTime fromDate, DateTime toDate)
    {
        var sales = await _service.GetSalesByDateAsync(fromDate, toDate);

        return Ok(sales);
    }

    [HttpGet("top-products")]
    public  async Task<IActionResult> GetTopProducts()
    {
        var sales = await _service.GetTopProductsAsync();

        return Ok(sales);
    }

    [HttpGet("daily-revenue")]
    public async Task<IActionResult> GetDailyRevenueAsync()
    {
        var reports = await _service.GetDailyRevenueAsync();

        return Ok(reports);
    }

    [HttpGet("dashboard/statistics")]
    public async Task<IActionResult> GetDashboardStatisticsAsync()
    {
        var dashboard = await _service.GetDashboardStatisticsAsync();

        return Ok(dashboard);
    }
}
