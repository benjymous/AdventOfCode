using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day14 : IPuzzle
    {
        public string Name => "2016-14";

        static char FindTriplet(int index, string input, bool stretch, Dictionary<int, char> cache, Dictionary<int, string> hashCache)
        {
            return cache.GetOrCalculate(index, _ =>
            {
                var hashVal = GetHash(input, index, hashCache, stretch);
                for (int i = 0; i < hashVal.Length - 2; ++i)
                {
                    if (hashVal[i] == hashVal[i + 1] &&
                        hashVal[i] == hashVal[i + 2])
                    {
                        return hashVal[i];
                    }
                }

                return (char)0;
            });
        }

        static char FindQuintuplets(int index, string input, bool stretch, Dictionary<int, char> cache, Dictionary<int, string> hashCache)
        {
            return cache.GetOrCalculate(index, _ =>
            {
                var hashVal = GetHash(input, index, hashCache, stretch);
                for (int i = 0; i < hashVal.Length - 4; ++i)
                {
                    if (hashVal[i] == hashVal[i + 1] &&
                        hashVal[i] == hashVal[i + 2] &&
                        hashVal[i] == hashVal[i + 3] &&
                        hashVal[i] == hashVal[i + 4])
                    {
                        return hashVal[i];
                    }
                }

                return (char)0;
            });
        }

        public static string GetHash(string input, int number, Dictionary<int, string> cache, bool stretch)
        {
            return cache.GetOrCalculate(number, _ =>
            {
                return GenHash(input, number, stretch);
            });
        }

        private static string GenHash(string input, int number, bool stretch)
        {
            string hash;
            hash = $"{input}{number}".GetMD5String(true);
            if (stretch)
            {
                for (int i = 0; i < 2016; ++i)
                {
                    hash = hash.GetMD5String(true);
                }
            }

            return hash;
        }

        private static int GenKeys(string input, bool stretch, ILogger logger)
        {
            input = input.Trim();

            int i = 0;
            int lastChecked = 0;
            int found = 0;

            Dictionary<int, string> hashCache = new();
            Dictionary<int, char> tripCache = new();
            Dictionary<int, char> quintCache = new();
            Dictionary<char, HashSet<int>> quintCache2 = new();

            foreach (var ch in "0123456789abcdef")
            {
                quintCache2[ch] = new();
            }

            while (true)
            {
                var trip = FindTriplet(i, input, stretch, tripCache, hashCache);
                if (trip != 0)
                {
                    if (quintCache2.TryGetValue(trip, out var cached) && cached.Any(x => x > i))
                    {
                        found++;
                        if (stretch && logger != null) logger.WriteLine($"Key {found} found at {i}");
                        if (found == 64) return i;
                    }
                    else
                    {
                        int start = Math.Min(lastChecked, i + 1);
                        for (int j = i + 1; j < i + 1000; ++j)
                        {
                            if (tripCache.TryGetValue(j, out var ch) && ch == 0) continue;

                            var quint = FindQuintuplets(j, input, stretch, quintCache, hashCache);
                            if (quint != 0)
                            {
                                quintCache2[quint].Add(j);
                            }
                        }
                        lastChecked = i + 1000;
                    }
                }

                i++;
            }
        }
        public static int Part1(string input, ILogger logger = null)
        {
            return GenKeys(input, false, logger);
        }

        public static int Part2(string input, ILogger logger = null)
        {
            return GenKeys(input, true, logger);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}