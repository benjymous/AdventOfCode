using System;
using System.Collections.Generic;

namespace AoC.Advent2015
{
    public class Day23 : IPuzzle
    {
        public string Name { get { return "2015-23"; } }

        public class CPU
        {
            string[] program;
            int programCounter = 0;
            Dictionary<string, int> registers = new Dictionary<string, int>();
            public CPU(string input)
            {
                program = Util.Split(input.Replace(",", ""));
            }

            bool Step()
            {
                var command = program[programCounter];

                var bits = command.Split(" ");
                var instr = bits[0];

                switch (instr)
                {
                    case "hlf":
                        registers[bits[1]] /= 2;
                        programCounter++;
                        break;

                    case "tpl":
                        registers[bits[1]] *= 3;
                        programCounter++;
                        break;

                    case "inc":
                        registers[bits[1]]++;
                        programCounter++;
                        break;

                    case "jmp":
                        programCounter += int.Parse(bits[1]);
                        break;

                    case "jie":
                        if ((registers[bits[1]] % 2) == 0)
                        {
                            programCounter += int.Parse(bits[2]);
                        }
                        else
                        {
                            programCounter++;
                        }
                        break;

                    case "jio":
                        if (registers[bits[1]] == 1)
                        {
                            programCounter += int.Parse(bits[2]);
                        }
                        else
                        {
                            programCounter++;
                        }
                        break;

                    default:
                        throw new Exception("Oops!");
                }

                return programCounter < program.Length;
            }

            public int Run(Dictionary<string, int> initial)
            {
                registers = initial;

                while (Step()) ;

                return registers["b"];
            }
        }

        public static int Part1(string input)
        {
            var cpu = new CPU(input);
            return cpu.Run(new Dictionary<string, int>() { { "a", 0 }, { "b", 0 } });
        }

        public static int Part2(string input)
        {
            var cpu = new CPU(input);
            return cpu.Run(new Dictionary<string, int>() { { "a", 1 }, { "b", 0 } });
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}