using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day03 : IPuzzle
    {
        public string Name => "2022-03";

        readonly struct Rucksack
        {
            public Rucksack(string line)
            {
                All = line;
                var size = line.Length / 2;
                Set1 = line.Take(size);
                Set2 = line.Skip(size);
            }

            public IEnumerable<char> Overlap => Set1.Intersect(Set2);
            public int Priority => Priority(Overlap.First());

            public readonly IEnumerable<char> All, Set1, Set2;
        }

        static int Priority(char c) => c switch
        {
            >= 'a' and <= 'z' => c - 'a' + 1,
            >= 'A' and <= 'Z' => c - 'A' + 27,
            _ => throw new Exception("unexpected char"),
        };

        public static int Part1(string input)
        {
            return Util.Parse<Rucksack>(input)
                       .Sum(line => line.Priority);
        }

        public static int Part2(string input)
        {
            return Util.Parse<Rucksack>(input)
                       .Split(3)
                       .Select(g => g.Select(r => r.All).ToArray())
                       .Sum(group => Priority(group[0].Intersect(group[1]).Intersect(group[2]).First()));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}