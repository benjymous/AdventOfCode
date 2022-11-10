using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day15 : IPuzzle
    {
        public string Name => "2017-15";

        public static IEnumerable<int> Generator(int value, int factor, int multiple)
        {
            while (true)
            {
                value = (int)(((Int64)value * factor) % 2147483647);
                if (value % multiple == 0) yield return value;
            }
        }

        public static IEnumerable<int> GeneratorA(int input, bool picky) => Generator(input, 16807, picky ? 4 : 1);
        public static IEnumerable<int> GeneratorB(int input, bool picky) => Generator(input, 48271, picky ? 8 : 1);

        public static IEnumerable<(int a, int b)> GeneratorDual(int inputA, int inputB, bool picky)
        {
            var gena = GeneratorA(inputA, picky).GetEnumerator();
            var genb = GeneratorB(inputB, picky).GetEnumerator();

            while (true)
            {
                gena.MoveNext(); genb.MoveNext();
                yield return (gena.Current, genb.Current);
            }
        }

        private static int RunDuel(string input, int pairs, bool picky)
        {
            int matches = 0;
            var values = Util.ExtractNumbers(input);
            foreach (var (a,b) in GeneratorDual(values[0], values[1], picky).Take(pairs))
            {
                var seq1 = a.BinarySequence().Take(16);
                var seq2 = b.BinarySequence().Take(16);

                if (seq1.SequenceEqual(seq2))
                {
                    matches++;
                }
            }

            return matches;
        }

        public static int Part1(string input)
        {
            return RunDuel(input, 40000000, false);
        }

        public static int Part2(string input)
        {
            return RunDuel(input, 5000000, true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}