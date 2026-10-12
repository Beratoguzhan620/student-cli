using System.Text.Encodings.Web;
using System.Text.Json;
using StudentCli.Models;

namespace StudentCli.Data;

public sealed class JsonStudentRepository(string filePath) : IStudentRepository
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public string FilePath { get; } = Path.GetFullPath(filePath);

    public bool Exists => File.Exists(FilePath);

    public IReadOnlyList<Student> Load()
    {
        if (!Exists)
        {
            return [];
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            return string.IsNullOrWhiteSpace(json)
                ? []
                : JsonSerializer.Deserialize<List<Student>>(json, Options) ?? [];
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"'{FilePath}' dosyası okunamadı: geçerli bir JSON değil.", ex);
        }
    }

    public void Save(IEnumerable<Student> students)
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Write to a temp file first so a crash mid-write never leaves a half-written data file.
        var tempPath = FilePath + ".tmp";
        File.WriteAllText(tempPath, JsonSerializer.Serialize(students, Options));
        File.Move(tempPath, FilePath, overwrite: true);
    }
}
