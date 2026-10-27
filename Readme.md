# CrossApp

Наскрізний проєкт з крос-платформного програмування.

**Предметна область: Бібліотека**
Сутності: `Book` (видання), `BookCopy` (примірник), `Reader` (читач), `Loan` (видача).
Призначення: облік видач примірників книг читачам і повернень.

## Запуск та публікація

```bash
# Збірка всього solution
dotnet build

# Запуск CLI
dotnet run --project src/Cli --framework net10.0

# Framework-dependent публікація (потребує встановленого .NET 10 на цільовій машині)
dotnet publish src/Cli -c Release -r win-x64 --self-contained false -o publish/win-x64-fd

# Self-contained публікація (не потребує .NET, усі залежності всередині)
dotnet publish src/Cli -c Release -r win-x64 --self-contained true -o publish/win-x64-sc
```

## Структура

```text
CrossApp/
├── .gitignore
├── CrossApp.slnx
├── Readme.md
├── data/
│   ├── sample.csv
│   └── sample.json
└── src/
    ├── Core/
    │   ├── Core.csproj
    │   ├── EnvironmentInfo.cs
    │   ├── Dto/
    │   │   ├── Dtos.cs
    │   │   └── ImportResult.cs
    │   ├── Domain/
    │   │   ├── Book.cs
    │   │   ├── BookCopy.cs
    │   │   └── Loan.cs
    │   └── Import/
    │       ├── CsvImporter.cs
    │       └── JsonImporter.cs
    └── Cli/
        ├── Cli.csproj
        └── Program.cs
```

Каталоги в `Core` на весь семестр:
- `Core/Dto/` – record-типи формату даних (тиждень 3)
- `Core/Domain/` – сутності з поведінкою та інваріантами (тиждень 4)
- `Core/Storage/` – реалізації сховищ (тиждень 5)

## Доменна модель та інваріанти (ЛР 4)

Головні сутності процесу: `Loan` (видача), `BookCopy` (примірник книги) та агрегат `Book` (видання зі списком примірників `IReadOnlyList<BookCopy>`).

### Перелік реалізованих інваріантів:
1. **Обов'язкові ключі та атрибути примірника (`BookCopy.Create`)**:
   - `id` та `isbn` не можуть бути порожніми або складатися лише з пробілів (`ArgumentException`).
2. **Заборона повторної видачі примірника (`BookCopy.Issue` / `Loan.Open`)**:
   - не можна видати примірник, який уже перебуває у видачі (`IsIssued == true`) (`InvalidOperationException`).
3. **Заборона повернення невиданого примірника (`BookCopy.Return`)**:
   - не можна повернути примірник, який зараз не виданий (`IsIssued == false`) (`InvalidOperationException`).
4. **Обов'язкові параметри відкриття видачі (`Loan.Open` / `Loan.Create`)**:
   - ідентифікатор видачі `id` та читача `readerId` не порожні (`ArgumentException`), об'єкт `copy` не `null` (`ArgumentNullException`), дата видачі `issuedOn` коректна (`ArgumentOutOfRangeException`).
5. **Коректність дати повернення (`Loan.Close` / `Loan.Create`)**:
   - дата повернення `returnedOn` не може бути раніше дати видачі `IssuedOn` (`ArgumentOutOfRangeException`).
6. **Заборона повторного закриття видачі (`Loan.Close`)**:
   - не можна закрити видачу, яка вже закрита (`IsClosed == true`) (`InvalidOperationException`).
7. **Унікальність примірників та валідність книги (`Book.Create` / `Book.RegisterCopy`)**:
   - `id`, `isbn`, `title` книги не порожні (`ArgumentException`), рік видання в межах `1450..2100` (`ArgumentOutOfRangeException`); не можна зареєструвати два примірники з однаковим `Id` (`InvalidOperationException`); колекція `Copies` доступна назовні лише для читання (`IReadOnlyList<BookCopy>`).

## Середовище

- .NET SDK 10.0.400
- Windows 10 x64 (RID: win-x64)
- Visual Studio Code + C# Dev Kit

### Порівняння режимів публікації

| RID     | Режим                | Розмір publish | Потрібен runtime |
| ------- | -------------------- | -------------- | ---------------- |
| win-x64 | self-contained       | ~76.6 MB       | ні               |
| win-x64 | framework-dependent  | ~0.19 MB       | так (.NET 10)    |

*Різниця полягає у тому, що self-contained публікація включає в себе середовище виконання .NET, тоді як framework-dependent покладається на те, що .NET вже встановлений на машині користувача.*
