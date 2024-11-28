namespace AoC.Advent2020;

public class Day05 : IPuzzle
{
    public class BinSearch(int min, int max)
    {
        int Min = min, Max = max;

        int Range => Max - Min + 1;

        public void Lower() => Max -= Math.Max(1, Range / 2);
        public void Upper() => Min += Math.Max(1, Range / 2);

        public static implicit operator int(BinSearch v) => v.Min;
    }
    public class BoardingPass
    {
        [Regex("(.+)")]
        public BoardingPass(string id)
        {
            BinSearch row = new(0, 127), col = new(0, 7);
            foreach (char c in id)
            {
                switch (c)
                {
                    case 'F':
                        row.Lower();
                        break;
                    case 'B':
                        row.Upper();
                        break;

                    case 'L':
                        col.Lower();
                        break;
                    case 'R':
                        col.Upper();
                        break;
                }
            }

            ID = (row * 8) + col;
        }

        public readonly int ID;
    }

    static HashSet<int> ParsePasses(string input) => Memoize(input, _ => Parser.Parse<BoardingPass>(input).Select(p => p.ID).ToHashSet());

    public static int Part1(string input) => ParsePasses(input).Max();

    public static int Part2(string input)
    {
        var ids = ParsePasses(input);
        return ids.Where(i => !ids.Contains(i + 1) && ids.Contains(i + 2)).Select(i => i + 1).Single();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}