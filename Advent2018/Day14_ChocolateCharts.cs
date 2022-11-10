using System;
using System.Collections.Generic;

namespace AoC.Advent2018
{
    public class Day14 : IPuzzle
    {
        public string Name => "2018-14";

        static List<int> Parse32(string input)
        {
            List<int> recipe = new();
            foreach (var c in input.Trim())
            {
                var i = c - '0';
                recipe.Add(i);
            }

            return recipe;
        }

        static List<int> Combine(int r1, int r2)
        {
            int sum = r1 + r2;

            return Parse32(sum.ToString());
        }

        static void Display(List<int> recipe, int[] current)
        {
            string str = "";
            for (var i = 0; i < recipe.Count; ++i)
            {
                if (current[0] == i)
                {
                    str += $"({recipe[i]})";
                }
                else if (current[1] == i)
                {
                    str += $"[{recipe[i]}]";
                }
                else
                {
                    str += $" {recipe[i]} ";
                }
            }
            Console.WriteLine(str);
        }

        public static int[] MoveCurrent(List<int> recipe, int[] current)
        {
            for (int j = 0; j < 2; ++j)
            {
                int forward = 1 + recipe[current[j]];
                current[j] = (current[j] + forward) % recipe.Count;
            }
            return current;
        }

        public static string Part1(int start, int keep)
        {
            var recipe = new List<int> { 3, 7 };

            int[] current = { 0, 1 };

            while (recipe.Count < start + keep)
            {
                var step = Combine(recipe[current[0]], recipe[current[1]]);
                recipe.AddRange(step);

                current = MoveCurrent(recipe, current);
            }

            return String.Join("", recipe).Substring(start, keep);
        }

        public static int Part2(string input)
        {
            var recipe = new List<int> { 3, 7 };
            var toFind = Parse32(input.Trim());

            int[] current = { 0, 1 };

            int searchPos = 0;

            while (true)
            {
                while (recipe.Count < searchPos + toFind.Count)
                {
                    var step = Combine(recipe[current[0]], recipe[current[1]]);
                    recipe.AddRange(step);

                    current = MoveCurrent(recipe, current);
                }

                bool found = true;
                for (int i = 0; i < toFind.Count; ++i)
                {
                    if (i + searchPos > recipe.Count) { found = false; break; }
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
