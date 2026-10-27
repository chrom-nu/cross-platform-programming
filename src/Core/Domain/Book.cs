using Core.Dto;

namespace Core.Domain;

public sealed class Book
{
    private const int MinPublicationYear = 1450;
    private const int MaxPublicationYear = 2100;

    private readonly List<BookCopy> _copies = [];

    public string Id { get; }
    public string Isbn { get; }
    public string Title { get; }
    public int Year { get; }
    public IReadOnlyList<BookCopy> Copies => _copies.AsReadOnly();

    private Book(string id, string isbn, string title, int year)
    {
        Id = id;
        Isbn = isbn;
        Title = title;
        Year = year;
    }

    public static Book Create(string id, string isbn, string title, int year)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор книги обов'язковий", nameof(id));
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN не може бути порожнім", nameof(isbn));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва книги не може бути порожньою", nameof(title));
        if (year < MinPublicationYear || year > MaxPublicationYear)
            throw new ArgumentOutOfRangeException(
                nameof(year),
                year,
                $"Рік видання має бути в межах {MinPublicationYear}..{MaxPublicationYear}");

        return new Book(id.Trim(), isbn.Trim().ToUpperInvariant(), title.Trim(), year);
    }

    public BookCopy RegisterCopy(string copyId)
    {
        if (string.IsNullOrWhiteSpace(copyId))
            throw new ArgumentException("Ідентифікатор примірника не може бути порожнім", nameof(copyId));

        string normalizedId = copyId.Trim();
        if (_copies.Any(c => string.Equals(c.Id, normalizedId, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException(
                $"Примірник {normalizedId} вже зареєстрований для книги {Id} [{Isbn}]");

        var copy = BookCopy.Create(normalizedId, Isbn);
        _copies.Add(copy);
        return copy;
    }

    public BookDto ToDto() => new(Id, Isbn, Title, Year);

    public static Book FromDto(BookDto dto) =>
        Create(dto.Id, dto.Isbn, dto.Title, dto.Year);

    public override string ToString() =>
        $"{Id} [{Isbn}] \"{Title}\" ({Year}) — примірників: {_copies.Count}";
}
