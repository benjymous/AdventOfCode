namespace AoC.Advent2023;
public partial class Day01 : IPuzzle
{
    [Regex("(.+)")]
    public partial class Value(string raw)
    {
        private IEnumerable<int> SplitDigitsAndWords(QuestionPart part) =>
            SplitterRegex().Split(raw).WithoutNullOrWhiteSpace().Select(entry =>
            int.TryParse(entry, out var v) ? v : (part.Two ? Parse(entry) : default)).Where(v => v != default);

        [GeneratedRegex(@"(\d|one|two|three|four|five|six|seven|eight|nine)")] private static partial Regex SplitterRegex();

        public int GetResult(QuestionPart part)
        {
            var digits = SplitDigitsAndWords(part);
            return (digits.First() * 10) + digits.Last();
        }

        private static int Parse(string word) => word switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => default,
        };
    }

    public static int Solve(string input, QuestionPart part) => Parser.Parse<Value>(input).Sum(v => v.GetResult(part));

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}