namespace AoC.Advent2022;
public class Day23 : IPuzzle
{
    private static readonly PackedPos32[] Neighbours = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)];
    private static bool HasNeighbour(HashSet<PackedPos32> map, PackedPos32 pos) => Neighbours.Any(offset => map.Contains(pos + offset));

    private static bool DirectionFree(HashSet<PackedPos32> map, PackedPos32 pos, int direction, out int newPos)
    {
        newPos = default;
        for (int i = 0; i < 3; ++i)
        {
            var move = pos + CheckDirs[direction, i];
            if (i == 0) newPos = move;
            if (map.Contains(move)) return false;
        }
        return true;
    }

    private static readonly PackedPos32[,] CheckDirs = new PackedPos32[,]
    {
        { (0, -1),(-1, -1),(1, -1) }, // check North
        { (0, 1),(-1, 1),(1, 1) }, // check South
        { (-1, 0),(-1, -1),(-1, 1) }, // check West
        { (1, 0),(1, -1),(1, 1) } // check East
    };

    private static int RunSimulation(string input, int maxSteps)
    {
        var positions = Util.ParseSparseMatrix<PackedPos32, bool>(input).Keys.Select(k => k + (100, 100)).ToHashSet();
        UniqueMap<PackedPos32, PackedPos32> potentialMoves = new(positions.Count);

        for (int moveIndex = 0; moveIndex < maxSteps; ++moveIndex)
        {
            foreach (var currentPos in positions)
            {
                if (!HasNeighbour(positions, currentPos)) continue;
                for (int i = 0; i < 4; i++)
                {
                    if (DirectionFree(positions, currentPos, (moveIndex + i) % 4, out var newPos))
                    {
                        potentialMoves.UniqueAdd(newPos, currentPos);
                        break;
                    }
                }
            }

            if (!potentialMoves.Any()) return moveIndex + 1;

            positions.ExceptWith(potentialMoves.Values);
            positions.UnionWith(potentialMoves.Keys);

            potentialMoves.Reset();
        }

        return CountEmpty(positions.Select(i => (x: i.X, y: i.Y)));
    }

    private static int CountEmpty(IEnumerable<(int x, int y)> positions) => ((positions.Max(v => v.x) - positions.Min(v => v.x) + 1) * (positions.Max(v => v.y) - positions.Min(v => v.y) + 1)) - positions.Count();

    public static int Part1(string input) => RunSimulation(input, 10);

    public static int Part2(string input) => RunSimulation(input, int.MaxValue);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}