using StudentCli.Models;

namespace StudentCli.Data;

public interface IStudentRepository
{
    /// <summary>True when the underlying store has been created before.</summary>
    bool Exists { get; }

    IReadOnlyList<Student> Load();

    void Save(IEnumerable<Student> students);
}
