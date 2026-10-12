using System.Globalization;

namespace StudentCli.Text;

public static class NameFormatter
{
    private static readonly CultureInfo Turkish = CultureInfo.GetCultureInfo("tr-TR");

    /// <summary>
    /// Trims the input, collapses repeated spaces and capitalises each word using Turkish
    /// casing rules, so "  aYŞE   nur " becomes "Ayşe Nur" and "irem" becomes "İrem".
    /// </summary>
    public static string ToTitleCase(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return string.Join(' ', words.Select(Capitalize));
    }

    public static string ToSection(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToUpper(Turkish);

    public static bool ContainsIgnoreCase(string source, string term) =>
        Turkish.CompareInfo.IndexOf(source, term, CompareOptions.IgnoreCase) >= 0;

    private static string Capitalize(string word) =>
        word.Length == 1
            ? word.ToUpper(Turkish)
            : word[..1].ToUpper(Turkish) + word[1..].ToLower(Turkish);
}
