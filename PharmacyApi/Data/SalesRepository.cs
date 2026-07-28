using PharmacyApi.Models;

namespace PharmacyApi.Data;

public class SalesRepository
{
    private readonly JsonFileStore<SaleRecord> _store;

    public SalesRepository(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "sales.json");
        _store = new JsonFileStore<SaleRecord>(path);
    }

    public async Task<List<SaleRecord>> GetAllAsync()
    {
        var items = await _store.ReadAllAsync();
        return items.OrderByDescending(s => s.SaleDate).ToList();
    }

    public Task<SaleRecord> AddAsync(SaleRecord sale)
    {
        return _store.MutateAsync(items =>
        {
            items.Add(sale);
            return sale;
        });
    }
}
