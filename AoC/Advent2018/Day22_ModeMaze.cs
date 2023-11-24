namespace AoC.Advent2018;
public class Day22 : IPuzzle
{
    public const char ROCKY = '.', WET = '=', NARROW = '|', BLOCKED = '#';
    const string TerrainKey = ".=|#";

    public record class Cave((int x, int y) Target, int Depth)
    {
        public char this[(int x, int y) pos] => this.Memoize(pos, pos => pos.x < 0 || pos.y < 0 || pos.x > Target.x + 20 || pos.y > Target.y + 20 ? BLOCKED : TerrainKey[ErosionLevel(pos) % 3]);

        public int GeologicIndex((int x, int y) pos) => this.Memoize(pos, _ => pos == (0, 0) || pos == Target ? 0 : pos.y == 0 ? pos.x * 16807 : pos.x == 0 ? pos.y * 48271 : ErosionLevel((pos.x - 1, pos.y)) * ErosionLevel((pos.x, pos.y - 1)));

        public int ErosionLevel((int x, int y) pos) => (GeologicIndex(pos) + Depth) % 20183;

        public int GetScore() => Util.Range2DInclusive((0, Target.y, 0, Target.x)).Sum(pos => TerrainKey.IndexOf(this[pos]));
    }

    public enum Tool { None, Torch, ClimbingGear }

    public readonly record struct State((int x, int y) Position, Tool Tool = Tool.Torch)
    {
        static bool ToolValid(char cell, Tool tool) => (cell == ROCKY && tool != Tool.None) || (cell == WET && tool != Tool.Torch) || (cell == NARROW && tool != Tool.ClimbingGear);

        public IEnumerable<(State state, int cost)> GetPotentialMoves(Cave cave)
        {
            foreach (var direction in Util.Values((1, 0), (0, 1), (-1, 0), (0, -1)))
                if (ToolValid(cave[Position.OffsetBy(direction)], Tool)) yield return (new State(Position.OffsetBy(direction), Tool), 1);

            foreach (var tool in Util.Values(Tool.None, Tool.Torch, Tool.ClimbingGear))
                if (tool != Tool && ToolValid(cave[Position], tool)) yield return (new State(Position, tool), 7);
        }
    }

    private class StateComparer : IEqualityComparer<(State, int, int)>
    {
        public bool Equals((State, int, int) x, (State, int, int) y) => x.Item1.GetHashCode() == y.Item1.GetHashCode();

        public int GetHashCode((State, int, int) obj) => obj.Item1.GetHashCode();
    }

    public static int Part1(int tx, int ty, int depth)
    {
        var cave = new Cave((tx, ty), depth);
        return cave.GetScore();
    }

    public static int Part1(string input)
    {
        var bits = input.Replace("\n", ",").Replace(" ", ",").Split(',');
        return Part1(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
    }

    public static int Part2(int tx, int ty, int depth)
    {
        var cave = new Cave((tx, ty), depth);

        var startPos = new State((0, 0), Tool.Torch);

        List<(State state, int time, int distance)> generation = [(startPos, 0, 0)];

        int best = int.MaxValue;
        var cache = new Dictionary<int, int> { { startPos.GetHashCode(), 0 } };
        var nextGen = new HashSet<(State state, int time, int distance)>(new StateComparer());

        while (generation.Count != 0)
        {
            nextGen.Clear();
            foreach (var (state, time, distance) in generation)
            {
                if (state.Position != cave.Target || state.Tool != Tool.Torch) nextGen.UnionWith(state.GetPotentialMoves(cave).Select(n => (newState: n.state, newTime: time + n.cost)).Where(v => v.newTime < best && cache.SmallerThanSeen(v.newState, v.newTime)).Select(v => (v.newState, v.newTime, v.newState.Position.Distance(cave.Target) + (v.newTime * 100))));
                else if (time < best) best = time;
            }

            generation = nextGen.OrderBy(v => v.distance).Take(64).ToList();
        }

        return best;
    }

    public static int Part2(string input)
    {
        var bits = input.Replace("\n", ",").Replace(" ", ",").Split(',');
        return Part2(int.Parse(bits[3]), int.Parse(bits[4]), int.Parse(bits[1]));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}