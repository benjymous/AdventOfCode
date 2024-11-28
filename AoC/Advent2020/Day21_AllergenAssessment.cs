namespace AoC.Advent2020;
public class Day21 : IPuzzle
{
    [Regex(@"(.+) \(contains (.+)\)")]
    record class Food([Split(" ")] string[] Ingredients, [Split(", ")] string[] Allergens);

    class Foods
    {
        public Foods(string input)
        {
            var FoodList = Parser.Parse<Food>(input).Index();

            foreach (var (foodIdx, food) in FoodList)
            {
                foreach (var allergen in food.Allergens)
                {
                    if (!Counts.ContainsKey(allergen)) Counts[allergen] = [];
                    foreach (var ingredient in food.Ingredients)
                    {
                        Counts[allergen].IncrementAtIndex(ingredient);
                        if (!Ingredients.ContainsKey(ingredient)) Ingredients[ingredient] = [];
                        Ingredients[ingredient].Add(foodIdx);
                    }
                }
            }

            var allergenNames = Counts.Keys.ToArray();
            while (Allergens.Count < allergenNames.Length)
            {
                foreach (var allergen in allergenNames.Where(a => !Allergens.ContainsKey(a)))
                {
                    var d = Counts[allergen];

                    var (min, max) = d.Values.MinMax();

                    if (max > min) d = d.Where(kvp => kvp.Value > min).ToDictionary();

                    if (d.Values.Count == 1)
                    {
                        var foundIngredient = d.Keys.First();
                        Allergens[allergen] = foundIngredient;
                        allergenNames.ForEach(a1 => Counts[a1].Remove(foundIngredient));
                    }

                    Counts[allergen] = d;
                }
            }
        }

        readonly Dictionary<string, Dictionary<string, int>> Counts = [];

        public Dictionary<string, HashSet<int>> Ingredients = [];
        public Dictionary<string, string> Allergens = [];
    }

    public static int Part1(string input)
    {
        var foods = new Foods(input);

        var ingredients = foods.Ingredients;

        foreach (var kvp in foods.Allergens) ingredients.Remove(kvp.Value);

        return ingredients.Values.Sum(h => h.Count);
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