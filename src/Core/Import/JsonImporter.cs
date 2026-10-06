using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class JsonImporter
{
    public static ImportResult<BookDto> Load(string path)
    {
        var errors = new List<string>();
        string json = File.ReadAllText(path);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        List<BookDto> items = [];
        try
        {
            items = JsonSerializer.Deserialize<List<BookDto>>(json, options) ?? [];
        }
        catch (JsonException ex)
        {
            errors.Add($"Помилка десеріалізації JSON: {ex.Message}");
        }

        return new ImportResult<BookDto>(items, errors);
    }
}
