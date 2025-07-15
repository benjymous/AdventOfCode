namespace AoC.Advent2021;
public class Day11 : IPuzzle
{
    private class State(string input)
    {
        public int[,] Cells { get; private set; } = Util.ParseMatrix<int>(input);

        private static readonly (int dx, int dy)[] directions = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];

        private static IEnumerable<(int x, int y)> Neighbours((int x, int y) pos) => from dir in directions select (pos.x + dir.dx, pos.y + dir.dy);

        public int Step()
        {
            // inc all cells
            foreach (var (x, y) in Cells.Keys()) Cells[x, y]++;

            // flash reaction
            var toFlash = Cells.Entries().Where(entry => entry.Value > 9);
            while (toFlash.Any())
            {
                foreach (var (key, value) in toFlash)
                {
                    Cells[key.x, key.y] = -100; // enough that it can't flash again this step
                    foreach (var neighbour in Neighbours(key)) Cells.TryIncrement(neighbour);
                }
            }

            // reset flashed
            var flashed = Cells.Entries().Where(entry => entry.Value < 0).ToList();
            flashed.ForEach(entry => Cells[entry.Key.x, entry.Key.y] = 0);

            return flashed.Count;
        }
    }

    public static int Part1(string input)
    {
        var state = new State(input);

        return Enumerable.Range(1, 100).Sum(_ => state.Step());
    }

    public static int Part2(string input)
    {
        var state = new State(input);
        int cellCount = state.Cells.Width() * state.Cells.Height();

        return Util.Forever(1).First(_ => state.Step() == cellCount);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}