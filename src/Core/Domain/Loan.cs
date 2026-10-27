using Core.Dto;

namespace Core.Domain;

public sealed class Loan
{
    private readonly BookCopy? _copy;

    public string Id { get; }
    public string CopyId { get; }
    public string ReaderId { get; }
    public DateOnly IssuedOn { get; }
    public DateOnly? ReturnedOn { get; private set; }
    public bool IsClosed => ReturnedOn.HasValue;

    private Loan(
        string id,
        string copyId,
        string readerId,
        DateOnly issuedOn,
        DateOnly? returnedOn,
        BookCopy? copy = null)
    {
        Id = id;
        CopyId = copyId;
        ReaderId = readerId;
        IssuedOn = issuedOn;
        ReturnedOn = returnedOn;
        _copy = copy;
    }

    public static Loan Open(string id, BookCopy copy, string readerId, DateOnly issuedOn)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор видачі обов'язковий", nameof(id));
        if (copy is null)
            throw new ArgumentNullException(nameof(copy), "Примірник книги обов'язковий для відкриття видачі");
        if (string.IsNullOrWhiteSpace(readerId))
            throw new ArgumentException("Ідентифікатор читача не може бути порожнім", nameof(readerId));
        if (issuedOn == default)
            throw new ArgumentOutOfRangeException(nameof(issuedOn), issuedOn, "Дата видачі має бути коректною");

        copy.Issue();

        return new Loan(id.Trim(), copy.Id, readerId.Trim(), issuedOn, null, copy);
    }

    public static Loan Open(BookCopy copy, string readerId, DateOnly issuedOn)
    {
        if (copy is null)
            throw new ArgumentNullException(nameof(copy), "Примірник книги обов'язковий для відкриття видачі");

        return Open($"L-{copy.Id}", copy, readerId, issuedOn);
    }

    public static Loan Create(
        string id,
        string copyId,
        string readerId,
        DateOnly issuedOn,
        DateOnly? returnedOn = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор видачі обов'язковий", nameof(id));
        if (string.IsNullOrWhiteSpace(copyId))
            throw new ArgumentException("Ідентифікатор примірника не може бути порожнім", nameof(copyId));
        if (string.IsNullOrWhiteSpace(readerId))
            throw new ArgumentException("Ідентифікатор читача не може бути порожнім", nameof(readerId));
        if (issuedOn == default)
            throw new ArgumentOutOfRangeException(nameof(issuedOn), issuedOn, "Дата видачі має бути коректною");
        if (returnedOn.HasValue && returnedOn.Value < issuedOn)
            throw new ArgumentOutOfRangeException(
                nameof(returnedOn),
                returnedOn.Value,
                $"Дата повернення ({returnedOn.Value:yyyy-MM-dd}) не може бути раніше дати видачі ({issuedOn:yyyy-MM-dd})");

        return new Loan(id.Trim(), copyId.Trim(), readerId.Trim(), issuedOn, returnedOn);
    }

    public void Close(DateOnly returnedOn)
    {
        if (IsClosed)
            throw new InvalidOperationException(
                $"Видача {Id} (примірник {CopyId}) вже закрита {ReturnedOn:yyyy-MM-dd}, повторне закриття неможливе");
        if (returnedOn < IssuedOn)
            throw new ArgumentOutOfRangeException(
                nameof(returnedOn),
                returnedOn,
                $"Дата повернення ({returnedOn:yyyy-MM-dd}) не може бути раніше дати видачі ({IssuedOn:yyyy-MM-dd}) для видачі {Id}");

        ReturnedOn = returnedOn;
        if (_copy is { IsIssued: true })
        {
            _copy.Return();
        }
    }

    public LoanDto ToDto() => new(Id, CopyId, ReaderId, IssuedOn, ReturnedOn);

    public static Loan FromDto(LoanDto dto) =>
        Create(dto.Id, dto.CopyId, dto.ReaderId, dto.IssuedOn, dto.ReturnedOn);

    public override string ToString() =>
        IsClosed
            ? $"{Id}: примірник {CopyId} -> читач {ReaderId} (видано {IssuedOn:yyyy-MM-dd}, повернено {ReturnedOn:yyyy-MM-dd})"
            : $"{Id}: примірник {CopyId} -> читач {ReaderId} (видано {IssuedOn:yyyy-MM-dd}, активна)";
}
