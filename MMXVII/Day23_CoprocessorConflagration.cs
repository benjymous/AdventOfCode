using Advent.MMXVII.NorthCloud;
using System;

namespace Advent.MMXVII
{
    public class Day23 : IPuzzle
    {
        public string Name { get { return "2017-23"; } }

        class Debug : NorthCloud.IDebugger
        {
            public int mulCount { get; private set; } = 0;
            public void Next(int IP, IInstr instr, Variant x, Variant y, DataBus bus)
            {
                if (instr.Name() == "mul") mulCount++;
            }
        }

        public static int Part1(string input)
        {
            var cpu = new NorthCloud.Coprocessor(input, "Common,Day23");
            var debugger = new Debug();
            cpu.Debugger = debugger;
            cpu.Run();
            return debugger.mulCount;
        }

        public static Int64 Part2(string input)
        {
            var cpu = new NorthCloud.Coprocessor(input, "Common,Day23");
            cpu.Set('a', 1);
            cpu.PeekTime = 10000000;
            cpu.Run();
            return cpu.Get('h');
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}