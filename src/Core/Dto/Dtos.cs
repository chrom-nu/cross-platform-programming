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
public record BookCopyDto(
    string Id,
    string Isbn,
    bool IsIssued);
public record LoanDto(
    string Id,
    string CopyId,
    string ReaderId,
    DateOnly IssuedOn,
    DateOnly? ReturnedOn);