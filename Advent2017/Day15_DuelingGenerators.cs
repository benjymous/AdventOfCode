using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2017
{
    public class Day15 : IPuzzle
    {
        public string Name { get { return "2017-15"; } }

        public static IEnumerable<Int64> Generator(Int64 input, int factor, int multiple)
        {
            while (true)
            {
                Int64 val = input * factor;
                Int64 remainder = val % 2147483647;
                if (multiple==0 || (remainder % multiple) == 0) 
                    yield return remainder;
                input = remainder;
            }
        }

        public static IEnumerable<Int64> GeneratorA(Int64 input, bool picky) => Generator(input, 16807, picky ? 4 : 0);
        public static IEnumerable<Int64> GeneratorB(Int64 input, bool picky) => Generator(input, 48271, picky ? 8 : 0);

        public static IEnumerable<Tuple<Int64, Int64>> GeneratorDual(Int64 inputA, Int64 inputB, bool picky)
        {
            var gena = GeneratorA(inputA, picky).GetEnumerator();
            var genb = GeneratorB(inputB, picky).GetEnumerator();

            while (true)
            {
                gena.MoveNext(); genb.MoveNext();
                yield return Tuple.Create(gena.Current, genb.Current);
            }
        }

        private static int RunDuel(string input, int pairs, bool picky)
        {
            int matches = 0;
            var values = Util.ExtractNumbers(input);
            foreach (var pair in GeneratorDual(values[0], values[1], picky).Take(pairs))
            {
                var seq1 = pair.Item1.BinarySequence().Take(16);
                var seq2 = pair.Item2.BinarySequence().Take(16);

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