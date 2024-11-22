namespace AoC.Advent2021;
public class Day12 : IPuzzle
{
    public static Tree<string> ParseTree(string input)
    {
        var tree = new Tree<string>();
        foreach (var line in Util.Split(input))
        {
            if (string.IsNullOrEmpty(line)) continue;
            var bits = line.Split('-');
            if (bits[0] == "start") tree.AddPair(bits[0], bits[1]);
            else if (bits[1] == "start") tree.AddPair(bits[1], bits[0]);
            else tree.AddBidirectional(bits[0], bits[1]);
        }
        return tree;
    }

    public static uint SetSeen(uint seen, int nodeId) => seen | (1U << nodeId);
    public static bool Contains(uint seen, int nodeId) => (seen & (1U << nodeId)) != 0;

    public static int Solve(Tree<string> map, bool revisit = false)
    {
        var startNode = map["start"];
        var endNode = map["end"];

        var queue = new Queue<(TreeNode<string, object> location, uint seen, bool canRevisit)>
            { (startNode, SetSeen(0, startNode.Id), revisit) };

        int routes = 0;

        queue.Operate(item =>
        {
            foreach (var neighbour in item.location.Children)
            {
                if (neighbour == endNode)
                {
                    routes++;
                    continue;
                }

                bool canRevisit = item.canRevisit;

                if (char.IsLower(neighbour.Key[0]) && Contains(item.seen, neighbour.Id))
                {
                    if (canRevisit)
                    {
                        canRevisit = false;
                    }
                    else
                    {
                        continue;
                    }
                }

                queue.Enqueue((neighbour, SetSeen(item.seen, neighbour.Id), canRevisit));
            }
        });

        return routes;
    }

    public static int Part1(string input) => Solve(ParseTree(input));

    public static int Part2(string input) => Solve(ParseTree(input), true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}