namespace AoC.Advent2025;
public class Day05 : IPuzzle
{
    [Regex(@"(\d+)-(\d+)")]
    public record class Range(ulong Low, ulong High)
    {
        public bool InRange(ulong id) => id >= Low && id <= High;

        public Range Overlap(Range other) => new(Math.Min(Low, other.Low), Math.Max(High, other.High));
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
        var rules = Parser.Parse<Range>(sections[0]).OrderBy(r => r.Low).ToList();

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
                    if (r1.InRange(r2.Low) || r1.InRange(r2.High))
                    {
                        var newRule = r1.Overlap(r2);

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

        return rules.Select(r => r.High - r.Low + 1).Sum();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}