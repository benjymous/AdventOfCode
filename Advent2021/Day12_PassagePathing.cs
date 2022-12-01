using AoC.Utils;
using AoC.Utils.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day12 : IPuzzle
    {
        public string Name => "2021-12";

        public static Tree<string> ParseTree(string input)
        {
            var tree = new Tree<string>();
            var data = Util.Split(input);
            foreach (var line in data)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var bits = line.Split('-');
                tree.AddBidirectional(bits[0], bits[1]);
            }
            return tree;
        }

        public static uint SetSeen(int nodeId) => (1U << nodeId);
        public static uint SetSeen(uint seen, int nodeId) => seen | (1U << nodeId);
        public static bool Contains(uint seen, int nodeId) => (seen & (1U << nodeId)) != 0;

        public static int Solve(Tree<string> map, bool revisit = false)
        { 
            var startNode = map["start"];
            var endNode = map["end"];

            var queue = new Queue<(TreeNode<string, object> location, ulong key, uint seen, bool canRevisit)>
                { (startNode, (ulong)startNode.Id, SetSeen(startNode.Id), revisit) };

            var cache = new HashSet<ulong>()
                { (ulong)startNode.Id };

            int routes = 0;

            while (queue.Any())
            {
                var item = queue.Dequeue();
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
                        if (!canRevisit || neighbour == startNode)
                        {
                            continue;
                        }
                        else
                        {
                            canRevisit = false;
                        }
                    }

                    var key = (item.key << 4) + (ulong)neighbour.Id;
                    if (cache.Contains(key)) continue;

                    cache.Add(key);

                    queue.Enqueue((neighbour, key, SetSeen(item.seen, neighbour.Id), canRevisit));
                }
            }

            return routes;
        }

        public static int Part1(string input)
        {
            var tree = ParseTree(input);
            return Solve(tree);
        }

        public static int Part2(string input)
        {
            var tree = ParseTree(input);
            return Solve(tree, true);
        }
        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}