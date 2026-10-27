using System.Text;
using Core.Domain;
using Core.Dto;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Сценарій 1: успіх ===");

Book book = Book.Create("B-001", "978-0132350884", "Clean Code", 2008);
BookCopy copy1 = book.RegisterCopy("BC-001");
BookCopy copy2 = book.RegisterCopy("BC-002");

Console.WriteLine($"Книга: {book}");
Console.WriteLine($"Примірник до видачі: {copy1}");

Loan loan1 = Loan.Open("L-001", copy1, "R-001", new DateOnly(2026, 10, 1));
Console.WriteLine($"Відкрито видачу:     {loan1}");
Console.WriteLine($"Примірник у видачі:  {copy1}");

loan1.Close(new DateOnly(2026, 10, 10));
Console.WriteLine($"Закрито видачу:      {loan1}");
Console.WriteLine($"Примірник після повернення: {copy1}");

LoanDto loanDto = loan1.ToDto();
Loan restoredLoan = Loan.FromDto(loanDto);
Console.WriteLine($"Відновлено з DTO:    {restoredLoan}");

Console.WriteLine();
Console.WriteLine("=== Сценарій 2: порушення інваріантів ===");

Loan activeLoan = Loan.Open("L-002", copy2, "R-002", new DateOnly(2026, 10, 5));

TryDo("повторна видача вже виданого примірника",
    () => Loan.Open("L-003", copy2, "R-001", new DateOnly(2026, 10, 6)));

TryDo("порожній ISBN примірника",
    () => BookCopy.Create("BC-003", "   "));

TryDo("дата повернення раніше дати видачі",
    () => activeLoan.Close(new DateOnly(2026, 10, 1)));

TryDo("повторне закриття вже закритої видачі",
    () => loan1.Close(new DateOnly(2026, 10, 12)));

TryDo("повернення невиданого примірника",
    () => copy1.Return());

TryDo("дублювання примірника у книзі",
    () => book.RegisterCopy("BC-001"));

TryDo("відновлення з некоректного DTO (FromDto)",
    () => Loan.FromDto(new LoanDto("L-999", "BC-001", "R-001", new DateOnly(2026, 10, 10), new DateOnly(2026, 10, 1))));

Console.WriteLine();
Console.WriteLine($"Стан активної видачі після всіх відмов (не змінився): {activeLoan}");
Console.WriteLine($"Стан примірника {copy2.Id} після всіх відмов (не змінився): {copy2}");

static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  {title}: виняток НЕ спрацював — інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  {title}: {ex.GetType().Name} — {ex.Message}");
    }
}