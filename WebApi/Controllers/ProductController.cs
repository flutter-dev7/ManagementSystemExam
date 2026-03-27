using System;
using Domain.DTOs.Products;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var products = await _service.GetAllProductsAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var product = await _service.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProductDto request)
    {
        var res = await _service.CreateProductAsync(request);

        return Created("", res);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateProductDto request)
    {
        var res = await _service.UpdateProductAsync(id, request);

        if (!res)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var res = await _service.DeleteProductAsync(id);

        if (!res)
            return NotFound();

        return NoContent();
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetProductsLowStock()
    {
        var products = await _service.GetProductsLowStock();

        return Ok(products);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetProductsStatistics()
    {
        var product = await _service.GetProductsStatistics();

        return Ok(product);
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> GetProductsDetails(int id)
    {
        var product = await _service.GetProductsDetails(id);

        if(product == null)
            return NotFound();

        return Ok(product);
    }
}
