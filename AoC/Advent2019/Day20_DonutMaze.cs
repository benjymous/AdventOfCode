namespace AoC.Advent2019;
public class Day20 : IPuzzle
{
    class PortalMap
    {
        public PortalMap(string[] lines)
        {
            var portals = new List<(ushort name, int location, bool isInner)>();
            var partPortals = new Dictionary<int, char>();
            var (width, height) = (lines[0].Length, lines.Length);

            WalkableSpaces = lines.Chars().Where(v => v.c == '.').Select(v => GetKey(v.x, v.y)).ToHashSet();
            foreach (var (x, y, c) in lines.Chars().Where(v => v.c is >= 'A' and <= 'Z'))
            {
                if (partPortals.TryGetValue(GetKey(x - 1, y), out var leftNeighbour))
                {
                    var code = Util.MakeTwoCC(leftNeighbour, c);
                    if (WalkableSpaces.Contains(GetKey(x - 2, y))) portals.Add((code, GetKey(x - 2, y), x != width - 1));
                    else portals.Add((code, GetKey(x + 1, y), x != 1));
                }
                else if (partPortals.TryGetValue(GetKey(x, y - 1), out var aboveNeighbour))
                {
                    var code = Util.MakeTwoCC(aboveNeighbour, c);
                    if (WalkableSpaces.Contains(GetKey(x, y - 2))) portals.Add((code, GetKey(x, y - 2), y != height - 1));
                    else portals.Add((code, GetKey(x, y + 1), y != 1));
                }
                else partPortals[GetKey(x, y)] = c;
            }

            foreach (var (p1, p2) in portals.GroupBy(p => p.name).Where(g => g.Count() == 2).Select(g => g.Decompose2()))
                (Portals[p1.location], Portals[p2.location]) = ((p2.location, p2.isInner ? -1 : 1), (p1.location, p1.isInner ? -1 : 1));

            (Start, End) = (portals.OrderBy(p => p.name).First().location, portals.OrderBy(p => p.name).Last().location);
        }

        static int GetKey(int x, int y) => x + (y << 8);

        readonly Dictionary<int, (int destination, int travelDirection)> Portals = [];
        readonly HashSet<int> WalkableSpaces = [];

        public readonly int Start, End, MaxDepth = 25;

        static readonly int[] neighbours = [-1, +1, -(1 << 8), 1 << 8];
        public IEnumerable<int> GetNeighbours(int key) => neighbours.Select(offset => key + offset).Where(WalkableSpaces.Contains);

        public IEnumerable<int> GetNeighbours1(int location)
        {
            if (Portals.TryGetValue(location, out var other)) yield return other.destination;
            foreach (var neighbour in GetNeighbours(location)) yield return neighbour;
        }

        public IEnumerable<int> GetNeighbours2(int location)
        {
            if (Portals.TryGetValue(location & 0xffff, out var other))
            {
                int newLevel = (location >> 16) + other.travelDirection;
                if (newLevel >= 0 && newLevel <= MaxDepth) yield return other.destination + (newLevel << 16);
            }
            foreach (var neighbour in GetNeighbours(location & 0xffff)) yield return neighbour + (location & 0xff0000);
        }
    }

    public static int Solve(string input, QuestionPart part)
    {
        var map = new PortalMap(Util.Split(input));
        Func<int, IEnumerable<int>> getNeighbours = part.One() ? map.GetNeighbours1 : map.GetNeighbours2;

        return Solver<(int location, int distance)>.Solve((map.End, 0), (entry, solver) =>
        {
            foreach (var newLocation in getNeighbours(entry.location))
                if (newLocation == map.Start) return entry.distance + 1;
                else if (solver.IsBetterThanSeen(newLocation, entry.distance + 1))
                    solver.Enqueue((newLocation, entry.distance + 1));

            return default;
        }, Math.Min);
    }

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}