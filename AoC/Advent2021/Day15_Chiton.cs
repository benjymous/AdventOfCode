namespace AoC.Advent2021;
public class Day15 : IPuzzle
{
    class Map : IMap<PackedPos32>
    {
        public Map(string input, QuestionPart part)
        {
            var raw = Util.ParseMatrix<int>(input);
            Data = raw.Entries().ToDictionary(kvp => (PackedPos32)kvp.key, kvp => kvp.value);

            if (part.One())
            {
                (RealX, RealY) = (MaxX, MaxY) = (raw.Width() - 1, raw.Height() - 1);
            }
            else
            {
                (RealX, RealY) = (raw.Width(), raw.Height());
                (MaxX, MaxY) = ((RealX * 5) - 1, (RealY * 5) - 1);
            }

            for (int x = -1; x <= MaxX + 1; ++x) Data[(x, -1)] = Data[(x, MaxY + 1)] = 9999;
            for (int y = -1; y <= MaxY + 1; ++y) Data[(-1, y)] = Data[(MaxX + 1, y)] = 9999;
        }

        readonly int MaxX, MaxY, RealX, RealY;
        readonly Dictionary<PackedPos32, int> Data;

        public IEnumerable<PackedPos32> GetNeighbours(PackedPos32 location)
        {
            yield return location + (1, 0);
            yield return location + (0, 1);
            yield return location - (1, 0);
            yield return location - (0, 1);
        }

        public int GScore(PackedPos32 pos) => Data.GetOrCalculate(pos, pos =>
        {
            var (x, y) = (pos & 0xffff, pos >> 16);
            return ((Data[(x % RealX) + ((y % RealY) << 16)] + ((x / RealX) + (y / RealY)) - 1) % 9) + 1;
        });

        public static int Solve(string input, QuestionPart part)
        {
            var map = new Map(input, part);
            return map.FindPath(0, (map.MaxX, map.MaxY)).Sum(map.GScore);
        }
    }

    public static int Part1(string input) => Map.Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Map.Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}