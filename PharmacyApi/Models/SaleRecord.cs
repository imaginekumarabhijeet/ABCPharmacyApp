namespace PharmacyApi.Models;

public class SaleRecord
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string MedicineId { get; set; } = string.Empty;
    public string MedicineName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal PriceAtSale { get; set; }
    public decimal TotalAmount => Math.Round(PriceAtSale * QuantitySold, 2);
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
}
