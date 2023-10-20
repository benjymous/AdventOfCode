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
            public Graph(string input)
            {
                Index = Util.Parse<ManhattanVector4>(input).Select((pos, idx) => new Node { position = pos.AsSimple(), Id = idx, links = new HashSet<int> { idx } }).ToArray();

                for (int i = 0; i < Index.Length; i++)
                {
                    var node1 = Index[i];
                    for (int j = i + 1; j < Index.Length; j++)
                    {
                        var node2 = Index[j];
                        if (node1.IsLinkedTo(node2))
                        {
                            node1.links.Add(node2.Id);
                            node2.links.Add(node1.Id);
                        }
                    }
                }
            }

            readonly Node[] Index;

            public int CountGroups()
            {
                List<HashSet<int>> foundGroups = new();
                foreach (var node in Index)
                {
                    var overlap = foundGroups.Where(g2 => g2.Overlaps(node.links)).ToHashSet();
                    node.links.UnionWith(overlap.SelectMany(x => x));
                    foundGroups.RemoveAll(overlap.Contains);
                    foundGroups.Add(node.links);
                }
                return foundGroups.Count;
            }
        }

        class Node
        {
            public (int x, int y, int z, int w) position;
            public HashSet<int> links = new();
            public int Id;

            public bool IsLinkedTo(Node other) => position.Distance(other.position) <= 3;
        }

        public static int Part1(string input)
        {
            return new Graph(input).CountGroups();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}
