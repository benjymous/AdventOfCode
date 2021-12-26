using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day24 : IPuzzle
    {
        public string Name => "2021-24";

        static (int i, int j, int k) GetCycleParams(int digit) => digit switch
        {
            0 => (1, 15, 15), 1 => (1, 12, 5), 2 => (1, 13, 6), 3 => (26, -14, 7), 4 => (1, 15, 9), 5 => (26, -7, 6), 6 => (1, 14, 14), 7 => (1, 15, 3), 8 => (1, 15, 1), 9 => (26, -7, 3), 10 => (26, -8, 4), 11 => (26, -7, 6), 12 => (26, -5, 7), 13 => (26, -10, 1), _ => (0, 0, 0),
        };

        public static int CalcPart(int w, int z, int digit)
        {
            var (i, j, k) = GetCycleParams(digit);

            int x = (z % 26) + j;            
            z /= i;
            return (w != x) ? (z * 26) + w + k : z;
        }

        private static long FindModelNumber(bool biggest)
        {
            var queue = new PriorityQueue<(int prevZ, int digit), int>();
            var seen = new HashSet<(int prevZ, int digit)>();
            var cache = new Dictionary<(int digit, int z), (int w, int z)>();

            queue.Enqueue((0, 0), 0);

            var endStates = new List<(int prevZ, int finalW)>();

            int step = 0;
            long lastBest = 0;

            while (queue.TryDequeue(out (int prevZ, int digit) state, out var _))
            {
                for (int inputW = 10 - 1; inputW >= 1; --inputW)
                {
                    int newZ = CalcPart(inputW, state.prevZ, state.digit);
                    if (!cache.ContainsKey((state.digit, newZ)) || (biggest && cache[(state.digit, newZ)].w < inputW) || (!biggest && (cache[(state.digit, newZ)].w > inputW)))
                    {
                        cache[(state.digit, newZ)] = (inputW, state.prevZ);

                        if (state.digit < 13)
                        {
                            var next = (newZ, state.digit + 1);
                            if (!seen.Contains(next))
                            {
                                queue.Enqueue(next, newZ);
                                seen.Add(next);
                            }
                        }
                        else if (newZ == 0) endStates.Add((state.prevZ, inputW));                           
                    }
                }

                if ((step++ % 100000) == 0 && endStates.Any())
                {
                    var best = ReconstructAll(cache, endStates, biggest);
                    if (best == lastBest) return best;
                    lastBest = best;
                }
            }

            return 0;
        }

        private static long ReconstructAll(Dictionary<(int digit, int z), (int w, int z)> cache, List<(int prevZ, int finalW)> endStates, bool biggest)
        {
            var res = endStates.Select(v => v.finalW + Reconstruct(cache, v.prevZ, 12));
            return biggest ? res.Max() : res.Min();
        }

        private static long Reconstruct(Dictionary<(int digit, int z), (int w, int z)> cache, int prevZ, int digit)
        {
            var (w, z) = cache[(digit, prevZ)];
            return (w * (long)Math.Pow(10, 13 - digit)) + ((digit > 0) ? Reconstruct(cache, z, digit - 1) : 0);
        }

        public static long Part1(string input)
        {
            return FindModelNumber(biggest: true);
        }

        public static long Part2(string input)
        {
            return FindModelNumber(biggest: false);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}