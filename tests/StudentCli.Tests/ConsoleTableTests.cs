using StudentCli.Cli;

namespace StudentCli.Tests;

public class ConsoleTableTests
{
    [Fact]
    public void Renders_aligned_columns_with_borders()
    {
        var lines = new ConsoleTable("No", "Ad")
            .AlignRight(0)
            .AddRow(7, "Ayşe")
            .AddRow(108, "Can")
            .RenderLines();

        Assert.Equal(
        [
            "┌─────┬──────┐",
            "│ No  │ Ad   │",
            "├─────┼──────┤",
            "│   7 │ Ayşe │",
            "│ 108 │ Can  │",
            "└─────┴──────┘",
        ], lines);
    }

    [Fact]
    public void AddRow_rejects_wrong_cell_count() =>
        Assert.Throws<ArgumentException>(() => new ConsoleTable("A", "B").AddRow("only one"));
}
