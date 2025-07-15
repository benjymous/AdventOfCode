namespace AoC.Advent2018;
public class Day25 : IPuzzle
{
    private class Graph
    {
        public Graph(Parser.AutoArray<ManhattanVector4> input)
        {
            Index = [.. input.Select((pos, idx) => new Node { position = pos.AsSimple(), links = [idx] })];

            for (int i = 0; i < Index.Length; i++)
            {
                var node1 = Index[i];
                for (int j = i + 1; j < Index.Length; j++)
                {
                    var node2 = Index[j];
                    if (node1.IsLinkedTo(node2))
                    {
                        node1.links.Add(j);
                        node2.links.Add(i);
                    }
                }
            }
        }

        private readonly Node[] Index;

        public int CountGroups()
        {
            List<HashSet<int>> foundGroups = [Index.First().links];
            foreach (var node in Index.Skip(1))
            {
                var overlap = foundGroups.Where(g2 => g2.Overlaps(node.links)).ToHashSet();
                node.links.UnionWith(overlap.SelectMany(x => x));
                foundGroups.RemoveAll(overlap.Contains);
                foundGroups.Add(node.links);
            }
            return foundGroups.Count;
        }
    }

    private class Node
    {
        public (int x, int y, int z, int w) position;
        public HashSet<int> links = [];

        public bool IsLinkedTo(Node other) => position.Distance(other.position) <= 3;
    }

    public static int Part1(string input) => new Graph(input).CountGroups();

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}
