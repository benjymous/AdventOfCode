using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day24 : IPuzzle
    {
        public string Name => "2021-24";

        public static int CalcPart(int w, int z, int digit)
        {
            var (i, j, k) = digit switch
            {
                0 => (1, 15, 15),
                1 => (1, 12, 5),
                2 => (1, 13, 6),
                3 => (26, -14, 7),
                4 => (1, 15, 9),
                5 => (26, -7, 6),
                6 => (1, 14, 14),
                7 => (1, 15, 3),
                8 => (1, 15, 1),
                9 => (26, -7, 3),
                10 => (26, -8, 4),
                11 => (26, -7, 6),
                12 => (26, -5, 7),
                13 => (26, -10, 1),
                _ => (0, 0, 0),
            };

            int x = (z % 26) + j;
            z /= i;
            return (w != x) ? (z * 26) + w + k : z;
        }

        private static long FindModelNumber(bool biggest)
        {
            var queue = new PriorityQueue<(int prevZ, int digit), int>();
            var cache = new Dictionary<int, (int w, int z)>();
            var endStates = new HashSet<(int prevZ, int finalW)>();
            queue.Enqueue((0, 0), 0);

            long step = 0, lastBest = 0;

            while (queue.TryDequeue(out (int prevZ, int digit) state, out var _))
            {
                for (int inputW = 10 - 1; inputW >= 1; --inputW)
                {
                    int newZ = CalcPart(inputW, state.prevZ, state.digit);
                    int cacheKey = state.digit + (newZ << 8);
                    if (!cache.ContainsKey(cacheKey) || (biggest && cache[cacheKey].w < inputW) || (cache[cacheKey].w > inputW))
                    {
                        cache[cacheKey] = (inputW, state.prevZ);
                        if (state.digit < 13) queue.Enqueue((newZ, state.digit + 1), newZ);
                        else if (newZ == 0)
                            if (biggest) endStates.Add((state.prevZ, inputW));
                            else return inputW + Reconstruct(cache, state.prevZ, 12);
                    }
                }

                if ((step++ % 10000) == 0 && endStates.Count != 0)
                {
                    var best = ReconstructAll(cache, endStates);
                    if (best == lastBest) return best;
                    lastBest = best;
                }
            }

            return 0;
        }

        private static long ReconstructAll(Dictionary<int, (int w, int z)> cache, HashSet<(int prevZ, int finalW)> endStates)
        {
            return endStates.Select(v => v.finalW + Reconstruct(cache, v.prevZ, 12)).Max();
        }

        private static long Reconstruct(Dictionary<int, (int w, int z)> cache, int prevZ, int digit)
        {
            var (w, z) = cache[digit + (prevZ << 8)];
            return (w * (long)Math.Pow(10, 13 - digit)) + ((digit > 0) ? Reconstruct(cache, z, digit - 1) : 0);
        }

        public static long Part1(string _)
        {
            return FindModelNumber(biggest: true);
        }

        public static long Part2(string _)
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