using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day04 : IPuzzle
    {
        public string Name => "2018-04";

        class Data
        {
            public Dictionary<int, Dictionary<int, int>> guards = new();
            public Dictionary<int, int> durations = new();
        }

        static Data Parse(string input)
        {
            int id = -1;
            string sleep = null;

            Data d = new();

            var lines = Util.Split(input).Order();

            foreach (var l in lines)
            {
                var line = l.Replace("[", "").Replace("]", "").Replace(":", "");
                var bits = line.Split(" ");
                if (line.Contains("Guard"))
                {
                    id = int.Parse(bits[3].Replace("#",""));

                    if (!d.guards.ContainsKey(id))
                    {
                        d.guards[id] = new Dictionary<int, int>();
                        d.durations[id] = 0;
                    }
                }
                else if (line.Contains("asleep"))
                {
                    sleep = bits[1];
                }
                else if (line.Contains("wakes"))
                {
                    var duration = int.Parse(bits[1]) - int.Parse(sleep);
                    d.durations[id] += duration;

                    for (var m = int.Parse(sleep); m < int.Parse(bits[1]); ++m)
                    {
                        d.guards[id].IncrementAtIndex(m);
                    }
                }
            }
            return d;
        }

        public static int Part1(string input)
        {
            var data = Parse(input);

            int sleepiest = -1;
            var v = 0;

            foreach (var id in data.durations.Keys)
            {
                if (data.durations[id] > v)
                {
                    v = data.durations[id];
                    sleepiest = id;
                }
            }

            var m = -1;
            var mv = -1;

            foreach (var min in data.guards[sleepiest].Keys)
            {
                if (data.guards[sleepiest][min] > mv)
                {
                    mv = data.guards[sleepiest][min];
                    m = min;
                }
            }

            return sleepiest * m;
        }

        public static int Part2(string input)
        {
            var data = Parse(input);

            var m = -1;
            var mv = -1;
            int sleepiest = -1;

            foreach (var id in data.guards.Keys)
            {
                foreach (var min in data.guards[id].Keys)
                {
                    if (data.guards[id][min] > mv)
                    {
                        mv = data.guards[id][min];
                        m = min;
                        sleepiest = id;
                    }
                }
            }

            return sleepiest * m;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
