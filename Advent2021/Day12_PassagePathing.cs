using AoC.Utils;
using AoC.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                tree.AddPair(bits[0], bits[1]);
                tree.AddPair(bits[1], bits[0]);
            }
            return tree;
        }

        public static int Solve(Tree<string> map, bool revisit = false)
        {
            var queue = new Queue<(TreeNode<string,object> location, List<int> visited, bool canRevisit)>();

            var startNode = map.GetNode("start");
            var endNode = map.GetNode("end");

            queue.Enqueue((startNode, new List<int> { startNode.Id }, revisit));
            var cache = new HashSet<string>();
            cache.Add(string.Join(',', new int[]{ startNode.Id }));

            var routes = new List<List<int>>();

            while (queue.Any())
            {
                // take an item from the job queue
                var item = queue.Dequeue();

                var node = item.location;

                foreach (var neighbour in node.Children)
                {
                    bool canRevisit = item.canRevisit;

                    if (char.IsLower(neighbour.Key[0]) && item.visited.Contains(neighbour.Id))
                    {
                        if (!canRevisit || neighbour == startNode || neighbour == endNode)
                        {
                            continue;
                        }
                        else
                        {
                            canRevisit = false;
                        }
                    }

                    var key = string.Join(',', item.visited) + "," + neighbour.Key;
                    if (cache.Contains(key)) continue;

                    if (neighbour.Key == "end")
                    {
                        routes.Add(item.visited);
                        continue;
                    }

                    cache.Add(key);
                    var newVisit = new List<int>(item.visited);
                    newVisit.Add(neighbour.Id);
                    queue.Enqueue((neighbour, newVisit, canRevisit));
                }
            }

            return routes.Count;
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