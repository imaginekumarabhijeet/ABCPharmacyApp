using System.ComponentModel.DataAnnotations;

namespace PharmacyApi.Models;

public class SellRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Quantity sold must be at least 1.")]
    public int Quantity { get; set; }
}
