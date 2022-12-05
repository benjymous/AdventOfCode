using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day05 : IPuzzle
    {
        public string Name => "2022-05";

        public record struct Instruction
        {
            [Regex(@"move (\d+) from (\d+) to (\d+)")]
            public Instruction(int count, int from, int to) => (Count, From, To) = (count, from - 1, to - 1);

            readonly int Count, From, To;

            public void ApplyV1(Stack<char>[] stacks)
            {
                for (int i=0; i<Count; ++i)
                {
                    stacks[To].Push(stacks[From].Pop());
                }
            }

            public void ApplyV2(Stack<char>[] stacks)
            {
                Stack<char> grab = new();

                for (int i = 0; i < Count; ++i)
                {
                    grab.Push(stacks[From].Pop());
                }

                while (grab.Any()) stacks[To].Push(grab.Pop());
            }
        }

        static (IEnumerable<Instruction>, Stack<char>[]) ParseData(string input)
        {
            var data = input.Split("\n\n");
            var layout = Util.Split(data[0]);
            var instructions = Util.RegexParse<Instruction>(data[1]);

            var stackCount = Util.ParseNumbers<int>(layout.Last(), ' ').Last();

            var stacks = new Stack<char>[stackCount];
            for (int i = 0; i < stackCount; ++i) stacks[i] = new();

            foreach (var line in layout.Reverse().Skip(1))
            {
                for (var i = 0; i < stackCount; ++i)
                {
                    var ch = line[i * 4 + 1];
                    if (ch != ' ') stacks[i].Push(ch);
                }
            }
            return (instructions, stacks);
        }

        public static string Part1(string input)
        {
            var (instructions, stacks) = ParseData(input);

            instructions.ForEach(i => i.ApplyV1(stacks));

            return stacks.Select(s => s.Peek()).AsString();
        }

        public static string Part2(string input)
        {
            var (instructions, stacks) = ParseData(input);

            instructions.ForEach(i => i.ApplyV2(stacks));

            return stacks.Select(s => s.Peek()).AsString();
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}