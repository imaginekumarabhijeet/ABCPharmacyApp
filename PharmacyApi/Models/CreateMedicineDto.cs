using System.ComponentModel.DataAnnotations;

namespace PharmacyApi.Models;

public class CreateMedicineDto
{
    [Required, StringLength(200, MinimumLength = 1)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Notes { get; set; }

    [Required]
    public DateOnly ExpiryDate { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, 1000000)]
    public decimal Price { get; set; }

    [Required, StringLength(100, MinimumLength = 1)]
    public string Brand { get; set; } = string.Empty;
}
