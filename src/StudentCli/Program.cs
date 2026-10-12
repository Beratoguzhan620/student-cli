using System.Text;
using StudentCli.Cli;
using StudentCli.Data;
using StudentCli.Services;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var dataFile = ParseDataFile(args) ?? "students.json";

try
{
    var repository = new JsonStudentRepository(dataFile);
    var service = new StudentService(repository, seedIfEmpty: SampleData.Students);
    new ConsoleUi(service, repository.FilePath).Run();
    return 0;
}
catch (InvalidDataException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(ex.Message);
    Console.ResetColor();
    Console.Error.WriteLine("Dosyayı düzeltin veya --data ile başka bir dosya belirtin.");
    return 1;
}

static string? ParseDataFile(string[] args)
{
    var index = Array.FindIndex(args, a => a is "--data" or "-d");
    return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
}
