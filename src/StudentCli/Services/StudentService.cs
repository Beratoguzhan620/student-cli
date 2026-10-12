using StudentCli.Data;
using StudentCli.Models;
using StudentCli.Text;

namespace StudentCli.Services;

public sealed class StudentService
{
    public const int MaxSectionLength = 3;

    private readonly IStudentRepository _repository;
    private readonly List<Student> _students;

    public StudentService(IStudentRepository repository, IEnumerable<Student>? seedIfEmpty = null)
    {
        _repository = repository;

        if (!repository.Exists && seedIfEmpty is not null)
        {
            _students = seedIfEmpty.Select(Clone).ToList();
            _repository.Save(_students);
        }
        else
        {
            _students = repository.Load().ToList();
        }
    }

    public int Count => _students.Count;

    public IReadOnlyList<Student> GetAll() =>
        _students
            .OrderBy(s => s.Section, StringComparer.Ordinal)
            .ThenBy(s => s.Number)
            .ToList();

    public Student? FindByNumber(int number) => _students.Find(s => s.Number == number);

    public IReadOnlyList<Student> Search(string? term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return GetAll();
        }

        term = term.Trim();
        var isNumber = int.TryParse(term, out var number);

        return GetAll()
            .Where(s => (isNumber && s.Number == number)
                        || NameFormatter.ContainsIgnoreCase(s.FullName, term)
                        || string.Equals(s.Section, NameFormatter.ToSection(term), StringComparison.Ordinal))
            .ToList();
    }

    public OperationResult Add(int number, string? firstName, string? lastName, string? section)
    {
        if (FindByNumber(number) is not null)
        {
            return OperationResult.Fail($"{number} numaralı bir öğrenci zaten kayıtlı.");
        }

        var student = new Student
        {
            Number = number,
            FirstName = NameFormatter.ToTitleCase(firstName),
            LastName = NameFormatter.ToTitleCase(lastName),
            Section = NameFormatter.ToSection(section),
        };

        var error = Validate(student);
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        _students.Add(student);
        Persist();
        return OperationResult.Ok($"{student.FullName} eklendi.");
    }

    public OperationResult Update(int number, string? firstName, string? lastName, string? section)
    {
        var existing = FindByNumber(number);
        if (existing is null)
        {
            return OperationResult.Fail($"{number} numaralı öğrenci bulunamadı.");
        }

        // Blank input keeps the current value, so the user only types what changes.
        var updated = new Student
        {
            Number = number,
            FirstName = string.IsNullOrWhiteSpace(firstName) ? existing.FirstName : NameFormatter.ToTitleCase(firstName),
            LastName = string.IsNullOrWhiteSpace(lastName) ? existing.LastName : NameFormatter.ToTitleCase(lastName),
            Section = string.IsNullOrWhiteSpace(section) ? existing.Section : NameFormatter.ToSection(section),
        };

        var error = Validate(updated);
        if (error is not null)
        {
            return OperationResult.Fail(error);
        }

        existing.FirstName = updated.FirstName;
        existing.LastName = updated.LastName;
        existing.Section = updated.Section;
        Persist();
        return OperationResult.Ok($"{existing.FullName} güncellendi.");
    }

    public OperationResult Delete(int number)
    {
        var existing = FindByNumber(number);
        if (existing is null)
        {
            return OperationResult.Fail($"{number} numaralı öğrenci bulunamadı.");
        }

        _students.Remove(existing);
        Persist();
        return OperationResult.Ok($"{existing.FullName} silindi.");
    }

    private static string? Validate(Student student)
    {
        if (student.Number <= 0)
        {
            return "Öğrenci numarası pozitif bir sayı olmalı.";
        }

        if (student.FirstName.Length == 0 || student.LastName.Length == 0)
        {
            return "Ad ve soyad boş bırakılamaz.";
        }

        if (student.FirstName.Any(char.IsDigit) || student.LastName.Any(char.IsDigit))
        {
            return "Ad ve soyad rakam içeremez.";
        }

        if (student.Section.Length is 0 or > MaxSectionLength || !student.Section.All(char.IsLetterOrDigit))
        {
            return $"Şube 1-{MaxSectionLength} karakterlik harf/rakam olmalı (ör. A, 5B).";
        }

        return null;
    }

    private void Persist() => _repository.Save(_students);

    private static Student Clone(Student s) => new()
    {
        Number = s.Number,
        FirstName = s.FirstName,
        LastName = s.LastName,
        Section = s.Section,
    };
}
