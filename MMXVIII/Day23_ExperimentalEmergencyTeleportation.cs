using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day23 : IPuzzle
    {
        public string Name { get { return "2018-23";} }
 
        struct Entry
        {
            public Entry(string line)
            {
                line = line.Replace("<", "").Replace(">", "").Replace(",", " ").Replace("=", " ").Replace("  "," ");
                var bits = line.Split(" ");

                position = new ManhattanVector3(Int32.Parse(bits[1]),Int32.Parse(bits[2]),Int32.Parse(bits[3]));
                radius = Int32.Parse(bits[5]);
            }
            public ManhattanVector3 position;
            public int radius;
        }

        static List<Entry> Parse(string input)
        {
            return Util.Parse<Entry>(input);
        }

        public static int Part1(string input)
        {
            var data = Parse(input);

            var strongest = data.OrderBy(e => e.radius).LastOrDefault(); 


            return data.Where(e => e.position.Distance(strongest.position) <= strongest.radius).Count();
        }

        public static int Part2(string input)
        {
            var data = Parse(input);

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

            int step = Math.Max(1,weakest.radius/2);

            ManhattanVector3 bestPos = null;
            int bestDistance = int.MaxValue;
            int bestScore = 0;

            while (step >= 1)
            {
                //Console.WriteLine(step);

                ManhattanVector3 pos = new ManhattanVector3(0,0,0);
                for (pos.X=minx; pos.X<=maxx; pos.X+=step)
                {
                    for (pos.Y=miny; pos.Y<=maxy; pos.Y+=step)
                    {
                        for (pos.Z=minz; pos.Z<=maxz; pos.Z+=step)
                        {
                            var count = data.Where(e => pos.Distance(e.position) <= e.radius).Count();

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

                minx = bestPos.X - (step*2);
                miny = bestPos.Y - (step*2);
                minz = bestPos.Z - (step*2);

                maxx = bestPos.X + (step*2);
                maxy = bestPos.Y + (step*2);
                maxz = bestPos.Z + (step*2);

                step/=2;

            }

            return bestDistance;
        }

        public void Run(string input)
        {
            //Console.WriteLine(Part1("pos=<0,0,0>, r=4\npos=<1,0,0>, r=1\npos=<4,0,0>, r=3\npos=<0,2,0>, r=1\npos=<0,5,0>, r=3\npos=<0,0,3>, r=1\npos=<1,1,1>, r=1\npos=<1,1,2>, r=1\npos=<1,3,1>, r=1"));

            //Console.WriteLine(Part2("pos=<10,12,12>, r=2\npos=<12,14,12>, r=2\npos=<16,12,12>, r=4\npos=<14,14,14>, r=6\npos=<50,50,50>, r=200\npos=<10,10,10>, r=5"));

            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
