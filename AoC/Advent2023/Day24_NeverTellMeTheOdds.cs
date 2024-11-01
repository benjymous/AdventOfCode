namespace AoC.Advent2023;

using DecVec3 = (decimal X, decimal Y, decimal Z);

public class Day24 : IPuzzle
{
    [Regex(@"(.+) @ (.+)")]
    record class Entry([Regex(@"(.+), (.+), (.+)")] (long X, long Y, long Z) Pos, [Regex(@"(.+), (.+), (.+)")] (long X, long Y, long Z) Vel)
    {
        public bool IsFuture((float X, float Y) p) => Math.Sign((float)(p.X - Pos.X)) == Math.Sign(Vel.X) && Math.Sign((float)(p.Y - Pos.Y)) == Math.Sign(Vel.Y);
    }

    static (float X, float Y)? GetIntersectionPoint((long X, long Y)[] l1, (long X, long Y)[] l2)
    {
        float denominator = ((l2[1].Y - l2[0].Y) * (l1[1].X - l1[0].X)) - ((l2[1].X - l2[0].X) * (l1[1].Y - l1[0].Y));
        float numerator1 = ((l2[1].X - l2[0].X) * (l1[0].Y - l2[0].Y)) - ((l2[1].Y - l2[0].Y) * (l1[0].X - l2[0].X));
        float numerator2 = ((l1[1].X - l1[0].X) * (l1[0].Y - l2[0].Y)) - ((l1[1].Y - l1[0].Y) * (l1[0].X - l2[0].X));

        if (denominator == 0f)
        {
            return numerator1 == 0f && numerator2 == 0f ? (l1[0].X, l1[0].Y) : null;
        }

        float r = numerator1 / denominator;

        return ((float)(l1[0].X + (r * (l1[1].X - l1[0].X))), (float)(l1[0].Y + (r * (l1[1].Y - l1[0].Y))));
    }

    public static int CheckTestArea(string input, long minTest, long maxTest)
    {
        var data = Util.RegexParse<Entry>(input).ToArray();
        var lines = data.Select(entry => ((long, long)[])[(entry.Pos.X, entry.Pos.Y), (entry.Pos.X + entry.Vel.X, entry.Pos.Y + entry.Vel.Y)]).ToArray();

        int count = 0;
        for (int i1 = 0; i1 < lines.Length; ++i1)
        {
            for (int i2 = i1 + 1; i2 < lines.Length; ++i2)
            {
                var intersect = GetIntersectionPoint(lines[i1], lines[i2]);
                if (intersect != null && data[i1].IsFuture(intersect.Value) && data[i2].IsFuture(intersect.Value) && intersect.Value.X >= minTest && intersect.Value.X <= maxTest && intersect.Value.Y >= minTest && intersect.Value.Y <= maxTest)
                {
                    count++;
                }
            }
        }

        return count;
    }

    static decimal CrossX(DecVec3 a, DecVec3 b) => (a.Y * b.Z) - (a.Z * b.Y);
    static decimal CrossY(DecVec3 a, DecVec3 b) => (a.Z * b.X) - (a.X * b.Z);
    static decimal CrossZ(DecVec3 a, DecVec3 b) => (a.X * b.Y) - (a.Y * b.X);
    static DecVec3 Subtract(DecVec3 a, DecVec3 b) => (a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static decimal[,] InvertMatrix(decimal[,] input)
    {
        ArgumentNullException.ThrowIfNull(input);
        if (input.GetLength(0) != input.GetLength(1)) throw new ArgumentException("Input matrix must be square.");

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
            if (pivot == 0) throw new ArgumentException("Input matrix is singular.");

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
            throw new ArgumentException("Matrix column count must match vector length.");

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

    static decimal SolvePt2((Entry a, Entry b, Entry c) input)
    {
        var min = input.a.Pos;

        DecVec3 a_pos = Subtract(input.a.Pos, min), b_pos = Subtract(input.b.Pos, min), c_pos = Subtract(input.c.Pos, min);
        DecVec3 ab_pos = Subtract(a_pos, b_pos), ab_vel = Subtract(input.a.Vel, input.b.Vel);
        DecVec3 ac_pos = Subtract(a_pos, c_pos), ac_vel = Subtract(input.a.Vel, input.c.Vel);

        decimal[,] mat = {
            {ab_vel.Y, -ab_vel.X,  0,   -ab_pos.Y,  ab_pos.X,  0},
            {ac_vel.Y, -ac_vel.X,  0,   -ac_pos.Y,  ac_pos.X,  0},
            {-ab_vel.Z, 0, ab_vel.X,     ab_pos.Z,  0, -ab_pos.X},
            {-ac_vel.Z, 0, ac_vel.X,     ac_pos.Z,  0, -ac_pos.X},
            {0, ab_vel.Z, -ab_vel.Y,     0, -ab_pos.Z,  ab_pos.Y},
            {0, ac_vel.Z, -ac_vel.Y,     0, -ac_pos.Z,  ac_pos.Y}
        };

        decimal[] vec = [
            CrossZ(input.b.Vel, b_pos) - CrossZ(input.a.Vel, a_pos),
            CrossZ(input.c.Vel, c_pos) - CrossZ(input.a.Vel, a_pos),
            CrossY(input.b.Vel, b_pos) - CrossY(input.a.Vel, a_pos),
            CrossY(input.c.Vel, c_pos) - CrossY(input.a.Vel, a_pos),
            CrossX(input.b.Vel, b_pos) - CrossX(input.a.Vel, a_pos),
            CrossX(input.c.Vel, c_pos) - CrossX(input.a.Vel, a_pos)
        ];

        var rock = MultiplyMatrixAndVector(InvertMatrix(mat), vec);
        return Math.Round(rock.Take(3).Sum() + min.X + min.Y + min.Z);
    }

    public static int Part1(string input) => CheckTestArea(input, 200000000000000, 400000000000000);

    public static decimal Part2(string input) => SolvePt2(Util.RegexParse<Entry>(input).Decompose3());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}