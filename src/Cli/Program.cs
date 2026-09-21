using System.Runtime.InteropServices;
using Core;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("CrossApp – практикум з крос-платформного програмування");
Console.WriteLine("Студент: Хромович Андрій, група ФЕІ-32с");
Console.WriteLine("CrossApp - Практика з крос-платформного програмування");
Console.WriteLine("Студент: Хромій Юрій, група КП-32мп");

Console.WriteLine(new string('-', 52));
EnvironmentReport report = EnvironmentInfo.Collect();

Console.WriteLine($"ОС (OSDescription) : {RuntimeInformation.OSDescription}");
Console.WriteLine($"ОС (Environment) : {Environment.OSVersion}");
Console.WriteLine($"Архітектура процесу : {RuntimeInformation.ProcessArchitecture}");
Console.WriteLine($"Версія .NET (CLR) : {Environment.Version}");
Console.WriteLine($"Runtime : {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"Каталог застосунку : {AppContext.BaseDirectory}");
Console.WriteLine($"Поточний каталог : {Environment.CurrentDirectory}");

Console.WriteLine(new string('-', 52));
Console.WriteLine($"ОС (OSDescription) : {report.OsDescription}");
Console.WriteLine($"Архітектура        : {report.ProcessArchitecture}");
Console.WriteLine($"Runtime            : {report.FrameworkDescription}");
Console.WriteLine($"RID (визначено)    : {report.DetectedRid}");
Console.WriteLine($"RID (від .NET)     : {report.ReportedRid}");
Console.WriteLine($"Каталог            : {report.BaseDirectory}");
Console.WriteLine($"Примітка збірки    : {report.BuildNote}");
Console.WriteLine(new string('-', 52));

Console.WriteLine("Предметна область: Бібліотека. Сутності: Book (видання), BookCopy (примірник), Reader (читач), Loan (видача).");
Console.WriteLine("Предметна область: Бібліотека. Сутності: Book (Книга), BookCopy (Примірник), Reader (Читач), Loan (Позика).");