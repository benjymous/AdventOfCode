using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using static AoC.Advent2018.Day16;

namespace AoC.Advent2018
{
    public class Day23 : IPuzzle
    {
        public string Name => "2018-23";

        [Regex(@"pos=<(.+,.+,.+)>, r=(.+)")]
        record struct Entry(ManhattanVector3 Position, int Radius) 
        {
            public bool Overlaps(Entry other) => Position.Distance(other.Position) <= Math.Max(Radius, other.Radius);
        }

        public static int Part1(string input)
        {
            var data = Util.RegexParse<Entry>(input).ToArray();

            var strongest = data.OrderByDescending(e => e.Radius).First();

            return data.Count(e => e.Position.Distance(strongest.Position) <= strongest.Radius);
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
                maxx = Math.Max(maxx, e.Position.X);
                minx = Math.Min(minx, e.Position.X);
                maxy = Math.Max(maxy, e.Position.Y);
                miny = Math.Min(miny, e.Position.Y);
                maxz = Math.Max(maxz, e.Position.Z);
                minz = Math.Min(minz, e.Position.Z);
            }

            var weakest = data.OrderBy(e => e.Radius).First();

            int step = Math.Max(1, weakest.Radius / 2);

            (int x, int y, int z) bestPos = (0,0,0);
            int bestDistance = maxx+maxy+maxz;
            int bestScore = 0;

            while (step >= 1)
            {
                int samples = 0;
                for (var x = minx; x <= maxx; x += step)
                {
                    for (var y = miny; y<= maxy; y += step)
                    {
                        for (var z = minz; z <= maxz; z += step)
                        {
                            var pos = (x, y, z);
                            var length = pos.Length();
                            
                            if (length > (bestDistance * 1.2f)) continue;

                            samples++;

                            var count = data.Count(e => e.Position.Distance(pos) <= e.Radius);

                            if (count > bestScore)
                            {
                                bestScore = count;
                                bestDistance = length;
                                bestPos = pos;
                            }
                            else if (count == bestScore)
                            {
                                var distance = length;
                                if (distance < bestDistance)
                                {
                                    bestDistance = distance;
                                    bestPos = pos;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"{step} {samples} {bestDistance} {bestScore}");

                step /= 3;

                minx = bestPos.x - step*2;
                miny = bestPos.y - step*2;
                minz = bestPos.z - step*2;

                maxx = bestPos.x + step*2;
                maxy = bestPos.y + step*2;
                maxz = bestPos.z + step*2;

                
            }

            return bestDistance;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
