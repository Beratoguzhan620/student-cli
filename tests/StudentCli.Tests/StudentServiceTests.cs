using StudentCli.Models;
using StudentCli.Services;

namespace StudentCli.Tests;

public class StudentServiceTests
{
    private static StudentService CreateService(params Student[] students) =>
        new(new InMemoryStudentRepository(students));

    private static Student Make(int number, string first, string last, string section) =>
        new() { Number = number, FirstName = first, LastName = last, Section = section };

    [Fact]
    public void Seeds_sample_data_only_when_store_does_not_exist()
    {
        var emptyStore = new InMemoryStudentRepository();
        var seeded = new StudentService(emptyStore, [Make(1, "Ali", "Can", "A")]);

        var existingStore = new InMemoryStudentRepository([]);
        var notSeeded = new StudentService(existingStore, [Make(1, "Ali", "Can", "A")]);

        Assert.Equal(1, seeded.Count);
        Assert.Equal(1, emptyStore.SaveCount);
        Assert.Equal(0, notSeeded.Count);
    }

    [Fact]
    public void Add_normalises_names_and_section_and_persists()
    {
        var repository = new InMemoryStudentRepository([]);
        var service = new StudentService(repository);

        var result = service.Add(7, "  aYŞE   nur ", "yılmaz", " 5b ");

        Assert.True(result.Success);
        var student = Assert.Single(repository.Load());
        Assert.Equal("Ayşe Nur", student.FirstName);
        Assert.Equal("Yılmaz", student.LastName);
        Assert.Equal("5B", student.Section);
    }

    [Fact]
    public void Add_rejects_duplicate_number()
    {
        var service = CreateService(Make(7, "Ali", "Can", "A"));

        var result = service.Add(7, "Veli", "Kaya", "B");

        Assert.False(result.Success);
        Assert.Equal(1, service.Count);
    }

    [Theory]
    [InlineData(0, "Ali", "Can", "A")]
    [InlineData(-3, "Ali", "Can", "A")]
    [InlineData(5, "", "Can", "A")]
    [InlineData(5, "Ali", "  ", "A")]
    [InlineData(5, "Ali2", "Can", "A")]
    [InlineData(5, "Ali", "Can", "")]
    [InlineData(5, "Ali", "Can", "ABCD")]
    [InlineData(5, "Ali", "Can", "A-1")]
    public void Add_rejects_invalid_input(int number, string first, string last, string section)
    {
        var service = CreateService();

        var result = service.Add(number, first, last, section);

        Assert.False(result.Success);
        Assert.Equal(0, service.Count);
    }

    [Fact]
    public void GetAll_orders_by_section_then_number()
    {
        var service = CreateService(
            Make(20, "C", "C", "B"),
            Make(5, "B", "B", "A"),
            Make(1, "A", "A", "B"));

        var numbers = service.GetAll().Select(s => s.Number);

        Assert.Equal([5, 1, 20], numbers);
    }

    [Theory]
    [InlineData("irem", 108)]
    [InlineData("İREM", 108)]
    [InlineData("çel", 108)]
    [InlineData("42", 42)]
    public void Search_matches_name_case_insensitively_and_number_exactly(string term, int expected)
    {
        var service = CreateService(
            Make(108, "İrem", "Çelik", "B"),
            Make(42, "Üzeyir", "Yaman", "C"));

        var result = Assert.Single(service.Search(term));

        Assert.Equal(expected, result.Number);
    }

    [Fact]
    public void Search_by_section_returns_every_student_in_it()
    {
        var service = CreateService(
            Make(1, "Ece", "Su", "5B"),
            Make(2, "Oğuz", "Er", "5B"),
            Make(3, "Can", "Ok", "6A"));

        var results = service.Search("5b");

        Assert.Equal(2, results.Count);
        Assert.All(results, s => Assert.Equal("5B", s.Section));
    }

    [Fact]
    public void Update_changes_only_provided_fields()
    {
        var service = CreateService(Make(42, "Üzeyir", "Yaman", "C"));

        var result = service.Update(42, firstName: null, lastName: "kaya", section: "  ");

        Assert.True(result.Success);
        var student = service.FindByNumber(42)!;
        Assert.Equal("Üzeyir", student.FirstName);
        Assert.Equal("Kaya", student.LastName);
        Assert.Equal("C", student.Section);
    }

    [Fact]
    public void Update_returns_failure_for_unknown_student()
    {
        var service = CreateService();

        Assert.False(service.Update(99, "Ali", "Can", "A").Success);
    }

    [Fact]
    public void Update_does_not_apply_invalid_values()
    {
        var service = CreateService(Make(1, "Ali", "Can", "A"));

        var result = service.Update(1, "Ali", "Can", "TOOLONG");

        Assert.False(result.Success);
        Assert.Equal("A", service.FindByNumber(1)!.Section);
    }

    [Fact]
    public void Delete_removes_student_and_persists()
    {
        var repository = new InMemoryStudentRepository([Make(1, "Ali", "Can", "A"), Make(2, "Ece", "Su", "B")]);
        var service = new StudentService(repository);

        var result = service.Delete(1);

        Assert.True(result.Success);
        Assert.Null(service.FindByNumber(1));
        Assert.Single(repository.Load());
    }

    [Fact]
    public void Delete_returns_failure_for_unknown_student()
    {
        var service = CreateService(Make(1, "Ali", "Can", "A"));

        Assert.False(service.Delete(2).Success);
        Assert.Equal(1, service.Count);
    }
}
