namespace AoC.Advent2023;
public class Day25 : IPuzzle
{
    [Regex(@"(.+): (.+)")]
    public record class Connection(StringInt6 Element, [Split(" ")] StringInt6[] Connections);

    class Map : IMap<int>
    {
        public Map(Parser.AutoArray<Connection> input)
        {
            foreach (var connection in input)
            {
                foreach (var child in connection.Connections)
                {
                    AddConnection(connection.Element, child);
                    AddConnection(child, connection.Element);
                }
            }
        }
        int StartNode = 0, EndNode = 0;
        readonly Dictionary<int, HashSet<int>> connections = [];

        void AddConnection(int from, int to) => connections.GetOrCalculate(from, _ => []).Add(to);
        public IEnumerable<int> GetNeighbours(int location) => location == StartNode ? connections[location].Except([EndNode]) : connections[location];

        public int FindLoopLength(int start, int end) => this.FindPath(StartNode = start, EndNode = end).Length;

        public int FindIslands()
        {
            var linksToCut = connections.SelectMany(node => node.Value.Where(v => v < node.Key).Select(link => (node.Key, link, loop: FindLoopLength(node.Key, link)))).OrderByDescending(v => v.loop).Take(3);

            foreach (var (n1, n2, loop) in linksToCut)
            {
                connections[n1].Remove(n2);
                connections[n2].Remove(n1);
            }

            List<HashSet<int>> islands = [];

            HashSet<int> visited = [];

            foreach (var node in connections.Keys)
            {
                if (visited.Add(node))
                {
                    HashSet<int> currentIsland = [node];

                    Queue<int> toVisit = [.. connections[node]];

                    while (toVisit.TryDequeue(out int next))
                    {
                        if (visited.Add(next))
                        {
                            currentIsland.UnionWith(connections[next]);
                            toVisit.EnqueueRange(connections[next]);
                        }
                    }

                    islands.Add(currentIsland);
                }
            }

            return islands.Count == 2 ? islands[0].Count * islands[1].Count : 0;
        }
    }

    public static int Part1(string input) => new Map(input).FindIslands();

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}