using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day25 : IPuzzle
    {
        public string Name => "2018-25";

        class Graph
        {
            readonly Dictionary<(int x, int y, int z, int w), Node> index = new();

            public IEnumerable<Node> GetNodes() => index.Values;

            public Node GetNode((int x, int y, int z, int w) p)
            {
                return index.GetOrCalculate(p, p =>
                    new Node
                    {
                        position = p,
                        Index = index.Count
                    });
            }

            public void AddLink((int x, int y, int z, int w) p1, (int x, int y, int z, int w) p2)
            {
                var node1 = GetNode(p1);
                var node2 = GetNode(p2);

                node1.links.Add(node2);
                node2.links.Add(node1);
            }

            public int CountGroups()
            {
                return GetNodes().Select( node => node.FindAllLinks().OrderBy(a => a).GetCombinedHashCode())
                                 .Distinct().Count();
            }
        }

        class Node
        {
            public (int x, int y, int z, int w) position;
            public HashSet<Node> links = new();
            public int Index;

            public HashSet<int> FindAllLinks(HashSet<int> seen = null)
            {
                seen ??= new HashSet<int>() { Index };
                foreach (var node in links)
                {
                    if (!seen.Contains(node.Index))
                    {
                        seen.Add(node.Index);
                        seen = node.FindAllLinks(seen);
                    }
                }

                return seen;
            }

            public override int GetHashCode() => Index;
        }

        public static int Part1(string input)
        {
            var data = Util.Parse<ManhattanVector4>(input).Select(v => v.AsSimple());

            var Graph = new Graph();

            foreach (var item1 in data)
            {
                Graph.GetNode(item1);
                foreach (var item2 in data)
                {
                    if (item1 != item2 && item1.Distance(item2) <= 3)
                    {
                        Graph.AddLink(item1, item2);
                    }
                }
            }

            return Graph.CountGroups();
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(Part1("0,0,0,0\n3,0,0,0\n0,3,0,0\n0,0,3,0\n0,0,0,3\n0,0,0,6\n9,0,0,0\n12,0,0,0\n"));
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}
