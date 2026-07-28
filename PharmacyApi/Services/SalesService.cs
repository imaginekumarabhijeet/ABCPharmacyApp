using PharmacyApi.Data;
using PharmacyApi.Models;

namespace PharmacyApi.Services;

public class SalesService(MedicineRepository medicineRepository, SalesRepository salesRepository)
{
    public async Task<(bool Success, string? Error, SaleRecord? Sale)> SellAsync(string medicineId, int quantity)
    {
        var (success, error, medicine) = await medicineRepository.TryDecrementQuantityAsync(medicineId, quantity);
        if (!success || medicine is null)
        {
            return (false, error, null);
        }

        var sale = new SaleRecord
        {
            MedicineId = medicine.Id,
            MedicineName = medicine.FullName,
            QuantitySold = quantity,
            PriceAtSale = medicine.Price,
            SaleDate = DateTime.UtcNow
        };

        await salesRepository.AddAsync(sale);
        return (true, null, sale);
    }
}
