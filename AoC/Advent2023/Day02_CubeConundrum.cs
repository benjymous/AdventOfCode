namespace AoC.Advent2023;
public class Day02 : IPuzzle
{
    [Regex(@"(.+)")]
    record class Set(Entry[] Items)
    {
        public bool Possible => Items.All(i => i.Possible);
    }

    enum Colour
    {
        red = 12, green = 13, blue = 14
    };

    [Regex(@"(\d+) (.+)")]
    record class Entry(int Num, Colour Colour)
    {
        public bool Possible => Num <= (int)Colour;
    }

    [Regex(@"Game (\d+): (.+)")]
    record class Game(int Id, [Split(";")] Set[] Sets)
    {
        public bool Possible => Sets.All(s => s.Possible);

        public long Power => Sets.SelectMany(set => set.Items).GroupBy(s => s.Colour)
                             .Select(group => group.Max(s => s.Num)).Product();
    }

    public static int Part1(string input) => Util.RegexParse<Game>(input).Where(game => game.Possible).Sum(g => g.Id);

    public static long Part2(string input) => Util.RegexParse<Game>(input).Sum(g => g.Power);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}