using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day15 : IPuzzle
    {
        public string Name => "2015-15";

        const int Calories = 4;

        class Ingredient
        {
            public string Name { get; private set; }

            public int[] Qualities { get; private set; }

            [Regex(@"(.+): capacity (.+), durability (.+), flavor (.+), texture (.+), calories (.+)")]
            public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
            {
                Name = name;

                Qualities = new int[] { capacity, durability, flavor, texture, calories };
            }
        }

        static int CalcScore(int[] weights, List<Ingredient> ingredients)
        {
            int score = 1;

            for (int q = 0; q < 4; ++q)
            {
                int qualScore = 0;
                for (int i = 0; i < weights.Length; ++i)
                {
                    qualScore += ingredients[i].Qualities[q] * weights[i];
                }
                qualScore = Math.Max(0, qualScore);
                score *= qualScore;
            }

            return score;
        }

        static int CalcCalories(int[] weights, List<Ingredient> ingredients)
        {
            int calories = 0;

            for (int i = 0; i < weights.Length; ++i)
            {
                calories += ingredients[i].Qualities[Calories] * weights[i];
            }

            return calories;
        }

        static IEnumerable<int[]> Neighbours(int[] weights)
        {
            if (weights[0] > 0)
            {
                yield return new int[] { weights[0] - 1, weights[1] + 1, weights[2], weights[3] };
                yield return new int[] { weights[0] - 1, weights[1], weights[2] + 1, weights[3] };
                yield return new int[] { weights[0] - 1, weights[1], weights[2], weights[3] + 1 };
            }

            if (weights[1] > 0)
            {
                yield return new int[] { weights[0] + 1, weights[1] - 1, weights[2], weights[3] };
                yield return new int[] { weights[0], weights[1] - 1, weights[2] + 1, weights[3] };
                yield return new int[] { weights[0], weights[1] - 1, weights[2], weights[3] + 1 };
            }

            if (weights[2] > 0)
            {
                yield return new int[] { weights[0] + 1, weights[1], weights[2] - 1, weights[3] };
                yield return new int[] { weights[0], weights[1] + 1, weights[2] - 1, weights[3] };
                yield return new int[] { weights[0], weights[1], weights[2] - 1, weights[3] + 1 };
            }

            if (weights[3] > 0)
            {
                yield return new int[] { weights[0] + 1, weights[1], weights[2], weights[3] - 1 };
                yield return new int[] { weights[0], weights[1] + 1, weights[2], weights[3] - 1 };
                yield return new int[] { weights[0], weights[1], weights[2] + 1, weights[3] - 1 };
            }
        }

        public static int Solve(string input, bool countCalories)
        {
            var ingredients = Util.RegexParse<Ingredient>(input).ToList();

            var jobqueue = new Queue<(int[], int)>();
            var initial = new int[4] { 25, 25, 25, 25 };
            var initialCalories = countCalories ? CalcCalories(initial, ingredients) : 500;
            var initialScore = initialCalories == 500 ? CalcScore(initial, ingredients) : 0;

            jobqueue.Enqueue((initial, initialScore));
            var cache = new Dictionary<string, int>();

            int best = initialScore;

            cache[string.Join(",", initial)] = initialScore;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item2 > best) best = entry.Item2;

                var neighbours = Neighbours(entry.Item1);

                foreach (var neighbour in neighbours)
                {
                    var key = string.Join(",", neighbour);

                    if (!cache.ContainsKey(key))
                    {
                        int calories = countCalories ? CalcCalories(neighbour, ingredients) : 500;
                        int newScore = calories == 500 ? CalcScore(neighbour, ingredients) : 0;

                        cache[key] = newScore;
                        jobqueue.Enqueue((neighbour, newScore));

                    }
                }

            }

            return best;
        }

        public static int Part1(string input)
        {
            return Solve(input, false);
        }

        public static int Part2(string input)
        {
            return Solve(input, true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}