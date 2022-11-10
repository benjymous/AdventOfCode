using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day14 : IPuzzle
    {
        public string Name => "2015-14";

        public class Reindeer
        {
            readonly string Name;
            readonly int Speed;
            readonly int Sprint;
            readonly int Rest;

            [Regex(@"(.+) can fly (.+) km\/s for (.+) seconds, but then must rest for (.+) seconds\.")]
            public Reindeer(string name, int speed, int sprint, int rest)
            {
                Name = name;
                Speed = speed;
                Sprint = sprint;
                Rest = rest;
            }

            public override string ToString()
            {
                return $"{Name} - {Speed} m/s for {Sprint} s, rest {Rest} s";
            }

            public IEnumerable<int> Distance()
            {
                int total = 0;
                yield return total;
                while (true)
                {
                    for (int fly = 0; fly < Sprint; ++fly)
                    {
                        total += Speed;
                        yield return total;
                    }
                    for (int wait = 0; wait < Rest; ++wait)
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
            var deer = Util.RegexParse<Reindeer>(input);

            return MaxDistanceAfterTime(deer, 2503);
        }

        public static int MaxScoreAfterTime(IEnumerable<Reindeer> deer, int seconds)
        {
            var distances = deer.Select(d => d.Distance().Take(seconds).ToArray()).ToArray();

            Dictionary<int, int> scores = new();

            for (int timeIdx = 1; timeIdx < seconds; ++timeIdx)
            {
                int maxDistanceAtTime = 0;
                for (int deerIdx = 0; deerIdx < distances.Length; ++deerIdx)
                {
                    maxDistanceAtTime = Math.Max(distances[deerIdx][timeIdx], maxDistanceAtTime);
                }
                for (int deerIdx = 0; deerIdx < distances.Length; ++deerIdx)
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
            var deer = Util.RegexParse<Reindeer>(input);

            return MaxScoreAfterTime(deer, 2503);
        }

        public void Run(string input, ILogger logger)
        {

            //var d1 = new Reindeer("Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds");
            //var d2 = new Reindeer("Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds");

            // logger.WriteLine(d1.Distance().Skip(1000).First());
            // logger.WriteLine(d2.Distance().Skip(1000).First());

            //logger.WriteLine(MaxScoreAfterTime(new List<Reindeer>{d1,d2}, 1000));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}