using System.Text.Json;

namespace PharmacyApi.Data;


public class JsonFileStore<T>
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonFileStore(string filePath)
    {
        _filePath = filePath;

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<List<T>> ReadAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            return await ReadFromDiskAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    
    public async Task<TResult> MutateAsync<TResult>(Func<List<T>, TResult> mutate)
    {
        await _lock.WaitAsync();
        try
        {
            var items = await ReadFromDiskAsync();
            var result = mutate(items);
            await WriteToDiskAsync(items);
            return result;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<T>> ReadFromDiskAsync()
    {
        await using var stream = new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true);

        if (stream.Length == 0)
        {
            return [];
        }

        return await JsonSerializer.DeserializeAsync<List<T>>(stream, SerializerOptions) ?? [];
    }

    private async Task WriteToDiskAsync(List<T> items)
    {
        
        var tempPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";

        await using (var stream = new FileStream(
            tempPath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true))
        {
            await JsonSerializer.SerializeAsync(stream, items, SerializerOptions);
        }

        File.Move(tempPath, _filePath, overwrite: true);
    }
}
