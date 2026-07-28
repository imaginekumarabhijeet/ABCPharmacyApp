using PharmacyApi.Models;

namespace PharmacyApi.Data;

public class MedicineRepository
{
    private readonly JsonFileStore<Medicine> _store;

    public MedicineRepository(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "medicines.json");
        _store = new JsonFileStore<Medicine>(path);
    }

    public async Task<List<Medicine>> GetAllAsync(string? search = null)
    {
        var items = await _store.ReadAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            items = items
                .Where(m => Matches(m.FullName, term) || Matches(m.Brand, term) || Matches(m.Notes, term))
                .ToList();
        }

        return items.OrderBy(m => m.FullName).ToList();
    }

    public async Task<Medicine?> GetByIdAsync(string id)
    {
        var items = await _store.ReadAllAsync();
        return items.FirstOrDefault(m => m.Id == id);
    }

    public Task<Medicine> AddAsync(Medicine medicine)
    {
        return _store.MutateAsync(items =>
        {
            items.Add(medicine);
            return medicine;
        });
    }

    /// <summary>
    /// Atomically checks stock and decrements it. Runs inside the store's lock so concurrent
    /// sell requests for the same medicine can't oversell.
    /// </summary>
    public Task<(bool Success, string? Error, Medicine? Medicine)> TryDecrementQuantityAsync(string id, int quantity)
    {
        return _store.MutateAsync(items =>
        {
            var medicine = items.FirstOrDefault(m => m.Id == id);
            if (medicine is null)
            {
                return (false, "Medicine not found.", (Medicine?)null);
            }

            if (medicine.Quantity < quantity)
            {
                return (false, $"Insufficient stock. Only {medicine.Quantity} unit(s) available.", (Medicine?)null);
            }

            medicine.Quantity -= quantity;
            return (true, (string?)null, medicine);
        });
    }

    private static bool Matches(string? value, string term) =>
        !string.IsNullOrEmpty(value) && value.Contains(term, StringComparison.OrdinalIgnoreCase);
}
