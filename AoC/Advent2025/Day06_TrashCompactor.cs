namespace AoC.Advent2025;
public class Day06 : IPuzzle
{
    static long Operation(char op, long lhs, long rhs)
        => op == '+' ? lhs + rhs : lhs * rhs;

    public static long Part1(string input)
    {
        var rowsRaw = Util.Split(input).Reverse().ToArray();

        var ops = Util.Split(rowsRaw.First(), " ").Select(s => s[0]).ToArray();

        var numberLines = rowsRaw.Skip(1).Select(line => Util.ParseNumbers<long>(line, " "));

        var values = numberLines.First();

        foreach (var e in numberLines.Skip(1))
        {
            for (var i = 0; i < e.Length; i++)
            {
                values[i] = Operation(ops[i], values[i], e[i]);
            }
        }
       
        return values.Sum();
    }

    public static long Part2(string input)
    {
        var grid = Util.Split(input.Replace("\n", " \n"));

        long total = 0;
        char op = ' ';
        long current = 0;

        int cols = grid[0].Length;
        int rows = grid.Length;

        for (int i=0; i<cols; i++)
        {
            var str = "";

            for (int row = 0; row < rows; row++)
            {
                str += grid[row][i];
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                total += current;
            }
            else
            {
                var num = long.Parse(str[..^1]);
                var endChar = str[^1];

                if (endChar == ' ')
                {
                    current = Operation(op, current, num);
                }
                else
                {
                    current = num;
                    op = endChar;
                }
            }
        }

        return total;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}