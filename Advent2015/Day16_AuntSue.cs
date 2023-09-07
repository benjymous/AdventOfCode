using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day16 : IPuzzle
    {
        public string Name => "2015-16";

        static Dictionary<string, int> ParseClues(string clues)
        {
            return clues.Split(",")
                        .Select(str => str.Split(":"))
                        .ToDictionary(bits => bits[0].Trim(), bits => int.Parse(bits[1]));
        }

        static Dictionary<string, int> clues = ParseClues("children: 3, cats: 7, samoyeds: 2, pomeranians: 3, akitas: 0, vizslas: 0, goldfish: 5, trees: 3, cars: 2, perfumes: 1");


        public class Sue
        {
            public int Id;
            public Dictionary<string, int> inventory;
            public Sue(string data)
            {
                var part1 = data[..data.IndexOf(":")];
                var part2 = data[(data.IndexOf(":") + 1)..];
                Id = int.Parse(part1.Split(" ")[1]);
                inventory = ParseClues(part2);
            }

            public int ScorePart1(Dictionary<string, int> clues)
            {
                return clues.Count(clue => clue.Value > 0 && inventory.TryGetValue(clue.Key, out int value) && clue.Value == value);
            }

            public int ScorePart2(Dictionary<string, int> clues)
            {
                int score = 0;
                foreach (var clue in clues)
                {
                    if (clue.Key == "cats" || clue.Key == "trees")
                    {
                        // In particular, the cats and trees readings indicates that there are greater than that many 
                        // (due to the unpredictable nuclear decay of cat dander and tree pollen)
                        if (inventory.TryGetValue(clue.Key, out int value) && clue.Value < value)
                        {
                            score++;
                        }
                    }
                    else if (clue.Key == "pomeranians" || clue.Key == "goldfish")
                    {
                        // the pomeranians and goldfish readings indicate that there are fewer than that many
                        // (due to the modial interaction of magnetoreluctance)
                        if (inventory.TryGetValue(clue.Key, out int value) && clue.Value > value)
                        {
                            score++;
                        }
                        else if (!inventory.ContainsKey(clue.Key))
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

        private static int Solve(string input, QuestionPart part)
        {
            return Util.Parse<Sue>(input).Select(a => (part.One() ? a.ScorePart1(clues) : a.ScorePart2(clues), a)).OrderByDescending(t => t.Item1).First().a.Id;
        }

        public static int Part1(string input)
        {
            return Solve(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Solve(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}