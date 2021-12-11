using AoC.Utils;
using AoC.Utils.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day10 : IPuzzle
    {
        public string Name { get { return "2017-10"; } }

        public struct Loop
        {
            public Loop(Circle<int> val)
            {
                first = val;
                current = val;
                skip = 0;
            }

            public int CheckSum()
            {
                return first.Value * first.Next().Value;
            }

            public string KnotHash()
            {
                IEnumerable<int> remaining = first.Values();

                string result = "";

                while (remaining.Any())
                {
                    var chunk = remaining.Take(16);
                    remaining = remaining.Skip(16);

                    result += chunk.Xor().ToHex();
                }

                return result;
            }

            public override string ToString()
            {
                return string.Join(", ", first.Values());
            }

            public Circle<int> first;
            public Circle<int> current;
            public int skip;
        }

        public static Loop PerformCycle(Loop loop, IEnumerable<int> instructions)
        {
            foreach (var instr in instructions)
            {
                loop.current.Reverse(instr);

                loop.current = loop.current.Forward(instr + loop.skip);
                loop.skip++;
            }
            return loop;
        }

        public static Loop RunHash(int listSize, IEnumerable<int> instructions, int cycles)
        {
            var cycle = new Loop(Circle<int>.Create(Enumerable.Range(0, listSize)));
            for (int i = 0; i < cycles; ++i)
            {
                cycle = PerformCycle(cycle, instructions);
            }
            return cycle;
        }

        public static string KnotHash(string input)
        {
            var instructions = input.Trim().ToList().Select(x => (int)x).ToList();
            instructions.AddRange(new int[] { 17, 31, 73, 47, 23 });

            return RunHash(256, instructions, 64).KnotHash();
        }




        public static int Part1(string input)
        {
            var instructions = Util.Parse32(input);

            return RunHash(256, instructions, 1).CheckSum();
        }

        public static string Part2(string input)
        {
            return KnotHash(input);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}