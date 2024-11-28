namespace AoC.Advent2017;
public class Day08 : IPuzzle
{
    class Factory
    {
        public readonly Dictionary<string, int> RegLookup = [];

        [Regex(@"(.+) (inc|dec) (-?\d+) if (.+) (<|<=|==|!=|>=|>) (-?\d+)")]
        public Action<int[]> Instr(string regToChange, string action, int amount, string regToCheck, string oper, int checkVal)
            => (int[] regs) => regs[RegIndex(regToChange)] += Operator(oper, regs[RegIndex(regToCheck)], checkVal) ? (action == "inc" ? amount : -amount) : 0;

        private int RegIndex(string name) => RegLookup.GetOrCalculate(name, _ => RegLookup.Count);

        static bool Operator(string op, int lhs, int rhs) => op switch
        {
            "==" => lhs == rhs,
            "!=" => lhs != rhs,
            "<" => lhs < rhs,
            ">" => lhs > rhs,
            "<=" => lhs <= rhs,
            ">=" => lhs >= rhs,
            _ => throw new Exception("Unknown operator"),
        };
    }

    public static (int largestEnd, int largestRecord) Run(string input)
    {
        var values = new int[32];

        int runningMax = 0;

        foreach (var instr in Parser.Factory<Action<int[]>, Factory>(input, new Factory()))
        {
            instr(values);

            runningMax = Math.Max(runningMax, values.Max());
        }

        return (values.Max(), runningMax);
    }

    public static int Part1(string input) => Run(input).largestEnd;

    public static int Part2(string input) => Run(input).largestRecord;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}