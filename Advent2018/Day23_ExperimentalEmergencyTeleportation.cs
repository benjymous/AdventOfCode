using AoC.Utils.Vectors;
using System;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day23 : IPuzzle
    {
        public string Name => "2018-23";

        struct Entry
        {
            [Regex(@"pos=<(.+,.+,.+)>, r=(.+)")]
            public Entry(ManhattanVector3 pos, int r)
            {
                position = pos;
                radius = r;
            }
            public ManhattanVector3 position;
            public int radius;
        }

        public static int Part1(string input)
        {
            var data = Util.RegexParse<Entry>(input).ToArray();

            var strongest = data.OrderBy(e => e.radius).LastOrDefault();

            return data.Count(e => e.position.Distance(strongest.position) <= strongest.radius);
        }

        public static int Part2(string input)
        {
            var data = Util.RegexParse<Entry>(input).ToArray();

            int maxx = int.MinValue;
            int minx = int.MaxValue;
            int maxy = int.MinValue;
            int miny = int.MaxValue;
            int maxz = int.MinValue;
            int minz = int.MaxValue;

            foreach (var e in data)
            {
                maxx = Math.Max(maxx, e.position.X);
                minx = Math.Min(minx, e.position.X);
                maxy = Math.Max(maxy, e.position.Y);
                miny = Math.Min(miny, e.position.Y);
                maxz = Math.Max(maxz, e.position.Z);
                minz = Math.Min(minz, e.position.Z);
            }

            var weakest = data.OrderBy(e => e.radius).FirstOrDefault();

            int step = Math.Max(1, weakest.radius / 2);

            ManhattanVector3 bestPos = null;
            int bestDistance = int.MaxValue;
            int bestScore = 0;

            while (step >= 1)
            {
                //Console.WriteLine(step);

                ManhattanVector3 pos = new(0, 0, 0);
                for (pos.X = minx; pos.X <= maxx; pos.X += step)
                {
                    for (pos.Y = miny; pos.Y <= maxy; pos.Y += step)
                    {
                        for (pos.Z = minz; pos.Z <= maxz; pos.Z += step)
                        {
                            var count = data.Count(e => pos.Distance(e.position) <= e.radius);

                            if (count > bestScore)
                            {
                                bestScore = count;
                                bestDistance = pos.Distance(ManhattanVector3.Zero);
                                bestPos = new ManhattanVector3(pos.X, pos.Y, pos.Z);
                            }
                            else if (count == bestScore)
                            {
                                var distance = pos.Distance(ManhattanVector3.Zero);
                                if (distance < bestDistance)
                                {
                                    bestDistance = distance;
                                    bestPos = new ManhattanVector3(pos.X, pos.Y, pos.Z);
                                }
                            }
                        }
                    }
                }

                //Console.WriteLine($"{bestPos} {bestDistance} {step}");

                minx = bestPos.X - (step * 2);
                miny = bestPos.Y - (step * 2);
                minz = bestPos.Z - (step * 2);

                maxx = bestPos.X + (step * 2);
                maxy = bestPos.Y + (step * 2);
                maxz = bestPos.Z + (step * 2);

                step /= 2;

            }

            return bestDistance;
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(Part1("pos=<0,0,0>, r=4\npos=<1,0,0>, r=1\npos=<4,0,0>, r=3\npos=<0,2,0>, r=1\npos=<0,5,0>, r=3\npos=<0,0,3>, r=1\npos=<1,1,1>, r=1\npos=<1,1,2>, r=1\npos=<1,3,1>, r=1"));

            //Console.WriteLine(Part2("pos=<10,12,12>, r=2\npos=<12,14,12>, r=2\npos=<16,12,12>, r=4\npos=<14,14,14>, r=6\npos=<50,50,50>, r=200\npos=<10,10,10>, r=5"));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
