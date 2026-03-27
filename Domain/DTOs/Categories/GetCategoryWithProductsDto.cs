using System;
using Domain.DTOs.Products;

namespace Domain.DTOs.Categories;

public class GetCategoryWithProductsDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<GetProductInCategoryDto> Products { get; set; } = new();
}
