namespace AoC.Advent2025;

public class Day11 : IPuzzle
{
    class TreeFactory()
    {
        public readonly Tree<string> Tree = new();

        [Regex("(...): (.+)")] public void Node(string key, [Split(" ")] string[] children) => Tree.AddChildren(key, children);
        
        public static TreeNode<string, object> Build(string input, string root) => Parser.Factory<TreeFactory>(input).Tree[root];
    }

    static long FindPathsToOut(TreeNode<string, object> node) => FindWorkingPathsToOut(node, node, false, false);
    static long FindWorkingPathsToOut(TreeNode<string, object> node) => FindWorkingPathsToOut(node, node);
    static long FindWorkingPathsToOut(TreeNode<string, object> node, TreeNode<string, object> root, bool needsDac = true, bool needsFft = true) => root.Memoize((node.Id, needsDac, needsFft), _ =>
    {
        if (node.Key == "dac") needsDac = false;
        else if (node.Key == "fft") needsFft = false;

        if (!needsDac && !needsFft && node.Children.Any(n => n.Key == "out"))
        {
            return 1;
        }

        return node.Children.Sum(c => FindWorkingPathsToOut(c, root, needsDac, needsFft));
    });

    public static long Part1(string input) => FindPathsToOut(TreeFactory.Build(input, "you"));

    public static long Part2(string input) => FindWorkingPathsToOut(TreeFactory.Build(input, "svr"));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}