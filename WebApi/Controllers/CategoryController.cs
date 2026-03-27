using System;
using Domain.DTOs.Categories;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var categories = await _service.GetAllCategoriesAsync();

        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var category = await _service.GetCategoryByIdAsync(id);

        if(category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryDto request)
    {
        var res = await _service.CreateCategoryAsync(request);

        return Created("", res);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateCategoryDto request)
    {
        var res = await _service.UpdateCategoryAsync(id, request);

        if(!res)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var res = await _service.DeleteCategoryAsync(id);

        if(!res)
            return NotFound();

        return NoContent();
    }

    [HttpGet("with-products")]
    public async Task<IActionResult> GetCategoriesWithProducts()
    {
        var categories = await _service.GetCategoriesWithProducts();

        return Ok(categories);
    }
}
