using System.Globalization;
using Core.Dto;

namespace Core.Import;

public static class CsvImporter
{
    private const char Separator = ';';

    public static (ImportResult<BookDto> Books, ImportResult<ReaderDto> Readers) Load(string path)
    {
        var books = new List<BookDto>();
        var readers = new List<ReaderDto>();
        var errors = new List<string>();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;
            if (number == 1 && line.StartsWith("Type", StringComparison.OrdinalIgnoreCase))
                continue;

            switch (ParseLine(line))
            {
                case ParseBookOk b:
                    books.Add(b.Value);
                    break;
                case ParseReaderOk r:
                    readers.Add(r.Value);
                    break;
                case ParseFailed failed:
                    errors.Add($"рядок {number}: {failed.Reason}");
                    break;
            }
        }

        return (new ImportResult<BookDto>(books, errors), new ImportResult<ReaderDto>(readers, []));
    }

    private static ParseOutcome ParseLine(string line)
    {
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

        return parts switch
        {
            { Length: < 4 } => new ParseFailed($"очікую щонайменше 4 колонки, отримав {parts.Length}"),
            ["B", _, "", _, _, ..] or ["B", _, _, "", _, ..]
                => new ParseFailed("ISBN або назва порожні"),
            ["B", var id, var isbn, var title, var year, ..] when !int.TryParse(year, NumberStyles.Integer, CultureInfo.InvariantCulture, out int y) || y < 1450 || y > DateTime.Now.Year
                => new ParseFailed($"рік '{year}' поза допустимими межами"),
            ["B", var id, var isbn, var title, var year]
                => new ParseBookOk(new BookDto(id, isbn, title, int.Parse(year, CultureInfo.InvariantCulture))),
            ["B", var id, var isbn, var title, var year, ..]
                => new ParseBookOk(new BookDto(id, isbn, title, int.Parse(year, CultureInfo.InvariantCulture))),
            ["R", "", _, _] or ["R", _, "", _]
                => new ParseFailed("Назва читача або телефон порожні"),
            ["R", var rId, var rName, var rPhone]
                => new ParseReaderOk(new ReaderDto(rId, rName, rPhone)),
            [var type, ..] when type != "B" && type != "R"
                => new ParseFailed($"невідомий тип запису: {type}"),
            _ => new ParseFailed($"неправильний формат рядка (колонок: {parts.Length})")
        };
    }

    private abstract record ParseOutcome;
    private sealed record ParseBookOk(BookDto Value) : ParseOutcome;
    private sealed record ParseReaderOk(ReaderDto Value) : ParseOutcome;
    private sealed record ParseFailed(string Reason) : ParseOutcome;
}
