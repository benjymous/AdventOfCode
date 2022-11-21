using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day14 : IPuzzle
    {
        public string Name => "2018-14";

        static IEnumerable<int> Parse(string input)
        {
            foreach (var c in input)
            {
                yield return c - '0';
            }
        }

        static IEnumerable<int> Combine(int r1, int r2)
        {
            int sum = r1 + r2;

            return Parse(sum.ToString());
        }

        public static void MoveCurrent(List<int> recipe, ref int[] current)
        {
            for (int j = 0; j < 2; ++j)
            {
                current[j] = (current[j] + 1 + recipe[current[j]]) % recipe.Count;
            }
        }

        public static string Part1(int start, int keep)
        {
            var recipe = new List<int> { 3, 7 };

            int[] current = { 0, 1 };

            while (recipe.Count < start + keep)
            {
                recipe.AddRange(Combine(recipe[current[0]], recipe[current[1]]));

                MoveCurrent(recipe, ref current);
            }

            return string.Join("", recipe).Substring(start, keep);
        }

        public static int Part2(string input)
        {
            var recipe = new List<int> { 3, 7 };
            var toFind = Parse(input.Trim()).ToArray();

            int[] current = { 0, 1 };

            int searchPos = 0;

            while (true)
            {
                while (recipe.Count < searchPos + toFind.Length)
                {
                    recipe.AddRange(Combine(recipe[current[0]], recipe[current[1]]));

                    MoveCurrent(recipe, ref current);
                }

                bool found = true;
                for (int i = 0; i < toFind.Length; ++i)
                {
                    if (recipe[i + searchPos] != toFind[i]) { found = false; break; }
                }
                if (found) return searchPos;

                searchPos++;
            }
        }

        public void Run(string input, ILogger logger)
        {
            input = input.Trim();

            logger.WriteLine("- Pt1 - " + Part1(int.Parse(input), 10));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
