using StudentCli.Models;

namespace StudentCli.Data;

public static class SampleData
{
    public static IReadOnlyList<Student> Students =>
    [
        new() { Number = 3, FirstName = "Serkan", LastName = "Kılıçkap", Section = "A" },
        new() { Number = 13, FirstName = "Mahmut", LastName = "Aydın", Section = "B" },
        new() { Number = 42, FirstName = "Üzeyir", LastName = "Yaman", Section = "C" },
        new() { Number = 57, FirstName = "Elif", LastName = "Şahin", Section = "A" },
        new() { Number = 108, FirstName = "İrem", LastName = "Çelik", Section = "B" },
    ];
}
