using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Categories;

public class UpdateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
