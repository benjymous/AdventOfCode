using System;
using System.Collections.Generic;
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

        public class Sue
        {
            public int Id;
            public Dictionary<string, int> inventory;
            public Sue(string data)
            {
                var part1 = data.Substring(0, data.IndexOf(":"));
                var part2 = data.Substring(data.IndexOf(":") + 1);
                Id = int.Parse(part1.Split(" ")[1]);
                inventory = ParseClues(part2);
            }

            public int ScorePart1(Dictionary<string, int> clues)
            {
                int score = 0;
                foreach (var clue in clues)
                {
                    if (clue.Value > 0 && inventory.ContainsKey(clue.Key) && clue.Value == inventory[clue.Key])
                    {
                        score++;
                    }
                }
                return score;
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
                        if (inventory.ContainsKey(clue.Key) && clue.Value < inventory[clue.Key])
                        {
                            score++;
                        }
                    }
                    else if (clue.Key == "pomeranians" || clue.Key == "goldfish")
                    {
                        // the pomeranians and goldfish readings indicate that there are fewer than that many
                        // (due to the modial interaction of magnetoreluctance)
                        if (inventory.ContainsKey(clue.Key) && clue.Value > inventory[clue.Key])
                        {
                            score++;
                        }
                        else if (!inventory.ContainsKey(clue.Key))
                        {
                            score++;
                        }
                    }
                    else if (clue.Value > 0 && inventory.ContainsKey(clue.Key) && clue.Value == inventory[clue.Key])
                    {
                        score++;
                    }
                }
                return score;
            }
        }

        public static int Part1(string input)
        {
            var aunts = Util.Parse<Sue>(input);

            var clues = ParseClues("children: 3, cats: 7, samoyeds: 2, pomeranians: 3, akitas: 0, vizslas: 0, goldfish: 5, trees: 3, cars: 2, perfumes: 1");

            var data = aunts.Select(a => Tuple.Create(a.ScorePart1(clues), a)).OrderBy(t => -t.Item1);

            return data.First().Item2.Id;
        }

        public static int Part2(string input)
        {
            var aunts = Util.Parse<Sue>(input);

            var clues = ParseClues("children: 3, cats: 7, samoyeds: 2, pomeranians: 3, akitas: 0, vizslas: 0, goldfish: 5, trees: 3, cars: 2, perfumes: 1");

            var data = aunts.Select(a => Tuple.Create(a.ScorePart2(clues), a)).OrderBy(t => -t.Item1);

            return data.First().Item2.Id;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}