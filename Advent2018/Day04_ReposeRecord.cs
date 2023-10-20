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
            int sleepStart = 0;

            Data d = new();

            var lines = Util.Split(input).Order();

            foreach (var l in lines)
            {
                var line = l.Replace("[", "").Replace("]", "").Replace(":", "");
                var bits = line.Split(" ");
                if (line.Contains("Guard"))
                {
                    id = int.Parse(bits[3].Replace("#", ""));

                    if (!d.guards.ContainsKey(id))
                    {
                        d.guards[id] = new Dictionary<int, int>();
                        d.durations[id] = 0;
                    }
                }
                else if (line.Contains("asleep"))
                {
                    sleepStart = int.Parse(bits[1]);
                }
                else if (line.Contains("wakes"))
                {
                    int sleepEnd = int.Parse(bits[1]);
                    var duration = sleepEnd - sleepStart;
                    d.durations[id] += duration;

                    for (var m = sleepStart; m < sleepEnd; ++m)
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

            var sleepiest = data.durations.OrderByDescending(kvp => kvp.Value).First().Key;
            var m = data.guards[sleepiest].OrderByDescending(kvp => kvp.Value).First().Key;

            return sleepiest * m;
        }

        public static int Part2(string input)
        {
            var data = Parse(input);

            var (guardId, min, count) = data.guards.SelectMany(kvp => kvp.Value.Select(kvp2 => (guardId: kvp.Key, min: kvp2.Key, count: kvp2.Value))).OrderByDescending(entry => entry.count).First();
            return guardId * min;

        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
