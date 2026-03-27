using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Suppliers;

public class CreateSupplierDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;
}
