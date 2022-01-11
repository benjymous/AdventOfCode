using AoC.Advent2017.NorthCloud;
using System;

namespace AoC.Advent2017
{
    public class Day23 : IPuzzle
    {
        public string Name => "2017-23";

        class DebugPt1 : NorthCloud.IDebugger
        {
            public int mulCount { get; private set; } = 0;
            public bool Next(int IP, IInstr instr, Variant x, Variant y, DataBus bus)
            {
                if (instr.Name() == "mul") mulCount++;
                return true;
            }
        }

        class DebugPt2: NorthCloud.IDebugger
        {
            public bool Next(int IP, IInstr instr, Variant x, Variant y, DataBus bus)
            {
                return (IP != 8);
            }
        }

        public static int Part1(string input)
        {
            var cpu = new NorthCloud.Coprocessor(input, "Common,Day23");
            var debugger = new DebugPt1();
            cpu.Debugger = debugger;
            cpu.Run();
            return debugger.mulCount;
        }

        public static Int64 Part2(string input)
        {
            var cpu = new NorthCloud.Coprocessor(input, "Common,Day23");
            var debugger = new DebugPt2();
            cpu.Debugger = debugger;
            cpu.Set('a', 1);
            cpu.Run();

            var b = cpu.Get('b');
            int h = 0;

            // counting non primes between b and b+17000 (in 17 step increments)

            for (long i=b; i<=b + 17000; i+=17)
            {
                for (long j=2; j<i; ++j)
                {
                    if (i % j == 0)
                    {
                        h++;
                        break;
                    }
                }
            }

            return h;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}