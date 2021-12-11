using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day19 : IPuzzle
    {
        public string Name => "2015-19";

        public static int Part1(string input)
        {
            var lines = Util.Split(input);

            var rules = lines.Take(lines.Length - 1).Select(x => x.Split(" => "));
            var molecule = lines.Last();

            HashSet<string> results = new HashSet<string>();

            foreach (var rule in rules)
            {
                var indices = molecule.AllIndexesOf(rule[0]);
                foreach (var i in indices)
                {
                    var newStr = molecule.ReplaceAtIndex(i, rule[0], rule[1]);
                    results.Add(newStr);
                }
            }

            return results.Count;
        }

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

        public static int Part2(string input)
        {
            input = input.Replace("Rn", "(").Replace("Y", ",").Replace("Ar", ")");

            var lines = Util.Split(input);

            var rules = lines.Take(lines.Length - 1).Select(x => x.Split(" => ")).OrderByDescending(x => x[1].Length).ToArray();

            var molecule = lines.Last();
            var original = molecule;

            int steps = 0;

            // first remove all the bracketed sections (Rn..Ar)
            while (molecule.Contains("("))
            {
                int left = 0;
                while (true)
                {
                    left = molecule.IndexOf("(", left + 1);
                    if (left == -1) break;

                    int count = 1;
                    int i = left + 1;
                    while (count > 0 && i != left)
                    {
                        if (molecule[i] == '(') count++;
                        if (molecule[i] == ')') count--;
                        i++;
                    }

                    var substr = molecule.Substring(left, i - left);

                    var shrunk = substr;
                    if (ReduceSubPart(ref shrunk, ref steps, rules))
                    {
                        molecule = molecule.ReplaceLast(substr, shrunk);
                    }
                }
                ReduceSubPart(ref molecule, ref steps, rules.Where(r => r[1].Contains("(")));
            }

            // with brackets gone, rest should reduce
            while (molecule != "e")
            {
                if (!ReduceSubPart(ref molecule, ref steps, rules))
                {
                    Console.WriteLine($"Stuck with {molecule} after {steps}");
                    molecule = original;
                    steps = 0;
                    rules = rules.Shuffle().ToArray();
                }
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