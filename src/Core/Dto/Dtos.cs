namespace Core.Dto;

public record BookDto(
    string Id,
    string Isbn,
    string Title,
    int Year);

public record ReaderDto(
    string Id,
    string Name,
    string Phone);
