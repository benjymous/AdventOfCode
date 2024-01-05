namespace AoC.Advent2023;
public class Day25 : IPuzzle
{
    [Regex(@"(.+): (.+)")]
    record class Connection(string Element, [Split(" ")] string[] Connections);

    class Map : IMap<string>
    {
        public string StartNode = "";
        public string EndNode = "";
        public Dictionary<string, HashSet<string>> connections = [];
        public IEnumerable<string> GetNeighbours(string location) => location == StartNode ? connections[location].Except([EndNode]) : connections[location];

        public int Find(string start, string end)
        {
            StartNode = start;
            EndNode = end;
            return this.FindPath(start, end).Length;
        }
    }

    public static int FindIslands(Dictionary<string, HashSet<string>> connections, IEnumerable<(string n1, string n2, int loop)> cutLinks)
    {
        var map = new Map { connections = connections.ToDictionary() };

        foreach (var (n1, n2, loop) in cutLinks)
        {
            map.connections[n1].Remove(n2);
            map.connections[n2].Remove(n1);
        }

        List<HashSet<string>> islands = [];

        var nodes = connections.Keys;

        islands.Add([nodes.First()]);

        foreach (var node in connections.Keys.Skip(1))
        {
            bool found = false;
            foreach (var island in islands)
            {
                if (map.connections[node].Intersect(island).Any() || map.FindPath(node, island.First()).Length != 0)
                {
                    island.Add(node);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                islands.Add([node]);
            }
        }

        return islands.Count == 2 ? islands[0].Count * islands[1].Count : 0;
    }

    public static int Part1(string input)
    {
        var map = new Map();

        foreach (var connection in Util.RegexParse<Connection>(input))
        {
            if (!map.connections.ContainsKey(connection.Element)) map.connections[connection.Element] = [];
            foreach (var child in connection.Connections)
            {
                if (!map.connections.ContainsKey(child)) map.connections[child] = [];
                map.connections[connection.Element].Add(child);
                map.connections[child].Add(connection.Element);
            }
        }

        List<(string n1, string n2, int loop)> results = [];

        foreach (var node in map.connections)
        {
            foreach (var link in node.Value.Where(v => string.Compare(v, node.Key, StringComparison.Ordinal) < 0))
            {
                results.Add((node.Key, link, map.Find(node.Key, link)));
            }
        }

        return FindIslands(map.connections, results.OrderByDescending(v => v.loop).Take(3));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
    }
}