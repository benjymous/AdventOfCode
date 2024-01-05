namespace AoC.Advent2023;
public class Day24 : IPuzzle
{
    [Regex(@"(\d+), (\d+), (\d+) @ +(-?\d+), +(-?\d+), +(-?\d+)")]
    record class Entry(long X, long Y, long Z, long Dx, long Dy, long Dz)
    {
        public bool IsFuture((float x, float y) p) => Math.Sign((float)(p.x - X)) == Math.Sign(Dx) && Math.Sign((float)(p.y - Y)) == Math.Sign(Dy);
    }

    static (long x, long y)[] GetLine2d(Entry entry) => [(entry.X, entry.Y), (entry.X + entry.Dx, entry.Y + entry.Dy)];

    static LineSegment GetLine3d(Entry entry)
    {
        const int lookahead = 1;
        return new LineSegment
        {
            Start = new Vector3(entry.X, entry.Y, entry.Z),
            End = new Vector3(entry.X + (entry.Dx * lookahead), entry.Y + (entry.Dy * lookahead), entry.Z + (entry.Dz * lookahead))
        };
    }

    static (float x, float y)? GetIntersectionPoint((long x, long y) p1, (long x, long y) p2, (long x, long y) p3, (long x, long y) p4)
    {
        float denominator = ((p4.y - p3.y) * (p2.x - p1.x)) - ((p4.x - p3.x) * (p2.y - p1.y));
        float numerator1 = ((p4.x - p3.x) * (p1.y - p3.y)) - ((p4.y - p3.y) * (p1.x - p3.x));
        float numerator2 = ((p2.x - p1.x) * (p1.y - p3.y)) - ((p2.y - p1.y) * (p1.x - p3.x));

        if (denominator == 0f)
        {
            return numerator1 == 0f && numerator2 == 0f ? (p1.x, p1.y) : null;
        }

        float r = numerator1 / denominator;

        return ((float)(p1.x + (r * (p2.x - p1.x))), (float)(p1.y + (r * (p2.y - p1.y))));
    }

    public static int CheckTestArea(string input, long minTest, long maxTest)
    {
        var data = Util.RegexParse<Entry>(input).ToList();
        var lines = data.Select(GetLine2d).ToList();

        int count = 0;
        for (int i1 = 0; i1 < lines.Count; ++i1)
        {
            for (int i2 = i1 + 1; i2 < lines.Count; ++i2)
            {
                var intersect = GetIntersectionPoint(lines[i1][0], lines[i1][1], lines[i2][0], lines[i2][1]);
                if (intersect != null)
                {
                    if (data[i1].IsFuture(intersect.Value) && data[i2].IsFuture(intersect.Value))
                    {
                        if (intersect.Value.x >= minTest && intersect.Value.x <= maxTest && intersect.Value.y >= minTest && intersect.Value.y <= maxTest)
                        {
                            count++;
                        }
                    }
                }
            }
        }

        return count;
    }

    public class LineSegment
    {
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
    }

    public static Vector3 FindIntersection(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2)
    {
        Vector3 p1p2 = p2 - p1;
        Vector3 q1q2 = q2 - q1;
        Vector3 p1q1 = q1 - p1;
        Vector3 n = Vector3.Cross(p1p2, q1q2);
        float t = Vector3.Dot(Vector3.Cross(p1q1, q1q2), n) / n.LengthSquared();
        return p1 + (t * p1p2);
    }

    public static LineSegment FindIntersectionOfLineSegments(LineSegment[] lineSegments)
    {
        Vector3[] intersections = new Vector3[lineSegments.Length - 1];
        for (int i = 0; i < lineSegments.Length - 1; i++)
        {
            intersections[i] = FindIntersection(lineSegments[i].Start, lineSegments[i].End, lineSegments[i + 1].Start, lineSegments[i + 1].End);
        }
        return new LineSegment { Start = intersections[0], End = intersections[intersections.Length - 1] };
    }

    public static int Part1(string input)
    {
        return CheckTestArea(input, 200000000000000, 400000000000000);
    }

    public static int Part2(string input)
    {
        var data = Util.RegexParse<Entry>(input).ToList();
        var lines = data.Select(GetLine3d).ToArray();
        var blah = FindIntersectionOfLineSegments(lines);
        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
