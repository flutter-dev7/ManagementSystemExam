using System;

namespace Domain.DTOs.Suppliers;

public class GetWithProductDto
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public List<string> Products { get; set; } = new();
}
