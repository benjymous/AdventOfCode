using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day17 : IPuzzle
    {
        public string Name => "2015-17";


        private static Dictionary<string, int> Noggify(string input)
        {
            int i = 0;
            var sizes = Util.Parse32(input).OrderBy(x => -x).ToDictionary(x => i++, x => x);

            var jobqueue = new Queue<Tuple<HashSet<int>, int>>();
            jobqueue.Enqueue(Tuple.Create(new HashSet<int>(), 0));
            var cache = new Dictionary<string, int>();

            cache[""] = 0;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item2 < 150)
                {
                    foreach (var other in sizes.Where(kvp => !entry.Item1.Contains(kvp.Key)))
                    {
                        int newScore = entry.Item2 + other.Value;

                        if (newScore > 150) continue;

                        var newValues = new HashSet<int>(entry.Item1);
                        newValues.Add(other.Key);

                        var key = string.Join(",", newValues.OrderBy(x => x));
                        if (!cache.ContainsKey(key))
                        {
                            cache[key] = newScore;
                            jobqueue.Enqueue(Tuple.Create(newValues, newScore));
                        }
                    }
                }
            }

            return cache;
        }

        public static int Part1(string input)
        {
            Dictionary<string, int> nogCombos = Noggify(input);

            var results = nogCombos.Where(kvp => kvp.Value == 150);

            return results.Count();
        }

        public static int Part2(string input)
        {
            Dictionary<string, int> nogCombos = Noggify(input);

            var results = nogCombos.Where(kvp => kvp.Value == 150)
                                   .GroupBy(kvp => kvp.Key.Where(c => c == ',').Count() + 1)
                                   .First();

            return results.Count();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}