using StudentCli.Data;
using StudentCli.Models;

namespace StudentCli.Tests;

public sealed class JsonStudentRepositoryTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "student-cli-tests", Guid.NewGuid().ToString("N"));

    private string FilePath => Path.Combine(_directory, "students.json");

    [Fact]
    public void Load_returns_empty_list_when_file_is_missing()
    {
        var repository = new JsonStudentRepository(FilePath);

        Assert.False(repository.Exists);
        Assert.Empty(repository.Load());
    }

    [Fact]
    public void Save_then_load_round_trips_students_including_turkish_characters()
    {
        var repository = new JsonStudentRepository(FilePath);
        Student[] students =
        [
            new() { Number = 1, FirstName = "Şükrü", LastName = "Öztürk", Section = "Ç" },
            new() { Number = 2, FirstName = "İrem", LastName = "Işık", Section = "5B" },
        ];

        repository.Save(students);
        var loaded = new JsonStudentRepository(FilePath).Load();

        Assert.True(repository.Exists);
        Assert.Equal(2, loaded.Count);
        Assert.Equal("Şükrü", loaded[0].FirstName);
        Assert.Equal("Işık", loaded[1].LastName);
        Assert.Contains("Öztürk", File.ReadAllText(FilePath));
    }

    [Fact]
    public void Load_throws_a_clear_error_for_corrupted_file()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(FilePath, "{ not json");

        var error = Assert.Throws<InvalidDataException>(() => new JsonStudentRepository(FilePath).Load());

        Assert.Contains("JSON", error.Message);
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }
}
