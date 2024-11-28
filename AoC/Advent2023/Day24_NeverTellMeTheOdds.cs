namespace AoC.Advent2023;
using static Utils.Vectors.Tuple;

using DecVec3 = (decimal X, decimal Y, decimal Z);

public class Day24 : IPuzzle
{
    [Regex(@"(.+) @ (.+)")]
    public record class Entry([Regex(@"(.+), (.+), (.+)")] (long X, long Y, long Z) Pos, [Regex(@"(.+), (.+), (.+)")] (long X, long Y, long Z) Vel)
    {
        public bool IsFuture((double X, double Y) p) => Math.Sign((float)(p.X - Pos.X)) == Math.Sign(Vel.X) && Math.Sign((float)(p.Y - Pos.Y)) == Math.Sign(Vel.Y);
    }

    public static bool IsInside((double X, double Y) position, long minTest, long maxTest) => position.X >= minTest && position.X <= maxTest && position.Y >= minTest && position.Y <= maxTest;

    public static int CheckTestArea(Parser.AutoArray<Entry> data, long minTest, long maxTest)
    {
        return data.Select(entry => (Entry: entry,
                                     Value: ((long, long)[])[(entry.Pos.X, entry.Pos.Y), (entry.Pos.X + entry.Vel.X, entry.Pos.Y + entry.Vel.Y)]))
                   .UniquePairs()
                   .Select(pair => (Pair: pair, Intersect: Util.GetIntersectionPoint(pair.Item1.Value, pair.Item2.Value)))
                   .Count(entry => entry.Intersect != null
                              && IsInside(entry.Intersect.Value, minTest, maxTest)
                              && entry.Pair.Item1.Entry.IsFuture(entry.Intersect.Value)
                              && entry.Pair.Item2.Entry.IsFuture(entry.Intersect.Value));
    }

    static decimal SolvePt2((Entry a, Entry b, Entry c) input)
    {
        var min = input.a.Pos;

        DecVec3 a_pos = Subtract(input.a.Pos, min), b_pos = Subtract(input.b.Pos, min), c_pos = Subtract(input.c.Pos, min);
        DecVec3 ab_pos = Subtract(a_pos, b_pos), ab_vel = Subtract(input.a.Vel, input.b.Vel);
        DecVec3 ac_pos = Subtract(a_pos, c_pos), ac_vel = Subtract(input.a.Vel, input.c.Vel);

        var mat = InvertMatrix(new decimal[,] {
            { ab_vel.Y, -ab_vel.X, 0, -ab_pos.Y, ab_pos.X, 0 },
            { ac_vel.Y, -ac_vel.X, 0, -ac_pos.Y, ac_pos.X, 0 },
            { -ab_vel.Z, 0, ab_vel.X, ab_pos.Z, 0, -ab_pos.X },
            { -ac_vel.Z, 0, ac_vel.X, ac_pos.Z, 0, -ac_pos.X },
            { 0, ab_vel.Z, -ab_vel.Y, 0, -ab_pos.Z, ab_pos.Y },
            { 0, ac_vel.Z, -ac_vel.Y, 0, -ac_pos.Z, ac_pos.Y }
        });

        decimal[] vec = [
            CrossZ(input.b.Vel, b_pos) - CrossZ(input.a.Vel, a_pos),
            CrossZ(input.c.Vel, c_pos) - CrossZ(input.a.Vel, a_pos),
            CrossY(input.b.Vel, b_pos) - CrossY(input.a.Vel, a_pos),
            CrossY(input.c.Vel, c_pos) - CrossY(input.a.Vel, a_pos),
            CrossX(input.b.Vel, b_pos) - CrossX(input.a.Vel, a_pos),
            CrossX(input.c.Vel, c_pos) - CrossX(input.a.Vel, a_pos)
        ];

        var rock = MultiplyMatrixAndVector(mat, vec);
        return Math.Round(rock.Take(3).Sum() + min.X + min.Y + min.Z);
    }

    public static int Part1(Parser.AutoArray<Entry> input) => CheckTestArea(input, 200000000000000, 400000000000000);

    public static decimal Part2(Parser.AutoArray<Entry> input) => SolvePt2(input.Decompose3());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}