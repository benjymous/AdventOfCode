using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Advent2017
{
    public class Day20 : IPuzzle
    {
        public string Name => "2017-20";

        class Particle
        {
            static readonly string pattern = @"[p]=<(-*\d+,-*\d+,-*\d+)>, [v]=<(-*\d+,-*\d+,-*\d+)>, [a]=<(-*\d+,-*\d+,-*\d+)>";
            public Particle(string line)
            {
                var match = Regex.Matches(line, pattern).First();

                var vals = match.Groups.Values.ToArray();

                pos = new ManhattanVector3(vals[1].Value);
                vel = new ManhattanVector3(vals[2].Value);
                acc = new ManhattanVector3(vals[3].Value);
            }

            public ManhattanVector3 pos;
            public ManhattanVector3 vel;
            public ManhattanVector3 acc;

            public int Distance
            {
                get
                {
                    return pos.Length;
                }
            }

            public void Step()
            {
                vel += acc;
                pos += vel;
            }

            public override string ToString()
            {
                return $"P={pos}, V={vel}, A={acc};  D={Distance}";
            }
        }

        public static int Part1(string input)
        {
            var particles = Util.Parse<Particle>(input);

            var slowest = particles.OrderBy(p => p.acc.Length).First();
            return particles.IndexOf(slowest);
        }

        public static int Part2(string input)
        {
            IEnumerable<Particle> particles = Util.Parse<Particle>(input);

            int lastCol = 0;

            while (true)
            {
                foreach (var p in particles)
                {
                    p.Step();
                }

                var grouped = particles.GroupBy(p => p.pos).Where(grp => grp.Count() == 1).Select(grp => grp.First());
                if (grouped.Count() < particles.Count())
                {
                    particles = grouped;
                    lastCol = 0;
                }

                lastCol++;

                if (lastCol > 100) return particles.Count();
            }
        }


        public void Run(string input, ILogger logger)
        {
            //var test = "p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>\np=<4,0,0>, v=<0,0,0>, a=<-2,0,0>";

            //Part1(test);

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}