using Advent.Utils.Vectors;
using System;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day10 : IPuzzle
    {
        public string Name { get { return "2018-10"; } }

        public class Drone
        {
            public ManhattanVector2 position;
            public ManhattanVector2 velocity;
            public Drone(string line)
            {
                var nums = Util.ExtractNumbers(line);
                position = new ManhattanVector2(nums[0], nums[1]);
                velocity = new ManhattanVector2(nums[2], nums[3]);
            }

            public void Step() => position += velocity;

            public override string ToString() => $"{position} - {velocity}";

        }

        public static (int steps, string message) Solve(string input)
        {
            var drones = Util.Parse<Drone>(input);

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

                    minx = Math.Min(minx, drone.position.X);
                    maxx = Math.Max(maxx, drone.position.X);

                    miny = Math.Min(miny, drone.position.Y);
                    maxy = Math.Max(maxy, drone.position.Y);
                }

                if (maxy - miny < 10)
                {
                    var sb = new StringBuilder();
                    for (var y = miny; y <= maxy; ++y)
                    {
                        for (var x = minx; x <= maxx; ++x)
                        {
                            var hit = false;
                            foreach (var drone in drones)
                            {
                                if (drone.position.X == x && drone.position.Y == y)
                                {
                                    hit = true; break;
                                }
                            }
                            sb.Append(hit ? "#" : " ");
                        }
                        sb.Append("\n");
                    }
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