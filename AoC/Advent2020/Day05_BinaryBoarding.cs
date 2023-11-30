namespace AoC.Advent2020;

public class Day05 : IPuzzle
{
    public class BinSearch(int min, int max)
    {
        public int Min = min, Max = max;

        int Range => Max - Min + 1;

        public void Lower() => Max -= Math.Max(1, Range / 2);
        public void Upper() => Min += Math.Max(1, Range / 2);
    }
    public class BoardingPass
    {
        public BoardingPass(string id)
        {
            var rowSearch = new BinSearch(0, 127);
            var colSearch = new BinSearch(0, 7);
            foreach (char c in id)
            {
                switch (c)
                {
                    case 'F':
                        rowSearch.Lower();
                        break;
                    case 'B':
                        rowSearch.Upper();
                        break;

                    case 'L':
                        colSearch.Lower();
                        break;
                    case 'R':
                        colSearch.Upper();
                        break;
                }
            }

            int row = rowSearch.Min;
            int col = colSearch.Min;
            ID = (row * 8) + col;
        }

        public readonly int ID;
    }

    static HashSet<int> ParsePasses(string input) => Memoize(input, _ => Util.Parse<BoardingPass>(input).Select(p => p.ID).ToHashSet());

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