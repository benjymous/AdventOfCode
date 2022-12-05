using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day05 : IPuzzle
    {
        public string Name => "2022-05";

        public struct Instruction
        {
            [Regex(@"move (\d+) from (\d+) to (\d+)")]
            public Instruction(int count, int from, int to)
            {
                (Count, From, To) = (count, from-1, to-1);
            }

            int Count, From, To;

            public void Apply(Stack<char>[] stacks)
            {
                for (int i=0; i<Count; ++i)
                {
                    var box = stacks[From].Pop();
                    stacks[To].Push(box);
                }
            }
        }

        public static int Part1(string input)
        {
            var data = input.Split("\n\n");
            var layout = Util.Split(data[0]);
            var instructions = Util.RegexParse<Instruction>(data[1]);

            var counts = Util.ParseNumbers<int>(layout.Last(), ' ').Max();

            var stacks = new Stack<char>[counts];
            for (int i = 0; i < counts; ++i) stacks[i] = new();

            foreach (var line in layout.Take(layout.Count() - 1).Reverse())
            {
                for (var i=0; i<counts; ++i)
                {
                    var ch = line[i * 4 + 1];
                    if (ch != ' ') stacks[i].Push(ch);
                    
                }
            }

            foreach (var instr in instructions)
            {
                instr.Apply(stacks);
            }


            foreach (var stack in stacks)
            {
                Console.Write(stack.Peek());
            }

            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}