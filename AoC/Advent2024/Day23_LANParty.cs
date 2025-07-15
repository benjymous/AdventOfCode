namespace AoC.Advent2024;
public class Day23 : IPuzzle
{
    public static IEnumerable<List<T>> GetCombinations<T>(IEnumerable<T> items, int n)
    {
        if (n == 0) yield return [];
        else
        {
            int startingIndex = 0;
            foreach (var item in items)
            {
                var remainingItems = items.Skip(startingIndex + 1);
                foreach (var combination in GetCombinations(remainingItems, n - 1))
                {
                    yield return [item, .. combination];
                }
                startingIndex++;
            }
        }
    }

    public static IEnumerable<List<T>> GetCombinations<T>(IEnumerable<T> items)
    {
        for (int i = items.Count() - 1; i >= 1; i--)
        {
            foreach (var combination in GetCombinations(items, i))
            {
                yield return combination;
            }
        }
    }

    private static Tree<string> BuildTree(string input)
    {
        var tree = new Tree<string>();

        Util.Split(input).Select(line => line.Split("-")).ForEach(pair => tree.AddBidirectional(pair[0], pair[1]));

        return tree;
    }

    public static int Part1(string input)
    {
        Tree<string> tree = BuildTree(input);

        HashSet<string> groups = [];

        foreach (var (node, pair) in tree.SelectMany(node => node.Children.UniquePairs()
                .Where(pair => pair.Item1.Children.Contains(pair.Item2))
                .Where(pair => node.Key.StartsWith('t') || pair.Item1.Key.StartsWith('t') || pair.Item2.Key.StartsWith('t'))
                .Select(pair => (node, pair))))
        {
            List<string> vals = [node.Key, pair.Item1.Key, pair.Item2.Key];
            groups.Add(string.Join("-", vals.Order()));
        }

        return groups.Count;
    }

    public static string Part2(string input)
    {
        Tree<string> tree = BuildTree(input);

        List<TreeNode<string, object>> best = [];

        foreach (var node in tree)
        {
            foreach (var perm in GetCombinations([node, .. node.Children]))
            {
                if (perm.Count <= best.Count)
                    break;

                if (perm.UniquePairs().All(pair => pair.Item1.Children.Contains(pair.Item2)))
                {
                    best = perm;
                }
            }
        }

        return string.Join(",", best.Select(n => n.Key).Order());
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}