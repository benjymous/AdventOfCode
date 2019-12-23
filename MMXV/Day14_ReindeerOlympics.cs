using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day14 : IPuzzle
    {
        public string Name { get { return "2015-14";} }

        public class Reindeer
        {
            string name;
            int speed;
            int sprint;
            int rest;
            public Reindeer(string data)
            {
                var bits = data.Split();
                name = bits[0];
                speed = int.Parse(bits[3]);
                sprint = int.Parse(bits[6]);
                rest = int.Parse(bits[13]);
            }

            public override string ToString()
            {
                return $"{name} - {speed} m/s for {sprint} s, rest {rest} s";
            }

            public IEnumerable<int> Distance()
            {
                int total = 0;
                yield return total;
                while (true)
                {                   
                    for (int fly = 0; fly < sprint; ++fly)
                    {
                        total += speed;
                        yield return total;
                    }
                    for (int wait = 0; wait < rest; ++wait)
                    {
                        yield return total;
                    }
                }
            }
        }

        static int MaxDistanceAfterTime(IEnumerable<Reindeer> deer, int seconds)
        {
            return deer.Select(d => d.Distance().Skip(seconds).First()).Max();
        }
 
        public static int Part1(string input)
        {
            var deer = Util.Parse<Reindeer>(input);

            return MaxDistanceAfterTime(deer, 2503);
        }

        public static int MaxScoreAfterTime(IEnumerable<Reindeer> deer, int seconds)
        {
            var distances = deer.Select(d => d.Distance().Take(seconds).ToArray()).ToArray();

            Dictionary<int,int> scores = new Dictionary<int, int>();

            for(int timeIdx=1; timeIdx<seconds; ++timeIdx)
            {
                int maxDistanceAtTime = 0;
                for(int deerIdx=0; deerIdx<distances.Length; ++deerIdx)
                {
                    maxDistanceAtTime = Math.Max(distances[deerIdx][timeIdx],maxDistanceAtTime);
                }
                for(int deerIdx=0; deerIdx<distances.Length; ++deerIdx)
                {
                    if (distances[deerIdx][timeIdx] == maxDistanceAtTime)
                    {
                        scores.IncrementAtIndex(deerIdx);
                    }
                }
            }

            return scores.Select(kvp => kvp.Value).Max();
        }

        public static int Part2(string input)
        {
            var deer = Util.Parse<Reindeer>(input);

            return MaxScoreAfterTime(deer, 2503);
        }

        public void Run(string input, System.IO.TextWriter console)
        {

            //var d1 = new Reindeer("Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds");
            //var d2 = new Reindeer("Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds");

            // console.WriteLine(d1.Distance().Skip(1000).First());
            // console.WriteLine(d2.Distance().Skip(1000).First());

            //console.WriteLine(MaxScoreAfterTime(new List<Reindeer>{d1,d2}, 1000));

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}