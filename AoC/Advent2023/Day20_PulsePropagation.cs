namespace AoC.Advent2023;
public class Day20 : IPuzzle
{
    const bool Low = false, High = true;
    const char FlipFlop = '%', Conjuction = '&', Broadcaster = 'b';

    [method: Regex(@"([%&])(.+) -> (.+)")]
    public record class Node(char Type, string Id, [Split(", ")] string[] Outputs)
    {
        [Regex(@"broadcaster -> (.+)")]
        public Node([Split(", ")] string[] outputs) : this(Broadcaster, "broad", outputs) { }

        bool State = false;
        readonly Dictionary<string, bool> Memory = [];

        public HashSet<string> InputWires = [];

        IEnumerable<(string dest, string from, bool level)> Send(bool signal) => Outputs.Select(o => (o, Id, signal));

        bool DoConjunction((string dest, string from, bool level) signal)
        {
            Memory[signal.from] = signal.level;
            return !InputWires.All(id => Memory.TryGetValue(id, out var value) && value);
        }

        public IEnumerable<(string dest, string from, bool level)> Operate((string dest, string from, bool level) signal) => Type switch
        {
            Broadcaster => Send(signal.level),
            FlipFlop when signal.level == Low => Send(State = !State),
            Conjuction => Send(DoConjunction(signal)),
            _ => [],
        };
    }

    private static (int highCount, int lowCount) PushButton(Dictionary<string, Node> network, HashSet<string> watchNodes = default, Action<string> watchCallback = default)
    {
        List<(string dest, string from, bool level)> signals = [("broad", "button", Low)];
        (int lowCount, int highCount) counts = (0, 0);
        HashSet<string> watches = [];
        do
        {
            if (watchNodes != default) signals.Where(s => watchNodes.Contains(s.dest) && s.level == Low).Select(s => s.dest).ForEach(watchCallback);
            else counts = counts.OffsetBy((signals.Count(v => v.level == Low), signals.Count(v => v.level == High)));

            signals = signals.Where(s => network.ContainsKey(s.dest)).SelectMany(s => network[s.dest].Operate(s)).ToList();
        } while (signals.Count != 0);

        return counts;
    }

    private static Dictionary<string, Node> InitNetwork(string input)
    {
        var network = Util.RegexParse<Node>(input).ToDictionary(n => n.Id, n => n);
        foreach (var (sourceId, childId) in network.Values.SelectMany(node => node.Outputs.Where(childId => network.ContainsKey(childId)).Select(childId => (node.Id, childId))))
            network[childId].InputWires.Add(sourceId);

        return network;
    }

    public static int Part1(string input)
    {
        Dictionary<string, Node> network = InitNetwork(input);
        (int lowCount, int highCount) counts = (0, 0);

        for (int pushCount = 0; pushCount < 1000; ++pushCount) counts = counts.OffsetBy(PushButton(network));

        return counts.lowCount * counts.highCount;
    }

    public static long Part2(string input)
    {
        var network = InitNetwork(input);
        var watchNodes = network.Values.Single(n => n.Outputs.Contains("rx")).InputWires;
        Dictionary<string, int> lastSeen = [], cycles = [];

        for (int pushCount = 0; ; ++pushCount)
        {
            PushButton(network, watchNodes, watchedId =>
            {
                if (lastSeen.TryGetValue(watchedId, out var last))
                {
                    watchNodes.Remove(watchedId);
                    cycles[watchedId] = pushCount - last;
                }
                else lastSeen[watchedId] = pushCount;
            });

            if (watchNodes.Count == 0) return cycles.Values.Product();
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}