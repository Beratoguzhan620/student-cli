using StudentCli.Text;

namespace StudentCli.Tests;

public class NameFormatterTests
{
    [Theory]
    [InlineData("ahmet", "Ahmet")]
    [InlineData("  aYŞE   nur ", "Ayşe Nur")]
    [InlineData("irem", "İrem")]
    [InlineData("IŞIK", "Işık")]
    [InlineData("ç", "Ç")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void ToTitleCase_uses_turkish_casing(string? input, string expected) =>
        Assert.Equal(expected, NameFormatter.ToTitleCase(input));

    [Theory]
    [InlineData(" 5b ", "5B")]
    [InlineData("i", "İ")]
    [InlineData(null, "")]
    public void ToSection_trims_and_uppercases(string? input, string expected) =>
        Assert.Equal(expected, NameFormatter.ToSection(input));
}
