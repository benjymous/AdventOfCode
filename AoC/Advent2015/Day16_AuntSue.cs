﻿namespace AoC.Advent2015;
public class Day16 : IPuzzle
{
    static readonly Dictionary<string, int> clues = Util.FromString<Dictionary<string, int>>("children: 3, cats: 7, samoyeds: 2, pomeranians: 3, akitas: 0, vizslas: 0, goldfish: 5, trees: 3, cars: 2, perfumes: 1");

    [method: Regex(@"Sue (\d+): (.+)")]
    public class Sue(int id, [Split(", ", @"(?<key>.+): (?<value>.+)")] Dictionary<string, int> inv)
    {
        public int Id = id;
        public Dictionary<string, int> inventory = inv;

        public int ScorePart1(Dictionary<string, int> clues) => clues.Count(clue => clue.Value > 0 && inventory.TryGetValue(clue.Key, out int value) && clue.Value == value);

        public int ScorePart2(Dictionary<string, int> clues)
        {
            int score = 0;
            foreach (var clue in clues)
            {
                if (clue.Key is "cats" or "trees")
                {
                    // In particular, the cats and trees readings indicates that there are greater than that many 
                    // (due to the unpredictable nuclear decay of cat dander and tree pollen)
                    if (inventory.TryGetValue(clue.Key, out int value) && clue.Value < value)
                    {
                        score++;
                    }
                }
                else if (clue.Key is "pomeranians" or "goldfish")
                {
                    // the pomeranians and goldfish readings indicate that there are fewer than that many
                    // (due to the modial interaction of magnetoreluctance)
                    if ((inventory.TryGetValue(clue.Key, out int value) && clue.Value > value) || !inventory.ContainsKey(clue.Key))
                    {
                        score++;
                    }
                }
                else if (clue.Value > 0 && inventory.TryGetValue(clue.Key, out int value) && clue.Value == value)
                {
                    score++;
                }
            }
            return score;
        }
    }

    private static int Solve(Util.AutoParse<Sue> input, QuestionPart part) => input.Select(a => (part.One() ? a.ScorePart1(clues) : a.ScorePart2(clues), a)).OrderByDescending(t => t.Item1).First().a.Id;

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}