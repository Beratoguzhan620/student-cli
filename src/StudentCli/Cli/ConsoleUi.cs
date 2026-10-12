using StudentCli.Models;
using StudentCli.Services;
using StudentCli.Text;

namespace StudentCli.Cli;

public sealed class ConsoleUi(StudentService service, string dataFilePath)
{
    private static readonly (string Key, string Shortcut, string Label)[] MenuItems =
    [
        ("1", "L", "Öğrencileri listele"),
        ("2", "E", "Öğrenci ekle"),
        ("3", "A", "Öğrenci ara"),
        ("4", "G", "Öğrenci güncelle"),
        ("5", "S", "Öğrenci sil"),
        ("0", "X", "Çıkış"),
    ];

    public void Run()
    {
        WriteHeader();

        while (true)
        {
            WriteMenu();
            var choice = Prompt("Seçiminiz");
            if (choice is null)
            {
                return; // input stream closed (Ctrl+Z / Ctrl+D)
            }

            Console.WriteLine();
            switch (choice.Trim().ToUpperInvariant())
            {
                case "1" or "L": ListStudents(); break;
                case "2" or "E": AddStudent(); break;
                case "3" or "A": SearchStudents(); break;
                case "4" or "G": UpdateStudent(); break;
                case "5" or "S": DeleteStudent(); break;
                case "0" or "X":
                    Info("Görüşmek üzere.");
                    return;
                default:
                    Error("Geçersiz seçim. Menüdeki numaralardan birini girin.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private void WriteHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Öğrenci Yönetimi");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"Veri dosyası: {dataFilePath}");
        Console.ResetColor();
        Console.WriteLine();
    }

    private static void WriteMenu()
    {
        foreach (var (key, shortcut, label) in MenuItems)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  {key}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($" ({shortcut})  ");
            Console.ResetColor();
            Console.WriteLine(label);
        }

        Console.WriteLine();
    }

    private void ListStudents()
    {
        Title("Öğrenci listesi");
        var students = service.GetAll();
        if (students.Count == 0)
        {
            Info("Henüz kayıtlı öğrenci yok. Eklemek için 2'yi seçin.");
            return;
        }

        WriteStudents(students);
    }

    private void AddStudent()
    {
        Title("Öğrenci ekle");
        Hint("Vazgeçmek için alanı boş bırakıp Enter'a basın.");

        var number = PromptNumber("Numara");
        if (number is null)
        {
            return;
        }

        if (service.FindByNumber(number.Value) is not null)
        {
            Error($"{number} numaralı bir öğrenci zaten kayıtlı.");
            return;
        }

        var firstName = PromptRequired("Ad");
        if (firstName is null) return;
        var lastName = PromptRequired("Soyad");
        if (lastName is null) return;
        var section = PromptRequired("Şube");
        if (section is null) return;

        var preview = $"{NameFormatter.ToTitleCase(firstName)} {NameFormatter.ToTitleCase(lastName)} ({NameFormatter.ToSection(section)})";
        if (!Confirm($"{preview} kaydedilsin mi?"))
        {
            Info("Kayıt iptal edildi.");
            return;
        }

        Report(service.Add(number.Value, firstName, lastName, section));
    }

    private void SearchStudents()
    {
        Title("Öğrenci ara");
        var term = Prompt("Ad, soyad, numara veya şube");
        if (string.IsNullOrWhiteSpace(term))
        {
            return;
        }

        var results = service.Search(term);
        if (results.Count == 0)
        {
            Info($"\"{term.Trim()}\" için sonuç bulunamadı.");
            return;
        }

        WriteStudents(results);
    }

    private void UpdateStudent()
    {
        Title("Öğrenci güncelle");
        var student = PromptExistingStudent();
        if (student is null)
        {
            return;
        }

        WriteStudents([student]);
        Hint("Değiştirmek istemediğiniz alanı boş bırakın.");
        var firstName = Prompt($"Ad [{student.FirstName}]");
        var lastName = Prompt($"Soyad [{student.LastName}]");
        var section = Prompt($"Şube [{student.Section}]");

        Report(service.Update(student.Number, firstName, lastName, section));
    }

    private void DeleteStudent()
    {
        Title("Öğrenci sil");
        if (service.Count == 0)
        {
            Info("Silinecek öğrenci yok.");
            return;
        }

        var student = PromptExistingStudent();
        if (student is null)
        {
            return;
        }

        WriteStudents([student]);
        if (!Confirm("Bu öğrenci silinsin mi?"))
        {
            Info("Silme iptal edildi.");
            return;
        }

        Report(service.Delete(student.Number));
    }

    private Student? PromptExistingStudent()
    {
        var number = PromptNumber("Öğrenci numarası");
        if (number is null)
        {
            return null;
        }

        var student = service.FindByNumber(number.Value);
        if (student is null)
        {
            Error($"{number} numaralı öğrenci bulunamadı.");
        }

        return student;
    }

    private static void WriteStudents(IReadOnlyList<Student> students)
    {
        var table = new ConsoleTable("#", "Numara", "Ad", "Soyad", "Şube").AlignRight(0).AlignRight(1);
        for (var i = 0; i < students.Count; i++)
        {
            var s = students[i];
            table.AddRow(i + 1, s.Number, s.FirstName, s.LastName, s.Section);
        }

        table.Write();
        Hint($"{students.Count} öğrenci");
    }

    private static string? Prompt(string label)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{label}: ");
        Console.ResetColor();
        return Console.ReadLine();
    }

    private static string? PromptRequired(string label)
    {
        var value = Prompt(label);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static int? PromptNumber(string label)
    {
        while (true)
        {
            var input = Prompt(label);
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (int.TryParse(input.Trim(), out var number) && number > 0)
            {
                return number;
            }

            Error("Lütfen pozitif bir tam sayı girin.");
        }
    }

    private static bool Confirm(string question)
    {
        while (true)
        {
            var answer = Prompt($"{question} (E/H)")?.Trim().ToUpperInvariant();
            switch (answer)
            {
                case "E" or "EVET": return true;
                case "H" or "HAYIR" or "" or null: return false;
                default: Error("Lütfen E veya H girin."); break;
            }
        }
    }

    private static void Report(OperationResult result)
    {
        if (result.Success) Success(result.Message);
        else Error(result.Message);
    }

    private static void Title(string text) => WriteColored($"— {text} —", ConsoleColor.Cyan);
    private static void Hint(string text) => WriteColored(text, ConsoleColor.DarkGray);
    private static void Info(string text) => WriteColored(text, ConsoleColor.Gray);
    private static void Success(string text) => WriteColored($"✓ {text}", ConsoleColor.Green);
    private static void Error(string text) => WriteColored($"✗ {text}", ConsoleColor.Red);

    private static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
