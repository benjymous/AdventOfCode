
namespace AoC.Advent2018;
public class Day10 : IPuzzle
{
    [method: Regex(@"position=<(.+, .+)> velocity=<(.+, .+)>")]
    public class Drone(ManhattanVector2 pos, ManhattanVector2 vel)
    {
        public (int X, int Y) position = pos;
        readonly (int X, int Y) velocity = vel;

        public void Step() => position = position.OffsetBy(velocity);

        public override string ToString() => $"{position} - {velocity}";
    }

    public static (int steps, string message) Solve(Parser.AutoArray<Drone> drones)
    {
        int steps = 0;

        while (true)
        {
            steps++;

            int miny = int.MaxValue;
            int maxy = int.MinValue;

            foreach (var drone in drones)
            {
                drone.Step();
                miny = Math.Min(miny, drone.position.Y);
                maxy = Math.Max(maxy, drone.position.Y);
            }

            if (maxy - miny < 10)
            {
                int minx = int.MaxValue;
                int maxx = int.MinValue;
                foreach (var drone in drones)
                {
                    minx = Math.Min(minx, drone.position.X);
                    maxx = Math.Max(maxx, drone.position.X);
                }

                var sb = new StringBuilder();
                for (var y = miny; y <= maxy; ++y)
                {
                    for (var x = minx; x <= maxx; ++x)
                    {
                        sb.Append(drones.Any(d => d.position == (x, y)) ? "#" : " ");
                    }
                    sb.Append('\n');
                }
                return (steps, sb.ToString());
            }
        }
    }

    public static string Part1(string input, ILogger logger)
    {
        var message = Solve(input).message;

        logger.WriteLine("\n" + message);
        return message;
    }

    public static int Part2(string input) => Solve(input).steps;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - \n" + Part1(input, logger));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}