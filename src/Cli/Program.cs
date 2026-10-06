using Core.Dto;
using Core.Import;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

string extension = Path.GetExtension(path).ToLowerInvariant();

List<BookDto> books = [];
List<ReaderDto> readers = [];
List<string> errors = [];

if (extension == ".csv")
{
    var (bResult, rResult) = CsvImporter.Load(path);
    books = [.. bResult.Items];
    readers = [.. rResult.Items];
    errors = [.. bResult.Errors];
}
else if (extension == ".json")
{
    var bResult = JsonImporter.Load(path);
    books = [.. bResult.Items];
    errors = [.. bResult.Errors];
}
else
{
    Console.WriteLine($"Непідтримуваний формат файлу: {extension}");
    return 1;
}

int totalRows = books.Count + readers.Count + errors.Count;
double errorRate = totalRows > 0 ? (double)errors.Count / totalRows * 100 : 0;

Console.WriteLine($"Статистика імпорту: Усього = {totalRows}, Прийнято = {books.Count + readers.Count}, Пропущено = {errors.Count}, % помилок = {errorRate:F1}%");

Console.WriteLine($"\nЗавантажено книг: {books.Count}");
foreach (BookDto b in books.Take(5))
{
    Console.WriteLine($"  {b.Id,-6} {b.Isbn,-17} {b.Title,-30} {b.Year,4}");
}

if (readers.Count > 0)
{
    Console.WriteLine($"\nЗавантажено читачів: {readers.Count}");
    foreach (ReaderDto r in readers.Take(5))
    {
        Console.WriteLine($"  {r.Id,-6} {r.Name,-26} {r.Phone}");
    }
}

if (errors.Count > 0)
{
    Console.WriteLine($"\nПропущено рядків: {errors.Count}");
    foreach (string e in errors)
    {
        Console.WriteLine($"  ! {e}");
    }
}

return 0;