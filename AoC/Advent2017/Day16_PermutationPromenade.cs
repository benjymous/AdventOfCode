namespace AoC.Advent2017;
public class Day16 : IPuzzle
{
    class InstructionFactory
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
            var i1 = Array.IndexOf(order, c1);
            var i2 = Array.IndexOf(order, c2);
            order[i1] = c2;
            order[i2] = c1;
            return order;
        };
    }

    private static Func<char[], char[]>[] ParseRules(string input) => Util.RegexFactory<Func<char[], char[]>, InstructionFactory>(input, null, ",").ToArray();

    public static string DoDance(string input, string players) => DoDance(ParseRules(input), [.. players]).AsString();

    public static char[] DoDance(IEnumerable<Func<char[], char[]>> instructions, char[] order)
    {
        int orderCount = order.Length;

        foreach (var instr in instructions)
        {
            order = instr(order);
        }

        return order;
    }

    public static string Part1(string input) => DoDance(input, "abcdefghijklmnop");

    public static string Part2(string input)
    {
        long expectedRounds = 1000000000;

        var instructions = ParseRules(input);

        var seen = new Dictionary<string, long>();
        long round = 0;
        var order = "abcdefghijklmnop".ToArray();
        while (!seen.ContainsKey(order.AsString()))
        {
            seen[order.AsString()] = round++;
            order = DoDance(instructions, order);
        }
        long remainder = expectedRounds % round;
        return seen.Where(kvp => kvp.Value == remainder).First().Key;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}