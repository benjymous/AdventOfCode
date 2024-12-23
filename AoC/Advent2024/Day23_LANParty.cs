namespace AoC.Advent2024;
public class Day23 : IPuzzle
{
    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> items, int n)
    {
        if (n == 0)
        {
            yield return [];
        }
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

    public static IEnumerable<IEnumerable<T>> GetAllCombinations<T>(IEnumerable<T> items)
    {
        for (int i = items.Count(); i >= 1; i--)
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

        foreach (var node in tree)
        {
            foreach (var pair in node.Children.UniquePairs())
            {
                if (pair.Item1.Children.Contains(pair.Item2))
                {
                    List<string> vals = [node.Key, pair.Item1.Key, pair.Item2.Key];
                    if (vals.Any(v => v.StartsWith('t')))
                    {
                        groups.Add(string.Join("-", vals.Order()));
                    }
                }
            }
        }

        return groups.Count;
    }

    public static string Part2(string input)
    {
        Tree<string> tree = BuildTree(input);

        List<TreeNode<string, object>> best = [];

        foreach (var node in tree.Where(n => n.Children.Count >= best.Count))
        {
            List<TreeNode<string, object>> group = [node, .. node.Children];
            var combos = GetAllCombinations(group);

            foreach (var perm in combos)
            {
                if (perm.Count() <= best.Count) break;

                if (!perm.UniquePairs().Where(pair => pair.Item1 != pair.Item2).Any(pair => !pair.Item1.Children.Contains(pair.Item2)))
                {
                    best = perm.ToList();
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