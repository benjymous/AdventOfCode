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
            var sizes = Util.ParseNumbers<int>(input).OrderDescending().ToDictionary(x => i++, x => x);

            var jobqueue = new Queue<(HashSet<int>, int)>();
            jobqueue.Enqueue((new HashSet<int>(), 0));
            var cache = new Dictionary<string, int>
            {
                [""] = 0
            };

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item2 < 150)
                {
                    foreach (var other in sizes.Where(kvp => !entry.Item1.Contains(kvp.Key)))
                    {
                        int newScore = entry.Item2 + other.Value;

                        if (newScore > 150) continue;

                        var newValues = new HashSet<int>(entry.Item1) { other.Key };

                        var key = string.Join(",", newValues.Order());
                        if (!cache.ContainsKey(key))
                        {
                            cache[key] = newScore;
                            jobqueue.Enqueue((newValues, newScore));
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