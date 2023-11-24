namespace AoC.Advent2020;
public class Day05 : IPuzzle
{
    public record class BinSearch
    {
        public BinSearch(int min, int max) => (Min, Max) = (min, max);

        public int Min, Max;

        int Range => Max - Min + 1;

        public void Lower() => Max -= Math.Max(1, Range / 2);
        public void Upper() => Min += Math.Max(1, Range / 2);
    }
    public class BoardingPass
    {
        public BoardingPass(string id)
        {
            var row = new BinSearch(0, 127);
            var col = new BinSearch(0, 7);
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

            Row = row.Min;
            Col = col.Min;
            ID = (Row * 8) + Col;
        }

        readonly int Row;
        readonly int Col;
        public readonly int ID;
    }

    public static int Part1(string input)
    {
        var passes = Util.Parse<BoardingPass>(input);
        return passes.Max(p => p.ID);
    }

    public static int Part2(string input)
    {
        var passes = Util.Parse<BoardingPass>(input);
        var ids = passes.Select(p => p.ID).Order().ToArray();

        for (int i = 0; i < ids.Length - 1; ++i)
        {
            if (ids[i + 1] - 2 == ids[i])
            {
                return ids[i] + 1;
            }
        }

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}