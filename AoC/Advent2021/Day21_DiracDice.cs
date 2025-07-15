namespace AoC.Advent2021;
public class Day21 : IPuzzle
{
    public class DeterministicDice
    {
        public int Rolls { get; private set; } = 0;
        private int Roll() => (Rolls++ % 100) + 1;

        public int Roll3() => Roll() + Roll() + Roll();
    }

    private static readonly (int roll, int weight)[] weights = [(3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1)];

    private static (long p1Won, long p2Won) DoStep(int p1Pos, int p2Pos, int p1Score = 0, int p2Score = 0, int turn = 0, Dictionary<int, (long, long)> cache = null)
    {
        if (p1Score >= 21) return (1, 0);
        if (p2Score >= 21) return (0, 1);

        return (cache ??= []).GetOrCalculate(p1Pos + (p2Pos << 4) + (p1Score << 8) + (p2Score << 13) + (turn << 18), _ =>
        {
            long p1Won = 0, p2Won = 0;

            foreach (var (roll, weight) in weights)
            {
                var res = turn == 0
                    ? DoStep((p1Pos + roll) % 10, p2Pos, p1Score + ((p1Pos + roll) % 10) + 1, p2Score, (turn + 1) % 2, cache)
                    : DoStep(p1Pos, (p2Pos + roll) % 10, p1Score, p2Score + ((p2Pos + roll) % 10) + 1, (turn + 1) % 2, cache);

                p1Won += res.p1Won * weight;
                p2Won += res.p2Won * weight;
            }

            return (p1Won, p2Won);
        });
    }

    public class Factory
    {
        [Regex(@"Player . starting position: (\d+)")]
        public static int Player(int pos) => pos - 1;
    }

    public static int Part1(Parser.AutoArray<int, Factory> input)
    {
        var positions = input.ToArray();

        var scores = new int[2] { 0, 0 };
        int turn = 0;

        var die = new DeterministicDice();
        while (!scores.Any(v => v >= 1000))
        {
            positions[turn] = (positions[turn] + die.Roll3()) % 10;
            scores[turn] += positions[turn] + 1;

            turn = (turn + 1) % 2;
        }

        return scores.Min() * die.Rolls;
    }

    public static long Part2(Parser.AutoArray<int, Factory> input)
    {
        var (p1Won, p2Won) = DoStep(input[0], input[1]);

        return Math.Max(p1Won, p2Won);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}