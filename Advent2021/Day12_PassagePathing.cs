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
            var queue = new Queue<(string location, List<string> visited, bool canRevisit)>();

            queue.Enqueue(("start", new List<string> { "start" }, revisit));
            var cache = new HashSet<string>();
            cache.Add("start");

            List<string> routes = new List<string>();

            while (queue.Any())
            {
                // take an item from the job queue
                var item = queue.Dequeue();

                var node = map.GetNode(item.location);

                foreach (var neighbour in node.Children)
                {
                    bool canRevisit = item.canRevisit;

                    if (char.IsLower(neighbour.Key[0]) && item.visited.Contains(neighbour.Key))
                    {
                        if (!canRevisit || neighbour.Key=="start" || neighbour.Key=="end")
                        {
                            continue;
                        }
                        else
                        {
                            canRevisit = false;
                        }
                    }

                    var key = String.Join(',', item.visited) + "," + neighbour.Key;
                    if (cache.Contains(key)) continue;

                    if (neighbour.Key == "end")
                    {
                        routes.Add(key);
                        continue;
                    }

                    cache.Add(key);
                    var newVisit = new List<string>(item.visited);
                    newVisit.Add(neighbour.Key);
                    queue.Enqueue((neighbour.Key, newVisit, canRevisit));
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

//            string test1 = @"start-A
//start-b
//A-c
//A-b
//b-d
//A-end
//b-end";

            //Console.WriteLine(Part1(test1.Replace("\r","")));
            //Console.WriteLine(Part2(test1.Replace("\r", "")));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}