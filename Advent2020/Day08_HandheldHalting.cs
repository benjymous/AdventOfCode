using AoC.Advent2020.Elforola;
using System;
using System.Collections.Generic;

namespace AoC.Advent2020
{
    public class Day08 : IPuzzle
    {
        public string Name => "2020-08";

        enum HaltType
        {
            Loop,
            Halt,
        }

        static HaltType CheckHalt(Elf80 cpu, ref HashSet<int> seen, ref int cyclesTested)
        {
            seen.Add(0);
            int pc = 0;
            while (true)
            {
                if (!cpu.Step())
                {
                    cyclesTested += cpu.CycleCount;
                    return HaltType.Halt;
                }
                seen.Add(pc);
                pc = cpu.Get(Elforola.RegisterId.pc);
                if (seen.Contains(pc))
                {
                    cyclesTested += cpu.CycleCount;
                    //Console.WriteLine($"looped after {cpu.CycleCount}");
                    return HaltType.Loop;
                }
            }
        }

        public static int Part1(string input)
        {
            Elf80 cpu = new(input);

            var seen = new HashSet<int>();
            int cycles = 0;
            CheckHalt(cpu, ref seen, ref cycles);
            return cpu.Get(Elforola.RegisterId.acc);
        }

        public static int Part2(string input)
        {
            Elf80 cpu = new(input);

            int cyclesTested = 0;

            var firstRun = new Elf80(cpu);
            var toChange = new HashSet<int>();

            CheckHalt(firstRun, ref toChange, ref cyclesTested);

            foreach (var i in toChange)
            {
                var clone = new Elf80(cpu);
                var instr = clone.Instructions[i];
                if (instr.Opcode == OpCode.acc) continue;

                if (instr.Opcode == OpCode.nop)
                {
                    instr.Opcode = OpCode.jmp;
                }
                else
                {
                    instr.Opcode = OpCode.nop;
                }

                var seen = new HashSet<int>();
                if (CheckHalt(clone, ref seen, ref cyclesTested) == HaltType.Halt)
                {
                    Console.WriteLine($"Found after {cyclesTested} cycles");
                    return clone.Get(RegisterId.acc);
                }
            }

            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}