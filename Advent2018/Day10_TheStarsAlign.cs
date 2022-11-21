using AoC.Utils.Vectors;
using System;
using System.Linq;
using System.Text;

namespace AoC.Advent2018
{
    public class Day10 : IPuzzle
    {
        public string Name => "2018-10";

        public class Drone
        {
            public ManhattanVector2 position;
            public ManhattanVector2 velocity;

            [Regex(@"position=<(.+, .+)> velocity=<(.+, .+)>")]
            public Drone(ManhattanVector2 pos, ManhattanVector2 vel)
            {
                position = pos;
                velocity = vel;
            }

            public void Step() => position += velocity;

            public override string ToString() => $"{position} - {velocity}";

        }

        public static (int steps, string message) Solve(string input)
        {
            var drones = Util.RegexParse<Drone>(input).ToArray();

            int steps = 0;
            while (true)
            {
                steps++;
                int minx = int.MaxValue;
                int maxx = int.MinValue;

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
                            var hit = drones.Any(d => d.position.X == x && d.position.Y == y);
                            sb.Append(hit ? "#" : " ");
                        }
                        sb.Append('\n');
                    }
                    Console.WriteLine(sb.ToString());
                    return (steps, sb.ToString());
                }
            }
        }

        public static string Part1(string input)
        {
            return Solve(input).message;
        }

        public static int Part2(string input)
        {
            return Solve(input).steps;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - \n" + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}