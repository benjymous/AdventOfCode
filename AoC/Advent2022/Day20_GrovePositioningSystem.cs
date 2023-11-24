namespace AoC.Advent2022;
public class Day20 : IPuzzle
{
    private static long Shuffle(string input, long key = 1, int repeats = 1)
    {
        var circle = Circle<Boxed<long>>.Create(Util.ParseNumbers<int>(input).Select(i => new Boxed<long>(key * i)));
        var elements = circle.Elements().ToArray();

        elements.Repeat(repeats).ForEach(el => el.Move(el.Value));

        var e1 = elements.First(e => e.Value == 0).Forward(1000);
        var e2 = e1.Forward(1000);
        var e3 = e2.Forward(1000);

        return e1.Value + e2.Value + e3.Value;
    }

    public static long Part1(string input) => Shuffle(input);

    public static long Part2(string input) => Shuffle(input, 811589153L, 10);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
