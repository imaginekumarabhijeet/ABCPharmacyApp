using PharmacyApi.Models;

namespace PharmacyApi.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(MedicineRepository repository)
    {
        var existing = await repository.GetAllAsync();
        if (existing.Count > 0)
        {
            return;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var seed = new List<Medicine>
        {
            new() { FullName = "Paracetamol 500mg", Notes = "Pain reliever and fever reducer", ExpiryDate = today.AddDays(180), Quantity = 120, Price = 3.50m, Brand = "Calpol" },
            new() { FullName = "Amoxicillin 250mg", Notes = "Antibiotic capsule, course of 7 days", ExpiryDate = today.AddDays(15), Quantity = 45, Price = 8.25m, Brand = "Amoxil" },
            new() { FullName = "Cetirizine 10mg", Notes = "Antihistamine for seasonal allergies", ExpiryDate = today.AddDays(90), Quantity = 8, Price = 4.75m, Brand = "Zyrtec" },
            new() { FullName = "Ibuprofen 200mg", Notes = "NSAID pain and inflammation reliever", ExpiryDate = today.AddDays(10), Quantity = 5, Price = 5.00m, Brand = "Advil" },
            new() { FullName = "Omeprazole 20mg", Notes = "Reduces stomach acid production", ExpiryDate = today.AddDays(240), Quantity = 60, Price = 12.99m, Brand = "Prilosec" },
            new() { FullName = "Metformin 500mg", Notes = "Type 2 diabetes management", ExpiryDate = today.AddDays(365), Quantity = 3, Price = 6.40m, Brand = "Glucophage" },
            new() { FullName = "Loratadine 10mg", Notes = "Non-drowsy allergy relief", ExpiryDate = today.AddDays(25), Quantity = 75, Price = 7.20m, Brand = "Claritin" },
            new() { FullName = "Aspirin 325mg", Notes = "Pain relief and blood thinner", ExpiryDate = today.AddDays(400), Quantity = 200, Price = 2.99m, Brand = "Bayer" },
        };

        foreach (var medicine in seed)
        {
            await repository.AddAsync(medicine);
        }
    }
}
