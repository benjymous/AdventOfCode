namespace AoC.Advent2019;
public class Day15 : IPuzzle
{
    static readonly (int dx, int dy)[] Neighbours = [(-1, 0), (1, 0), (0, -1), (0, 1)];
    public const int OPEN = 1, WALL = 0, OXYGEN = 2;

    public class RepairDrone(string input) : NPSA.IntCPU(input), NPSA.ICPUInterrupt
    {
        (int x, int y) position = (0, 0), tryDestination;
        Queue<(int x, int y)> path = [];
        public GridMap<bool> map = new(new WalkableBool());

        public (int x, int y) OxygenLocation { get; private set; }

        readonly Stack<((int x, int y) position, (int x, int y) unknownNeighbour)> unknowns = new();

        public void OutputReady()
        {
            var destinationCellType = (int)Output.Dequeue();

            map.Data[tryDestination] = destinationCellType != WALL;
            if (destinationCellType == OXYGEN) OxygenLocation = tryDestination;
            if (destinationCellType != WALL) position = tryDestination;
        }

        public void RequestInput()
        {
            unknowns.PushRange(Neighbours.Select(n => position.OffsetBy(n)).Where(n => !map.Data.ContainsKey(n)).Select(n => (position, n)));

            if (path.Count == 0)
            {
                if (unknowns.Count == 0)
                {
                    AddInput(0); // stop
                    return;
                }
                path = PlotRoute(unknowns.Pop());
            }

            tryDestination = path.Dequeue();
            AddInput(tryDestination.Subtract(position) switch
            {
                (0, -1) => 1, // up
                (0, 1) => 2,  // down
                (-1, 0) => 3, // left
                (1, 0) => 4,  // right
                _ => 0
            });
        }

        private Queue<(int x, int y)> PlotRoute(((int x, int y) position, (int x, int y) unknownNeighbour) target) => [.. FindPath(position, target.position), target.unknownNeighbour];

        public (int x, int y)[] FindPath((int x, int y) start, (int x, int y) end) => map.FindPath(start, end);
    }

    static void FloodFill(int currentDistance, (int x, int y) position, GridMap<bool> map, Dictionary<(int, int), int> distances)
    {
        if (!map.IsValidNeighbour(position) || (distances.TryGetValue(position, out var prev) && prev <= currentDistance)) return;
        distances[position] = currentDistance;
        Neighbours.ForEach(neighbour => FloodFill(currentDistance + 1, position.OffsetBy(neighbour), map, distances));
    }

    public static int Part1(string input)
    {
        var droid = new RepairDrone(input);
        droid.Run();
        return droid.FindPath((0, 0), droid.OxygenLocation).Length;
    }

    public static int Part2(string input)
    {
        var droid = new RepairDrone(input);
        droid.Run();

        Dictionary<(int, int), int> distance = [];
        FloodFill(0, droid.OxygenLocation, droid.map, distance);

        return distance.Values.Max();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}