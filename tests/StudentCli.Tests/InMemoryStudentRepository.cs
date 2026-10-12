using StudentCli.Data;
using StudentCli.Models;

namespace StudentCli.Tests;

internal sealed class InMemoryStudentRepository(IEnumerable<Student>? initial = null) : IStudentRepository
{
    private List<Student>? _stored = initial?.ToList();

    public int SaveCount { get; private set; }

    public bool Exists => _stored is not null;

    public IReadOnlyList<Student> Load() => _stored ?? [];

    public void Save(IEnumerable<Student> students)
    {
        _stored = students.Select(s => new Student
        {
            Number = s.Number,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Section = s.Section,
        }).ToList();
        SaveCount++;
    }
}
