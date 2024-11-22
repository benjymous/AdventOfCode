namespace AoC.Advent2022;
public class Day16 : IPuzzle
{
    [method: Regex("Valve (..) has flow rate=(.+); tunnels? leads? to valves? (.+)")]
    record class Valve(TwoCC Id, int Rate, [Split(", ")] TwoCC[] Neighbours)
    {
        public uint BitIndex;
    }

    record MapData(Dictionary<TwoCC, TwoCC[]> Data) : IMap<TwoCC> { public IEnumerable<TwoCC> GetNeighbours(TwoCC location) => Data[location]; }

    private static (Dictionary<uint, int> routes, Dictionary<uint, int> valveRates, uint availableNodes) Init(string input)
    {
        return Memoize(input, _ =>
        {
            var data = Util.RegexParse<Valve>(input).ToArray();
            var valves = data.Where(val => val.Rate > 0).WithIndex(1).Select(i => { i.Value.BitIndex = 1U << i.Index; return i.Value; }).ToArray();
            return (routes: BuildRoutes(valves, new MapData(data.ToDictionary(val => val.Id, val => val.Neighbours))).ToDictionary(), valveRates: valves.ToDictionary(val => val.BitIndex, val => val.Rate), availableNodes: (uint)valves.Sum(v => v.BitIndex));
        });
    }

    private static IEnumerable<(uint, int)> BuildRoutes(Valve[] valves, MapData map)
    {
        for (int i = 0; i < valves.Length; i++)
        {
            Valve v1 = valves[i];
            yield return (1 | v1.BitIndex, map.FindPath("AA", v1.Id).Length + 1);
            for (int j = i + 1; j < valves.Length; j++)
            {
                Valve v2 = valves[j];
                yield return (v1.BitIndex | v2.BitIndex, map.FindPath(v1.Id, v2.Id).Length + 1);
            }
        }
    }

    private static int GetScore((uint, int mins, int open, int released, uint) entry) => entry.released + (entry.open * entry.mins);

    private static int Solve(Dictionary<uint, int> routes, Dictionary<uint, int> valveRates, uint availableNodes, int availableTime)
    {
        List<(uint location, int mins, int open, int released, uint toVisit)> generation = [(1U, availableTime, 0, 0, availableNodes)];
        int best = 0;

        while (generation.Count != 0)
        {
            List<(uint location, int mins, int open, int released, uint toVisit)> nextGen = [];
            foreach (var entry in generation)
            {
                best = Math.Max(best, GetScore(entry));

                foreach (var next in entry.toVisit.BitSequence())
                {
                    var distance = routes[entry.location | next];
                    var nextState = (location: next, mins: entry.mins - distance, open: entry.open + valveRates[next], released: entry.released + (entry.open * distance), toVisit: entry.toVisit - next);
                    if (nextState.mins >= 0)
                        nextGen.Add(nextState);
                }
            }

            generation = nextGen.OrderByDescending(GetScore).Take(15).ToList();
        }

        return best;
    }

    public static int Part1(string input)
    {
        var (routes, valveRates, availableNodes) = Init(input);
        return Solve(routes, valveRates, availableNodes, 30);
    }

    public static int Part2(string input)
    {
        var (routes, valveRates, availableNodes) = Init(input);

        int minValves = (valveRates.Count / 2) - 1;
        int maxValves = (valveRates.Count / 2) + 1;

        return Util.For<uint, (uint n1, uint n2)>(2, availableNodes / 2, 2, i => (i, ~i & availableNodes))
                    .Where(i => i.n1.CountBits() >= minValves && i.n1.CountBits() <= maxValves).AsParallel().Max(p => Solve(routes, valveRates, p.n1, 26) + Solve(routes, valveRates, p.n2, 26));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}