namespace AoC.Advent2017;
public class Day07 : IPuzzle
{
    public class TreeBuilder
    {
        [Regex(@"(.+) \((\d+)\) -> (.+)")] public static (string, int, string[]) ParentNode(string key, int value, [Split(", ")] string[] children) => (key, value, children);
        [Regex(@"(.+) \((\d+)\)")] public static (string, int, string[]) LeafNode(string key, int value) => (key, value, []);

        public static Tree<string, int> Build(Util.AutoParse<(string key, int value, string[] children), TreeBuilder> input)
        {
            var tree = new Tree<string, int>();

            foreach (var (key, value, children) in input)
            {
                tree.AddChildren(key, value, children);
            }
            return tree;
        }
    }

    static int GetChildScore(TreeNode<string, int> node) => node.Value + node.Children.Sum(GetChildScore);

    public static string Part1(string input) => TreeBuilder.Build(input).GetRootKey();

    public static int Part2(string input)
    {
        var tree = TreeBuilder.Build(input);

        var currentParents = tree.Where(node => node.Children.Count == 0)
                                 .Select(node => node.Parent)
                                 .ToHashSet();

        // Find any leaves that have a missmatched weight
        while (currentParents.Count != 0)
        {
            var newParents = new HashSet<TreeNode<string, int>>();
            foreach (var node in currentParents)
            {
                var childWeights = node.Children.Select(child => (score: GetChildScore(child), child)).GroupBy(x => x.score).OrderBy(g => g.Count());

                if (childWeights.Count() > 1)
                {
                    // children weights are mismatched

                    int wrongScore = childWeights.First().First().score;
                    var wrongNode = childWeights.First().First().child;
                    int rightScore = childWeights.Last().First().score;

                    int scoreChange = rightScore - wrongScore;

                    return wrongNode.Value + scoreChange;
                }
                else
                {
                    newParents.Add(node.Parent);
                }
            }
            currentParents = newParents;
        }

        return 0;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}