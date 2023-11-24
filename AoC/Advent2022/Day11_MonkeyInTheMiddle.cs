namespace AoC.Advent2022;
public class Day11 : IPuzzle
{
    public class Monkey
    {
        [Regex(@"Monkey (.+): Starting items: (.+) Operation: new = old (.) (.+) Test: divisible by (.+) If true: throw to monkey (.) If false: throw to monkey (.)")]
        public Monkey(int id, int[] items, char op, string opBy, int test, int ifTrue, int ifFalse)
        {
            Id = id;
            Items = [.. items];

            bool byOld = !int.TryParse(opBy, out int opByVal);

            Operation = op switch
            {
                '+' => old => old + opByVal,
                '*' => old => old * (byOld ? old : opByVal),
                _ => throw new Exception("unexpected op"),
            };

            Divisor = test;
            TargetIfTrue = ifTrue;
            TargetIfFalse = ifFalse;
        }

        readonly public int Id, Divisor;
        public int Score;
        readonly public List<int> Items;
        readonly Func<long, long> Operation;
        readonly int TargetIfTrue, TargetIfFalse;

        public void DoRound(int reduceWorry, long filter, Monkey[] others)
        {
            Score += Items.Count;
            var (destinationIfTrue, destinationIfFalse) = (others[TargetIfTrue].Items, others[TargetIfFalse].Items);
            Items.Select(i => Operation(i) / reduceWorry).ForEach(item => (item % Divisor == 0 ? destinationIfTrue : destinationIfFalse).Add((int)(item % filter)));
            Items.Clear();
        }
    }

    private static long RunRounds(string input, int numRounds, int reduceWorry)
    {
        var monkeys = Util.RegexParse<Monkey>(input.Split("\n\n").Select(line => line.Replace("\n", "").WithoutMultipleSpaces())).ToArray();

        var filter = monkeys.Product(m => m.Divisor);

        for (int round = 0; round < numRounds; ++round)
        {
            foreach (var monkey in monkeys)
            {
                monkey.DoRound(reduceWorry, filter, monkeys);
            }
        }

        return monkeys.Select(m => m.Score).OrderDescending().Take(2).Product();
    }

    public static long Part1(string input) => RunRounds(input, 20, 3);

    public static long Part2(string input) => RunRounds(input, 10000, 1);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}