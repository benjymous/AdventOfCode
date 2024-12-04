namespace AoC.Advent2021;
public class Day15 : IPuzzle
{
    class Map : IMap<PackedPos32>
    {
        public Map(string input, QuestionPart part)
        {
            Data = Util.ParseSparseMatrix<PackedPos32, int>(input);

            if (part.One())
            {
                (RealX, RealY) = (MaxX, MaxY) = (Data.Width, Data.Height);
            }
            else
            {
                (RealX, RealY) = (Data.Width + 1, Data.Height + 1);
                (MaxX, MaxY) = ((RealX * 5) - 1, (RealY * 5) - 1);
            }

            for (int x = -1; x <= MaxX + 1; ++x) Data[(x, -1)] = Data[(x, MaxY + 1)] = 9999;
            for (int y = -1; y <= MaxY + 1; ++y) Data[(-1, y)] = Data[(MaxX + 1, y)] = 9999;
        }

        readonly int MaxX, MaxY, RealX, RealY;
        readonly Util.SparseMatrix<PackedPos32, int> Data;

        public IEnumerable<PackedPos32> GetNeighbours(PackedPos32 location) =>
        [
            location + (1, 0),
            location + (0, 1),
            location - (1, 0),
            location - (0, 1)
        ];

        public int GScore(PackedPos32 pos) => Data.GetOrCalculate(pos, pos => ((Data[(pos.X % RealX) + ((pos.Y % RealY) << 16)] + ((pos.X / RealX) + (pos.Y / RealY)) - 1) % 9) + 1);

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