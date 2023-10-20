using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day19 : IPuzzle
    {
        public string Name => "2015-19";

        public static bool ReduceSubPart(ref string molecule, ref int steps, IEnumerable<string[]> rules)
        {
            bool changed = false;
            while (true)
            {
                var tmp = molecule;
                foreach (var rule in rules)
                {
                    if (molecule.Contains(rule[1]))
                    {
                        molecule = molecule.ReplaceLast(rule[1], rule[0]);
                        steps++;
                        changed = true;
                    }
                }
                if (molecule == tmp)
                {
                    return changed;
                }
            }
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);

            var rules = lines.Take(lines.Length - 1).Select(x => x.Split(" => "));
            var molecule = lines.Last();

            return rules.SelectMany(r => molecule.AllIndexesOf(r[0]).Select(i => molecule.ReplaceAtIndex(i, r[0], r[1]))).Distinct().Count();
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input.Replace("Rn", "(").Replace("Y", ",").Replace("Ar", ")"));

            var rules = lines.Take(lines.Length - 1).Select(x => x.Split(" => ")).OrderByDescending(x => x[1].Length).ToArray();

            var molecule = lines.Last();

            int steps = 0;

            while (molecule.Contains('(')) // first remove all the bracketed sections (Rn..Ar)
            {
                int left = 0;
                while (true)
                {
                    left = molecule.IndexOf('(', left + 1);
                    if (left == -1) break;

                    int count = 1;
                    int i = left + 1;
                    while (count > 0)
                    {
                        count += molecule[i++] switch { '(' => 1, ')' => -1, _ => 0 };
                    }

                    var substr = molecule[left..i];

                    var shrunk = substr;
                    if (ReduceSubPart(ref shrunk, ref steps, rules))
                    {
                        molecule = molecule.ReplaceLast(substr, shrunk);
                    }
                }
                ReduceSubPart(ref molecule, ref steps, rules.Where(r => r[1].Contains('(')));
            }

            while (molecule != "e") // with brackets gone, rest should reduce
            {
                ReduceSubPart(ref molecule, ref steps, rules);
            }
            return steps;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}