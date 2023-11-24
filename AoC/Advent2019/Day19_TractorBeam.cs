namespace AoC.Advent2019;
public class Day19 : IPuzzle
{
    public class TestDrone(string program)
    {
        readonly NPSA.IntCPU cpu = new(program, 1000);

        public int Scans { get; private set; } = 0;

        public long Visit(int scanX, int scanY) => this.Memoize((scanX, scanY), _ =>
        {
            Scans++;
            cpu.AddInput(scanX, scanY);
            cpu.Run();
            long res = cpu.Output.Dequeue();
            cpu.Reset(1000);
            return res;
        });
    }

    public static long Part1(string input)
    {
        const int scanSize = 50;
        var drone = new TestDrone(input);

        return Util.Matrix(scanSize, scanSize).Select((pos) => drone.Visit(pos.x, pos.y)).Sum();
    }

    static bool BoxFit(TestDrone drone, int x, int y, int boxSize)
    {
        int size = boxSize - 1;

        return drone.Visit(x + size, y) +
               drone.Visit(x, y + size) == 2;
    }

    public static int Part2(string input)
    {
        const int boxSize = 100;

        ManhattanVector2 topPos = new(0, 0), bottomPos = new(0, 0);

        var drone = new TestDrone(input);

        // start out a bit, since the intial beam is gappy
        int x = boxSize; int y = 0;
        while (drone.Visit(x, y) == 0) y++;
        topPos.Set(x, y);
        bottomPos.Set(x, y);

        while (true)
        {
            // track the top of the beam
            while (true)
            {
                if (drone.Visit(topPos.X + 1, topPos.Y) > 0) topPos.X++;
                else if (drone.Visit(topPos.X, topPos.Y - 1) > 0) topPos.Y--;
                else break;
            }

            // track the bottom of the beam
            bottomPos.X = topPos.X;
            while (drone.Visit(bottomPos.X, bottomPos.Y + 1) == 1) bottomPos.Y++;

            if (bottomPos.Y - topPos.Y + 1 >= boxSize && BoxFit(drone, topPos.X, bottomPos.Y + 1 - boxSize, boxSize))
                return (topPos.X * 10000) + (bottomPos.Y + 1 - boxSize);

            topPos.Y++;
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}