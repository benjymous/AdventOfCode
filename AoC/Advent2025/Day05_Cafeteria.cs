namespace AoC.Advent2025;
public class Day05 : IPuzzle
{
    [Regex(@"(\d+)-(\d+)")]
    public record class Range(ulong low, ulong high)
    {
        public bool InRange(ulong id) => id >= low && id <= high;
    }

    public static int Part1(string input)
    {
        var sections = input.SplitSections();
        var rules = Parser.Parse<Range>(sections[0]).ToList();
        var ids = Util.ParseNumbers<ulong>(sections[1]);

        return ids.Count(id => rules.Any(r => r.InRange(id)));
    }

    public static ulong Part2(string input)
    {
        var sections = input.SplitSections();
        var rules = Parser.Parse<Range>(sections[0]).OrderBy(r => r.low).ToList();

        bool removed = false;
        do
        {
            removed = false;
            for (int i1 = 0; i1 < rules.Count; i1++)
            {
                var r1 = rules[i1];
                for (int i2 = i1 + 1; i2 < rules.Count; i2++)
                {
                    var r2 = rules[i2];
                    if (r1.InRange(r2.low) || r1.InRange(r2.high))
                    {
                        var newRule = new Range(Math.Min(r1.low, r2.low), Math.Max(r1.high, r2.high));

                        rules.Remove(r1);
                        rules.Remove(r2);

                        rules.Add(newRule);

                        removed = true;
                        break;
                    }
                }
                if (removed) break;
            }
        } while (removed);

        return rules.Select(r => r.high - r.low + 1).Sum();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}