# CrossApp

Наскрізний проєкт з крос-платформного програмування.

**Предметна область: Бібліотека**
Сутності: `Book` (видання), `BookCopy` (примірник), `Reader` (читач), `Loan` (видача).
Призначення: облік видач примірників книг читачам і повернень.

## Запуск

```bash
dotnet build
dotnet run --project src/Cli
```

## Структура

```
CrossApp/
├── .gitignore
├── global.json
├── CrossApp.slnx
├── README.md
└── src/
    └── Cli/
        ├── Cli.csproj
        └── Program.cs
```

## Середовище

- .NET SDK 10.0.400
- Windows 10 x64 (RID: win-x64)
- Visual Studio Code + C# Dev Kit
