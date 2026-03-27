using System;

namespace Domain.DTOs.Products;

public class GetProductInCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
