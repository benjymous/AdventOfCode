namespace AoC.Advent2023;
public class Day02 : IPuzzle
{
    [Regex(@"(.+)")]
    public record class Set(Entry[] Items)
    {
        public readonly bool Possible = Items.All(i => i.Possible);
    }

    public enum Colour
    {
        red = 12, green = 13, blue = 14
    }

    [Regex(@"(\d+) (.+)")]
    public record class Entry(int Num, Colour Colour)
    {
        public readonly bool Possible = Num <= (int)Colour;
    }

    [Regex(@"Game (\d+): (.+)")]
    public record class Game(int Id, [Split(";")] Set[] Sets)
    {
        public bool Possible => Sets.All(s => s.Possible);

        public long Power => Sets.SelectMany(set => set.Items).GroupBy(s => s.Colour)
                             .Select(group => group.Max(s => s.Num)).Product();
    }

    public static int Part1(Util.AutoParse<Game> input) => input.Where(game => game.Possible).Sum(g => g.Id);

    public static long Part2(Util.AutoParse<Game> input) => input.Sum(g => g.Power);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}