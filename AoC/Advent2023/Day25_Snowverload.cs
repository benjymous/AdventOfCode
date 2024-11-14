namespace AoC.Advent2023;
public class Day25 : IPuzzle
{
    [Regex(@"(.+): (.+)")]
    public record class Connection(StringInt32 Element, [Split(" ")] StringInt32[] Connections);

    class Map : IMap<int>
    {
        public int StartNode = 0, EndNode = 0;
        public Dictionary<int, HashSet<int>> connections = [];
        public IEnumerable<int> GetNeighbours(int location) => location == StartNode ? connections[location].Except([EndNode]) : connections[location];

        public int Find(int start, int end) => this.FindPath(StartNode = start, EndNode = end).Length;
    }

    public static int FindIslands(Dictionary<int, HashSet<int>> connections, IEnumerable<(int n1, int n2, int loop)> cutLinks)
    {
        var map = new Map { connections = connections.Select(kvp => (kvp.Key, kvp.Value.ToHashSet())).ToDictionary() };

        foreach (var (n1, n2, loop) in cutLinks)
        {
            map.connections[n1].Remove(n2);
            map.connections[n2].Remove(n1);
        }

        List<HashSet<int>> islands = [[connections.Keys.First()]];

        foreach (var node in connections.Keys.Skip(1))
        {
            var island = islands.FirstOrDefault(island => map.connections[node].Intersect(island).Any() || map.FindPath(node, island.First()).Length != 0);
            if (island != null)
            {
                island.Add(node);
            }
            else
            {
                islands.Add([node]);
            }
        }

        return islands.Count == 2 ? islands[0].Count * islands[1].Count : 0;
    }

    public static int Part1(Util.AutoParse<Connection> input)
    {
        var map = new Map();

        foreach (var connection in input)
        {
            if (!map.connections.ContainsKey(connection.Element.Value)) map.connections[connection.Element.Value] = [];
            foreach (var child in connection.Connections)
            {
                if (!map.connections.ContainsKey(child.Value)) map.connections[child.Value] = [];
                map.connections[connection.Element.Value].Add(child.Value);
                map.connections[child.Value].Add(connection.Element.Value);
            }
        }

        var results = map.connections.SelectMany(node => node.Value.Where(v => v < node.Key).Select(link => (node.Key, link, loop: map.Find(node.Key, link))));
        return FindIslands(map.connections, results.OrderByDescending(v => v.loop).Take(3));
    }

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}