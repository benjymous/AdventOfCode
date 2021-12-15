using AoC.Utils;
using System;
using System.Collections.Generic;

namespace AoC.Advent2016
{
    public class Day14 : IPuzzle
    {
        public string Name => "2016-14";

        static char FindTriplet(string input, Dictionary<string, char> cache)
        {
            if (cache.TryGetValue(input, out var res))
            {
                return res;
            }
            for (int i = 0; i < input.Length - 2; ++i)
            {
                if (input[i] == input[i + 1] &&
                    input[i] == input[i + 2])
                {
                    cache[input] = input[i];
                    return input[i];
                }
            }

            cache[input] = (char)0;
            return (char)0;
        }

        static char FindQuintuplets(string input, Dictionary<string, char> cache)
        {
            if (cache.TryGetValue(input, out var res))
            {
                return res;
            }
            for (int i = 0; i < input.Length - 4; ++i)
            {
                if (input[i] == input[i + 1] &&
                    input[i] == input[i + 2] &&
                    input[i] == input[i + 3] &&
                    input[i] == input[i + 4])
                {
                    cache[input] = input[i];
                    return input[i];
                }
            }

            cache[input] = (char)0;
            return (char)0;
        }



        public static string GetHash(string input, int number, Dictionary<int, string> cache, bool stretch)
        {
            if (cache.TryGetValue(number, out var hash))
            {
                return hash;
            }

            return cache[number] = GenHash(input, number, stretch);
        }

        private static string GenHash(string input, int number, bool stretch)
        {
            string hash;
            if (stretch)
            {
                hash = $"{input}{number}".GetMD5String().ToLower();
                for (int i = 0; i < 2016; ++i)
                {
                    hash = hash.GetMD5String().ToLower();
                }
            }
            else
            {
                hash = $"{input}{number}".GetMD5String().ToLower();
            }

            return hash;
        }

        private static int GenKeys(string input, bool stretch)
        {
            input = input.Trim();
            int i = 0;
            int found = 0;

            Dictionary<int, string> hashCache = new Dictionary<int, string>();
            Dictionary<string, char> tripCache = new Dictionary<string, char>();
            Dictionary<string, char> quintCache = new Dictionary<string, char>();
            Dictionary<char, int> quintCache2 = new Dictionary<char, int>();

            while (true)
            {
                var hashVal = GetHash(input, i, hashCache, stretch);
                var trip = FindTriplet(hashVal, tripCache);
                if (trip != 0)
                {
                    //if (quintCache2.TryGetValue(trip, out var cached) && cached > i)
                    //{
                    //    Console.WriteLine("Cache hit!");
                    //    found++;
                    //    Console.WriteLine($"Key {found} found at {i}:{hashVal} - {cached}");
                    //    if (found == 64) return i;
                    //}
                    //else
                    {
                        for (int j = i + 1; j < i + 1000; ++j)
                        {
                            var hash2 = GetHash(input, j, hashCache, stretch);
                            var quint = FindQuintuplets(hash2, quintCache);
                            if (quint != 0)
                            {
                                quintCache2[quint] = j;
                            }
                            if (quint == trip)
                            {
                                found++;
                                Console.WriteLine($"Key {found} found at {i}:{hashVal} - {j}:{hash2}");
                                if (found == 64) return i;
                                break;
                            }
                        }
                    }
                }

                i++;
            }
        }
        public static int Part1(string input)
        {
            return GenKeys(input, false);
        }

        public static int Part2(string input)
        {
            return GenKeys(input, true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}