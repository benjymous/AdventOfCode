using AoC.Utils;
using AoC.Utils.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day10 : IPuzzle
    {
        public string Name => "2017-10";

        public struct Loop
        {
            public Loop(Circle<int> val) => (first, current) = (val, val);

            public readonly int CheckSum() => first * first.Next();

            public void PerformCycles(IEnumerable<int> instructions, int cycles)
            {
                for (int i = 0; i < cycles; ++i)
                {
                    foreach (var instr in instructions)
                    {
                        current.Reverse(instr);
                        current = current.Forward(instr + skip++);
                    }
                }
            }

            public readonly IEnumerable<int> KnotHash()
            {
                return first.Partition(16).Select(g => g.Xor());
            }

            public Circle<int> first;
            public Circle<int> current;
            public int skip = 0;
        }

        public static Loop RunHash(int listSize, IEnumerable<int> instructions, int cycles)
        {
            var cycle = new Loop(Circle<int>.Create(Enumerable.Range(0, listSize)));
            cycle.PerformCycles(instructions, cycles);
            return cycle;
        }

        public static IEnumerable<int> KnotHash(string input)
        {
            var instructions = input.Trim().Select(c => (int)c).Concat(new int[] { 17, 31, 73, 47, 23 }).ToArray();

            return RunHash(256, instructions, 64).KnotHash();
        }

        public static int Part1(string input)
        {
            var instructions = Util.ParseNumbers<int>(input);

            return RunHash(256, instructions, 1).CheckSum();
        }


        public static string Part2(string input)
        {
            return string.Join("", KnotHash(input).Select(i => i.ToHex()));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}