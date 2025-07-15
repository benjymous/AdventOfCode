namespace AoC.Advent2017;
public class Day16 : IPuzzle
{
    private class InstructionFactory
    {
        [Regex(@"s(\d+)")]
        public static Func<char[], char[]> Spin(int count) => order => [.. order[^count..], .. order[..(order.Length - count)]];

        [Regex(@"x(\d+)\/(\d+)")]
        public static Func<char[], char[]> Exchange(int s1, int s2) => order =>
        {
            (order[s1], order[s2]) = (order[s2], order[s1]);
            return order;
        };

        [Regex(@"p(.)\/(.)")]
        public static Func<char[], char[]> Partner(char c1, char c2) => order =>
        {
            (order[Array.IndexOf(order, c1)], order[Array.IndexOf(order, c2)]) = (c2, c1);
            return order;
        };
    }

    private static Func<char[], char[]>[] ParseRules(string input) => [.. Parser.Factory<Func<char[], char[]>, InstructionFactory>(input, null, ",")];

    public static string DoDance(string input, string players) => DoDance(ParseRules(input), [.. players]).AsString();

    public static char[] DoDance(IEnumerable<Func<char[], char[]>> instructions, char[] order) => instructions.ApplyAll(order);

    public static string Part1(string input) => DoDance(input, "abcdefghijklmnop");

    public static string Part2(string input)
    {
        int expectedRounds = 1000000000;

        var instructions = ParseRules(input);

        var order = "abcdefghijklmnop".ToArray();

        return Util.FindCycle(expectedRounds, order, s => s.AsString(), s => DoDance(instructions, s)).AsString();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}