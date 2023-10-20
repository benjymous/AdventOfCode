using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day15 : IPuzzle
    {
        public string Name => "2015-15";

        const int Calories = 4;

        class Factory
        {
            [Regex(@".+: capacity (.+), durability (.+), flavor (.+), texture (.+), calories (.+)")]
            public static int[] Ingredient(int capacity, int durability, int flavor, int texture, int calories) => new int[] { capacity, durability, flavor, texture, calories };
        }

        static int CalcScore(int[] weights, int[][] ingredients, bool countCalories)
        {
            if (countCalories)
            {
                int calories = 0;
                for (int i = 0; i < 4; ++i) calories += ingredients[i][Calories] * weights[i];
                if (calories != 500) return 0;
            }

            int score = 1;

            for (int q = 0; q < 4; ++q)
            {
                int qualScore = 0;
                for (int i = 0; i < 4; ++i) qualScore += ingredients[i][q] * weights[i];
                if (qualScore <= 0) return 0;
                score *= qualScore;
            }

            return score;
        }

        public static IEnumerable<int[]> IngredientCombinations()
        {
            for (int[] combination = new int[] { 1, 0, 0, 0 }; combination[0] <= 97; ++combination[0])
                for (combination[1] = 1; combination[1] <= 97 - combination[0]; ++combination[1])
                    for (combination[2] = 1; combination[2] <= 97 - (combination[0] + combination[1]); ++combination[2])
                    {
                        combination[3] = 100 - (combination[0] + combination[1] + combination[2]);
                        yield return combination;
                    }
        }

        public static int Solve(string input, bool countCalories) => IngredientCombinations().Max(set => CalcScore(set, Util.RegexFactory<int[], Factory>(input).ToArray(), countCalories));

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