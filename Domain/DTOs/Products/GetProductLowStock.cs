using System;

namespace Domain.DTOs.Products;

public class GetProductLowStock
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
}
