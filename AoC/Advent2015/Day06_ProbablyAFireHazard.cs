namespace AoC.Advent2015;
public class Day06 : IPuzzle
{
    public enum Mode
    {
        turn_on,
        turn_off,
        toggle
    }

    [method: Regex(@"(toggle|turn on|turn off) (\d+,\d+) through (\d+,\d+)")]
    public readonly struct Instruction(Mode mode, ManhattanVector2 a, ManhattanVector2 b)
    {
        readonly (int x, int y) bl = a, tr = b;

        void Apply(ref bool val) => val = mode switch
        {
            Mode.turn_on => true,
            Mode.turn_off => false,
            Mode.toggle => !val,
            _ => throw new Exception("Unexpected light mode"),
        };

        void Apply(ref int val) => val = mode switch
        {
            Mode.turn_on => val + 1,
            Mode.turn_off => Math.Max(0, val - 1),
            Mode.toggle => val + 2,
            _ => throw new Exception("Unexpected light mode"),
        };

        public void Apply(bool[,] grid)
        {
            for (var y = bl.y; y <= tr.y; ++y)
                for (var x = bl.x; x <= tr.x; ++x)
                    Apply(ref grid[x, y]);
        }

        public void Apply(int[,] grid)
        {
            for (var y = bl.y; y <= tr.y; ++y)
                for (var x = bl.x; x <= tr.x; ++x)
                    Apply(ref grid[x, y]);
        }
    }

    public static int Part1(Parser.AutoArray<Instruction> input)
    {
        bool[,] grid = new bool[1000, 1000];

        input.ForEach(i => i.Apply(grid));
        return grid.Values().Count(i => i);
    }

    public static int Part2(Parser.AutoArray<Instruction> input)
    {
        int[,] grid = new int[1000, 1000];

        input.ForEach(i => i.Apply(grid));
        return grid.Values().Sum();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}