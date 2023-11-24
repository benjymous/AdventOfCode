namespace AoC.Advent2015;
public class Day17 : IPuzzle
{
    private static IEnumerable<int> Noggify(string input)
    {
        int i = 0;
        var sizes = Util.ParseNumbers<int>(input).OrderDescending().ToDictionary(x => 1 << i++, x => x);

        var jobqueue = new Queue<(int values, int score)>();
        jobqueue.Enqueue((0, 0));
        var cache = new Dictionary<int, int>();

        jobqueue.Operate(entry =>
        {
            foreach (var other in sizes.Where(kvp => (entry.values & kvp.Key) == 0))
            {
                var newScore = entry.score + other.Value;
                var newValues = entry.values + other.Key;

                if (newScore > 150 || cache.ContainsKey(newValues)) continue;

                cache[newValues] = newScore;
                if (newScore < 150) jobqueue.Enqueue((newValues, newScore));
            }
        });
        return cache.Where(kvp => kvp.Value == 150).Select(kvp => kvp.Key);
    }

    public static int Part1(string input)
    {
        var nogCombos = Noggify(input);

        return nogCombos.Count();
    }

    public static int Part2(string input)
    {
        var nogCombos = Noggify(input);

        var results = nogCombos.GroupBy(combo => combo.CountBits())
                               .First();

        return results.Count();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}