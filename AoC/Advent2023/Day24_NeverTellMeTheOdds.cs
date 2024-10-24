namespace AoC.Advent2023;

using DecVec3 = (decimal X, decimal Y, decimal Z);

public class Day24 : IPuzzle
{
    [Regex(@"(\d+), (\d+), (\d+) @ +(-?\d+), +(-?\d+), +(-?\d+)")]
    record class Entry(long X, long Y, long Z, long Dx, long Dy, long Dz)
    {
        public bool IsFuture((float X, float Y) p) => Math.Sign((float)(p.X - X)) == Math.Sign(Dx) && Math.Sign((float)(p.Y - Y)) == Math.Sign(Dy);

        public DecVec3 AtTime(int time) => (X + (Dx * time), Y + (Dy * time), Z + (Dz * time));

        public DecVec3 Pos => (X, Y, Z);
        public DecVec3 Vel => (Dx, Dy, Dz);
    }

    static (long X, long Y)[] GetLine2d(Entry entry) => [(entry.X, entry.Y), (entry.X + entry.Dx, entry.Y + entry.Dy)];

    static (float X, float Y)? GetIntersectionPoint((long X, long Y) p1, (long X, long Y) p2, (long X, long Y) p3, (long X, long Y) p4)
    {
        float denominator = ((p4.Y - p3.Y) * (p2.X - p1.X)) - ((p4.X - p3.X) * (p2.Y - p1.Y));
        float numerator1 = ((p4.X - p3.X) * (p1.Y - p3.Y)) - ((p4.Y - p3.Y) * (p1.X - p3.X));
        float numerator2 = ((p2.X - p1.X) * (p1.Y - p3.Y)) - ((p2.Y - p1.Y) * (p1.X - p3.X));

        if (denominator == 0f)
        {
            return numerator1 == 0f && numerator2 == 0f ? (p1.X, p1.Y) : null;
        }

        float r = numerator1 / denominator;

        return ((float)(p1.X + (r * (p2.X - p1.X))), (float)(p1.Y + (r * (p2.Y - p1.Y))));
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
                        if (intersect.Value.X >= minTest && intersect.Value.X <= maxTest && intersect.Value.Y >= minTest && intersect.Value.Y <= maxTest)
                        {
                            count++;
                        }
                    }
                }
            }
        }

        return count;
    }

    static decimal CrossX(DecVec3 a, DecVec3 b) => (a.Y * b.Z) - (a.Z * b.Y);
    static decimal CrossY(DecVec3 a, DecVec3 b) => (a.Z * b.X) - (a.X * b.Z);
    static decimal CrossZ(DecVec3 a, DecVec3 b) => (a.X * b.Y) - (a.Y * b.X);

    public static decimal[,] InvertMatrix(decimal[,] input)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (input.GetLength(0) != input.GetLength(1))
        {
            throw new ArgumentException("Input matrix must be square.");
        }

        int n = input.GetLength(0);
        decimal[,] result = new decimal[n, n];
        decimal[,] augmented = new decimal[n, 2 * n];

        // Augment the input matrix with an identity matrix
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                augmented[i, j] = input[i, j];
            }
            augmented[i, i + n] = 1;
        }

        // Perform row operations to transform the left half of the augmented matrix into the identity matrix
        for (int i = 0; i < n; i++)
        {
            decimal pivot = augmented[i, i];
            if (pivot == 0)
            {
                throw new ArgumentException("Input matrix is singular.");
            }

            for (int j = 0; j < 2 * n; j++)
            {
                augmented[i, j] /= pivot;
            }

            for (int j = 0; j < n; j++)
            {
                if (j != i)
                {
                    decimal factor = augmented[j, i];
                    for (int k = 0; k < 2 * n; k++)
                    {
                        augmented[j, k] -= factor * augmented[i, k];
                    }
                }
            }
        }

        // Extract the inverted matrix from the right half of the augmented matrix
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result[i, j] = augmented[i, j + n];
            }
        }

        return result;
    }

    public static decimal[] MultiplyMatrixAndVector(decimal[,] matrix, decimal[] vector)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        ArgumentNullException.ThrowIfNull(vector);

        if (matrix.GetLength(1) != vector.Length)
        {
            throw new ArgumentException("Matrix column count must match vector length.");
        }

        int rowCount = matrix.GetLength(0);
        int colCount = matrix.GetLength(1);
        decimal[] result = new decimal[rowCount];

        for (int i = 0; i < rowCount; i++)
        {
            decimal sum = 0;
            for (int j = 0; j < colCount; j++)
            {
                sum += matrix[i, j] * vector[j];
            }
            result[i] = sum;
        }

        return result;
    }

    static DecVec3 Subtract(DecVec3 a, DecVec3 b) => (a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    static decimal SolvePt2(Entry[] entries)
    {
        var a = entries[0];
        var b = entries[1];
        var c = entries[2];

        var min = a.Pos;

        var a_pos = Subtract(a.Pos, min);
        var b_pos = Subtract(b.Pos, min);
        var c_pos = Subtract(c.Pos, min);

        var ab_pos = Subtract(a_pos, b_pos);
        var ab_vel = Subtract(a.Vel, b.Vel);

        var ac_pos = Subtract(a_pos, c_pos);
        var ac_vel = Subtract(a.Vel, c.Vel);

        decimal[,] mat = {
                {ab_vel.Y, -ab_vel.X,  0,   -ab_pos.Y,  ab_pos.X,  0},
                {ac_vel.Y, -ac_vel.X,  0,   -ac_pos.Y,  ac_pos.X,  0},
                {-ab_vel.Z, 0, ab_vel.X,     ab_pos.Z,  0, -ab_pos.X},
                {-ac_vel.Z, 0, ac_vel.X,     ac_pos.Z,  0, -ac_pos.X},
                {0, ab_vel.Z, -ab_vel.Y,     0, -ab_pos.Z,  ab_pos.Y},
                {0, ac_vel.Z, -ac_vel.Y,     0, -ac_pos.Z,  ac_pos.Y }
        };

        decimal[] vec = [
            CrossZ(b.Vel, b_pos) - CrossZ(a.Vel, a_pos),
            CrossZ(c.Vel, c_pos) - CrossZ(a.Vel, a_pos),
            CrossY(b.Vel, b_pos) - CrossY(a.Vel, a_pos),
            CrossY(c.Vel, c_pos) - CrossY(a.Vel, a_pos),
            CrossX(b.Vel, b_pos) - CrossX(a.Vel, a_pos),
            CrossX(c.Vel, c_pos) - CrossX(a.Vel, a_pos)
        ];

        var rock = MultiplyMatrixAndVector(InvertMatrix(mat), vec);
        return Math.Round(rock[0] + rock[1] + rock[2] + min.X + min.Y + min.Z);
    }

    public static int Part1(string input) => CheckTestArea(input, 200000000000000, 400000000000000);

    public static decimal Part2(string input)
    {
        var data = Util.RegexParse<Entry>(input).ToArray();
        return SolvePt2(data);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
