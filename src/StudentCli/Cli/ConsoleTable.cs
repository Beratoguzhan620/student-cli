namespace StudentCli.Cli;

/// <summary>
/// Minimal box-drawn table renderer. Kept dependency-free so the app runs anywhere the SDK does.
/// </summary>
public sealed class ConsoleTable(params string[] headers)
{
    private readonly List<string[]> _rows = [];
    private readonly HashSet<int> _rightAligned = [];

    public ConsoleTable AlignRight(int columnIndex)
    {
        _rightAligned.Add(columnIndex);
        return this;
    }

    public ConsoleTable AddRow(params object[] cells)
    {
        if (cells.Length != headers.Length)
        {
            throw new ArgumentException($"Expected {headers.Length} cells but got {cells.Length}.", nameof(cells));
        }

        _rows.Add(cells.Select(c => c?.ToString() ?? string.Empty).ToArray());
        return this;
    }

    public IReadOnlyList<string> RenderLines()
    {
        var widths = headers
            .Select((h, i) => Math.Max(h.Length, _rows.Count == 0 ? 0 : _rows.Max(r => r[i].Length)))
            .ToArray();

        var lines = new List<string>
        {
            Border('┌', '┬', '┐', widths),
            Row(headers, widths, alignHeaders: true),
            Border('├', '┼', '┤', widths),
        };
        lines.AddRange(_rows.Select(r => Row(r, widths, alignHeaders: false)));
        lines.Add(Border('└', '┴', '┘', widths));
        return lines;
    }

    public void Write(ConsoleColor headerColor = ConsoleColor.Cyan, ConsoleColor borderColor = ConsoleColor.DarkGray)
    {
        var lines = RenderLines();
        for (var i = 0; i < lines.Count; i++)
        {
            var isBorder = i is 0 or 2 || i == lines.Count - 1;
            Console.ForegroundColor = isBorder ? borderColor : i == 1 ? headerColor : ConsoleColor.Gray;
            Console.WriteLine(lines[i]);
        }

        Console.ResetColor();
    }

    private static string Border(char left, char middle, char right, int[] widths) =>
        left + string.Join(middle, widths.Select(w => new string('─', w + 2))) + right;

    private string Row(string[] cells, int[] widths, bool alignHeaders)
    {
        var parts = cells.Select((c, i) =>
            !alignHeaders && _rightAligned.Contains(i) ? c.PadLeft(widths[i]) : c.PadRight(widths[i]));
        return "│ " + string.Join(" │ ", parts) + " │";
    }
}
