using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace AoC.Advent2022
{
    public class Day03 : IPuzzle
    {
        public string Name => "2022-03";

        class Rucksack
        {
            public Rucksack(string line)
            {
                var len = line.Length;
                var size = len / 2;
                All = line.ToHashSet();
                Set1 = line.Take(size).ToHashSet();
                Set2 = line.Skip(size).ToHashSet();
            }

            public IEnumerable<char> Overlap => Set1.Intersect(Set2);
            public int Priority => Priority(Overlap.First());

            public HashSet<char> All, Set1, Set2;
        }

        static int Priority(char c)
        {
            if (c >= 'a' && c <= 'z') return (c - 'a' + 1);
            if (c >= 'A' && c <= 'Z') return (c - 'A' + 27);
            throw new Exception("unexpected char");
        }

        public static int Part1(string input)
        {
            var lines = Util.Parse<Rucksack>(input);

            return lines.Sum(line => line.Priority);
        }

        public static int Part2(string input)
        {
            var lines = Util.Parse<Rucksack>(input);

            var groups = lines.Split(3).Select(g => g.ToArray());

            return groups.Sum(group => Priority(group[0].All.Intersect(group[1].All).Intersect(group[2].All).First()));
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}