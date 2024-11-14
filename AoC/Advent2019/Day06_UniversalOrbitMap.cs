namespace AoC.Advent2019;
public class Day06 : IPuzzle
{
    public static int MakeId(string str) => str.Select((c, i) => (c.AsDigit()) << (i * 6)).Sum();

    public static Tree<int> ParseTree(string input)
    {
        var tree = new Tree<int>();
        var data = Util.Split(input);
        foreach (var line in data)
        {
            var bits = line.Split(')');
            tree.AddPair(MakeId(bits[0]), MakeId(bits[1]));
        }
        return tree;
    }

    public static int Part1(string input)
    {
        var tree = ParseTree(input);
        return tree.Sum(n => n.GetDescendantCount());
    }

    public static int Part2(string input)
    {
        var tree = ParseTree(input);
        var youUp = tree.TraverseToRoot(MakeId("YOU")).ToArray();
        var santaUp = tree.TraverseToRoot(MakeId("SAN")).ToArray();

        return youUp.Intersect(santaUp)
            .Select(v => Array.IndexOf(youUp, v) + Array.IndexOf(santaUp, v))
            .First();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
