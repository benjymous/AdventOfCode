namespace AoC.Advent2023;
public class Day02 : IPuzzle
{
    [Regex(@"(.+)")]
    record class Set(Entry[] Items)
    {
        public bool Possible() => Items.All(i => i.Possible());
    }

    [Regex(@"(\d+) (.+)")]
    record class Entry(int Num, string Colour)
    {
        public bool Possible() => Colour switch
        {
            "red" => Num <= 12,
            "green" => Num <= 13,
            "blue" => Num <= 14,
            _ => false,
        };

    }
    [Regex(@"Game (\d+): (.+)")]
    record class Game(int Id, [Split(";")] Set[] Sets)
    {
        public bool Possible() => Sets.All(s => s.Possible());

        public Dictionary<string, int> GetMinCounts()
        {
            Dictionary<string, int> values = [];

            foreach (var entry in Sets.SelectMany(set => set.Items.Where(entry => !values.TryGetValue(entry.Colour, out var prev) || prev < entry.Num)))
            {
                values[entry.Colour] = entry.Num;
            }

            return values;
        }

        public int Power()
        {
            var counts = GetMinCounts();
            return counts["red"] * counts["green"] * counts["blue"];
        }
    }

    public static int Part1(string input) => Util.RegexParse<Game>(input).Where(game => game.Possible()).Sum(g => g.Id);

    public static int Part2(string input) => Util.RegexParse<Game>(input).Sum(g => g.Power());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
