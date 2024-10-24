namespace AoC.Advent2023;
public partial class Day01 : IPuzzle
{

    public partial class Value(string raw)
    {
        private IEnumerable<int> SplitDigitsAndWords(bool part2) =>
            SplitterRegex().Split(raw).WithoutNullOrWhiteSpace().Select(entry =>
            int.TryParse(entry, out var v) ? v : (part2 ? Parse(entry) : int.MaxValue)).Where(v => v != int.MaxValue);

        [GeneratedRegex(@"(\d|one|two|three|four|five|six|seven|eight|nine)")] private static partial Regex SplitterRegex();

        private int Get(bool part2)
        {
            var digits = SplitDigitsAndWords(part2);
            return (digits.First() * 10) + digits.Last();
        }

        static int Parse(string word) => word switch
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
            _ => int.MaxValue,
        };

        public int ValPt1 => Get(false);
        public int ValPt2 => Get(true);
    }

    public static int Part1(string input)
    {
        var data = Util.Parse<Value>(input);

        return data.Sum(v => v.ValPt1);
    }

    public static int Part2(string input)
    {
        var data = Util.Parse<Value>(input);

        return data.Sum(v => v.ValPt2);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
