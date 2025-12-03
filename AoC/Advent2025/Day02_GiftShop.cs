namespace AoC.Advent2025;

public class Day02 : IPuzzle
{
    [Regex(@"(\d+)-(\d+)")]
    public record class Range(long Min, long Max)
    {
        public static bool IsRepeat(long value, int repeats, int digits)
        {
            if (digits % repeats != 0) return false;

            var mod = Util.Pow10(digits / repeats);
            long sub = value % mod;

            long v2 = sub;
            for (int i = 1; i < repeats; i++)
                v2 = (v2 * mod) + sub;

            return v2 == value;
        }

        public static bool IsInvalid(long value, QuestionPart part)
        {
            var digits = Util.Log10i(value) + 1;

            if (part.One)
                return IsRepeat(value, 2, digits);

            for (int i = 2; i <= digits; ++i)
                if (IsRepeat(value, i, digits)) return true;

            return false;
        }

        public long SumInvalids(QuestionPart part)
        {
            long sum = 0;
            for (long v = Min; v <= Max; v++)
                if (IsInvalid(v, part)) sum += v;
            return sum;
        }
    }

    private static long Solve(string input, QuestionPart part) 
        => Parser.Parse<Range>(input, ",").Sum(r => r.SumInvalids(part));

    public static long Part1(string input) => Solve(input, QuestionPart.Part1);

    public static long Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}