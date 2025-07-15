namespace AoC.Advent2017;
public class Day24 : IPuzzle
{
    public class Factory
    {
        [Regex(@"(.+)\/(.+)")] public static (int x, int y, int s) Component(int x, int y) => (x, y, x + y);
    }

    public static ((int length, int strength) longstrong, int strongest) GetChains(string input) => GetChains(0, [.. Parser.Factory<(int x, int y, int s), Factory>(input)]);

    public static ((int length, int strength) longstrong, int strongest) GetChains(int currentPort, (int x, int y, int s)[] inputs)
    {
        int strongest = 0;
        (int length, int strength) longstrong = (0, 0);

        foreach (var component in inputs.Where(c => c.x == currentPort || c.y == currentPort))
        {
            var childResult = GetChains(component.x == currentPort ? component.y : component.x, [.. inputs.Where(c => c != component)]);

            strongest = Math.Max(strongest, component.s + childResult.strongest);
            childResult.longstrong.length++;
            childResult.longstrong.strength += component.s;
            if (childResult.longstrong.length > longstrong.length || (childResult.longstrong.length == longstrong.length && childResult.longstrong.strength > longstrong.strength)) longstrong = childResult.longstrong;
        }

        return (longstrong, strongest);

    }
    public static int Part1(((int length, int strength) longstrong, int strongest) chains) => chains.strongest;

    public static int Part2(((int length, int strength) longstrong, int strongest) chains) => chains.longstrong.strength;

    public static int Part1(string input) => Part1(GetChains(input));

    public static int Part2(string input) => Part2(GetChains(input));

    public void Run(string input, ILogger logger)
    {
        var chains = GetChains(input);

        logger.WriteLine("- Pt1 - " + Part1(chains));
        logger.WriteLine("- Pt2 - " + Part2(chains));
    }
}