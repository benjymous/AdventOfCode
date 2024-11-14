namespace AoC.Advent2023;
public class Day18 : IPuzzle
{
    static readonly Direction2[] Directions = [Direction2.Right, Direction2.Down, Direction2.Left, Direction2.Up];

    public record class Instr(Direction2 Dir, int Distance);

    public record class InstructionV1 : Instr
    {
        [Regex(@"(.) (\d+) \(#......\)")] public InstructionV1(Direction2 dir, int distance) : base(dir, distance) { }
    }

    public record class InstructionV2 : Instr
    {
        [Regex(@". \d+ \(#(.....)(.)\)")] public InstructionV2([Base(16)] int distance, int dirIdx) : base(Directions[dirIdx], distance) { }
    }

    static long Solve(IEnumerable<Instr> instructions) => PicksTheorem(GetPath((0, 0), instructions).ToList());

    static IEnumerable<(int x, int y)> GetPath((int x, int y) pos, IEnumerable<Instr> instructions) => instructions.Select(instr => pos = pos.OffsetBy(instr.Dir, instr.Distance));

    static long PicksTheorem(List<(int x, int y)> points) // Area of integer polygon
    {
        long area = 0;
        int boundaryPoints = 0, interiorPoints = 0;

        for (int i = 0; i < points.Count; ++i)
        {
            var ((x1, y1), (x2, y2)) = (points[i], points[(i + 1) % points.Count]);

            area += ((long)x1 * y2) - ((long)x2 * y1);

            if (x1 == x2)
            {
                boundaryPoints += Math.Abs(y2 - y1);
            }
            else if (y1 == y2)
            {
                boundaryPoints += Math.Abs(x2 - x1);
            }
            else
            {
                interiorPoints += Util.GCD(Math.Abs(x2 - x1), Math.Abs(y2 - y1));
            }
        }

        return Math.Abs(area / 2) - (interiorPoints / 2) + (boundaryPoints / 2) + 1;
    }

    public static long Part1(Util.AutoParse<InstructionV1> input) => Solve(input);

    public static long Part2(Util.AutoParse<InstructionV2> input) => Solve(input);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}