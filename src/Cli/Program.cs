using Core;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("CrossApp - Практика з крос-платформного програмування");
Console.WriteLine("Студент: Хромій Юрій, група КП-32мп");

EnvironmentReport report = EnvironmentInfo.Collect();

Console.WriteLine(new string('-', 52));
Console.WriteLine($"ОС (OSDescription) : {report.OsDescription}");
Console.WriteLine($"Архітектура        : {report.ProcessArchitecture}");
Console.WriteLine($"Runtime            : {report.FrameworkDescription}");
Console.WriteLine($"RID (визначено)    : {report.DetectedRid}");
Console.WriteLine($"RID (від .NET)     : {report.ReportedRid}");
Console.WriteLine($"Каталог            : {report.BaseDirectory}");
Console.WriteLine($"Примітка збірки    : {report.BuildNote}");
Console.WriteLine(new string('-', 52));

Console.WriteLine("Предметна область: Бібліотека. Сутності: Book (Книга), BookCopy (Примірник), Reader (Читач), Loan (Позика).");