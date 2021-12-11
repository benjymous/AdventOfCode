using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day21 : IPuzzle
    {
        public string Name => "2020-21";

        class Food
        {
            public Food(string input)
            {
                var bits = input.Split(" (contains ");
                Ingredients = bits[0].Split(" ").ToList();
                Allergens = bits[1].Replace(")", "").Split(", ").ToList();
            }
            public List<string> Ingredients;
            public List<string> Allergens;
        }

        class Foods
        {
            public Foods(string input)
            {
                FoodList = Util.Parse<Food>(input);

                int foodIdx = 0;
                foreach (var food in FoodList)
                {
                    foreach (var allergen in food.Allergens)
                    {
                        if (!Counts.ContainsKey(allergen)) Counts[allergen] = new Dictionary<string, int>();
                        foreach (var ingredient in food.Ingredients)
                        {
                            Counts[allergen].IncrementAtIndex(ingredient);
                            if (!Ingredients.ContainsKey(ingredient)) Ingredients[ingredient] = new HashSet<int>();
                            Ingredients[ingredient].Add(foodIdx);
                        }
                    }
                    foodIdx++;
                }

                var k = Counts.Keys.ToArray();
                while (Allergens.Count < k.Length)
                {
                    foreach (var allergen in k)
                    {
                        if (Allergens.ContainsKey(allergen)) continue;
                        var d = Counts[allergen];

                        var min = d.Values.Min();
                        var max = d.Values.Max();

                        if (max > min)
                        {
                            d = d.Where(kvp => kvp.Value > min).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                        }

                        if (d.Values.Count == 1)
                        {
                            var found = d.Keys.First();
                            Allergens[allergen] = found;

                            foreach (var a1 in k)
                            {
                                if (Counts[a1].ContainsKey(found))
                                {
                                    Counts[a1].Remove(found);
                                }
                            }
                        }

                        Counts[allergen] = d;
                    }
                }
            }

            List<Food> FoodList;

            Dictionary<string, Dictionary<string, int>> Counts = new Dictionary<string, Dictionary<string, int>>();

            public Dictionary<string, HashSet<int>> Ingredients = new Dictionary<string, HashSet<int>>();
            public Dictionary<string, string> Allergens = new Dictionary<string, string>();
        }

        public static int Part1(string input)
        {
            var foods = new Foods(input);

            var ingredients = foods.Ingredients;

            foreach (var kvp in foods.Allergens)
            {
                ingredients.Remove(kvp.Value);
            }

            return ingredients.Values.Select(h => h.Count()).Sum();
        }

        public static string Part2(string input)
        {
            var foods = new Foods(input);

            return string.Join(",", foods.Allergens.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));  // 2061
            logger.WriteLine("- Pt2 - " + Part2(input));  // cdqvp,dglm,zhqjs,rbpg,xvtrfz,tgmzqjz,mfqgx,rffqhl
        }
    }
}