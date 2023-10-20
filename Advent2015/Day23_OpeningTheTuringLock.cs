using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day23 : IPuzzle
    {
        public string Name => "2015-23";

        public class CPU
        {
            readonly string[][] program;
            int programCounter = 0;
            Dictionary<char, int> registers = new();
            public CPU(string input) => program = Util.Split(input.Replace(",", "")).Select(str => str.Split(" ")).ToArray();

            bool Step()
            {
                var bits = program[programCounter++];

                var instr = bits[0];
                var reg = bits[1][0];

                switch (instr)
                {
                    case "hlf":
                        registers[reg] /= 2;
                        break;

                    case "tpl":
                        registers[reg] *= 3;
                        break;

                    case "inc":
                        registers[reg]++;
                        break;

                    case "jmp":
                        programCounter += int.Parse(bits[1]) - 1;
                        break;

                    case "jie":
                        if ((registers[reg] % 2) == 0)
                        {
                            programCounter += int.Parse(bits[2]) - 1;
                        }

                        break;

                    case "jio":
                        if (registers[reg] == 1)
                        {
                            programCounter += int.Parse(bits[2]) - 1;
                        }
                        break;

                    default:
                        throw new Exception("Oops!");
                }

                return programCounter < program.Length;
            }

            public int Run(Dictionary<char, int> initial)
            {
                registers = initial;

                while (Step()) ;

                return registers['b'];
            }
        }

        public static int Part1(string input)
        {
            var cpu = new CPU(input);
            return cpu.Run(new Dictionary<char, int>() { { 'a', 0 }, { 'b', 0 } });
        }

        public static int Part2(string input)
        {
            var cpu = new CPU(input);
            return cpu.Run(new Dictionary<char, int>() { { 'a', 1 }, { 'b', 0 } });
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}