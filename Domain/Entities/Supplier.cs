using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Supplier
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    // navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
