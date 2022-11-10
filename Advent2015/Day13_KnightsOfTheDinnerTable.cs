using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day13 : IPuzzle
    {
        public string Name => "2015-13";

        public class Entry
        {
            [Regex(@"(.+) would (.+) (.+) happiness units by sitting next to (.+)")]
            public Entry(string name1, string factor, int units, string name2)
            {
                Initial1 = name1[0];
                Initial2 = name2[0];
                Score = units * ((factor == "lose") ? -1 : 1);
            }

            public char Initial1, Initial2;
            public int Score;
        }

        static int GetScore(char[] perm, Dictionary<int, int> scores)
        {
            int score = 0;
            for (int i = 0; i < perm.Length; ++i)
            {
                int j = (i + 1) % perm.Length;
                score += scores[GetKey(perm[i], perm[j])];
            }
            return score;
        }

        public static int GetKey(char p1, char p2) => (p1 << 16) + p2;

        public static int Solve(string input, bool includeYou = false)
        {
            var entries = Util.RegexParse<Entry>(input);

            HashSet<char> people = new();

            Dictionary<int, int> scores = new();

            foreach (var entry in entries)
            {
                people.Add(entry.Initial1);

                var score = entry.Score;

                scores.IncrementAtIndex(GetKey(entry.Initial1, entry.Initial2), score);
                scores.IncrementAtIndex(GetKey(entry.Initial2, entry.Initial1), score);
            }

            var starter = '?';
            if (includeYou)
            {
                starter = 'Y';
                foreach (var other in people)
                {
                    scores[GetKey(starter, other)] = 0;
                    scores[GetKey(other, starter)] = 0;
                }
            }
            else
            {
                starter = people.First();
                people.Remove(starter);
            }

            var perms = people.Permutations();

            var additional = $"{starter}";
            return perms.AsParallel().Select(p => GetScore(additional.Union(p).ToArray(), scores)).Max();
        }

        public static int Part1(string input) => Solve(input);

        public static int Part2(string input) => Solve(input, true);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}