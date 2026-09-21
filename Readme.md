# CrossApp

Наскрізний проєкт з крос-платформного програмування.

**Предметна область: Бібліотека**
Сутності: `Book` (видання), `BookCopy` (примірник), `Reader` (читач), `Loan` (видача).
Призначення: облік видач примірників книг читачам і повернень.

## Запуск
## Запуск та публікація

```bash
# Збірка всього solution
dotnet build

# Запуск CLI
dotnet run --project src/Cli

# Framework-dependent публікація (потребує встановленого .NET 10 на цільовій машині)
dotnet publish src/Cli -c Release -r win-x64 --self-contained false -o publish/win-x64-fd

# Self-contained публікація (не потребує .NET, усі залежності всередині)
dotnet publish src/Cli -c Release -r win-x64 --self-contained true -o publish/win-x64-sc
```

## Структура

```
```text
CrossApp/
├── .gitignore
├── global.json
├── CrossApp.slnx
├── CrossApp.sln
├── README.md
└── src/
    ├── Core/
    │   ├── Core.csproj
    │   └── EnvironmentInfo.cs
    └── Cli/
        ├── Cli.csproj
        └── Program.cs
```

Каталоги в `Core` на весь семестр:
- `Core/Dto/` – record-типи формату даних (тиждень 3)
- `Core/Domain/` – сутності з поведінкою та інваріантами (тиждень 4)
- `Core/Storage/` – реалізації сховищ (тиждень 5)

## Середовище

- .NET SDK 10.0.400
- .NET SDK 10.0
- Windows 10 x64 (RID: win-x64)
- Visual Studio Code + C# Dev Kit

### Порівняння режимів публікації

| RID     | Режим                | Розмір publish | Потрібен runtime |
| ------- | -------------------- | -------------- | ---------------- |
| win-x64 | self-contained       | ~76.6 MB       | ні               |
| win-x64 | framework-dependent  | ~0.19 MB       | так (.NET 10)    |

*Різниця полягає у тому, що self-contained публікація включає в себе середовище виконання .NET, тоді як framework-dependent покладається на те, що .NET вже встановлений на машині користувача.*
