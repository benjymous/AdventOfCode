using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day12 : IPuzzle
    {
        public string Name { get { return "2016-12";} }
 
        public static int Part1(string input)
        {
            var cpu = new BunniTek.BunnyCPU(input);
            cpu.Run();
            Console.WriteLine(cpu.Speed());
            return cpu.Get(BunniTek.RegisterId.a);
        }

        public static int Part2(string input)
        {
            var cpu = new BunniTek.BunnyCPU(input);
            cpu.Set(BunniTek.RegisterId.c, 2);
            cpu.Run();
            Console.WriteLine(cpu.Speed());
            return cpu.Get(BunniTek.RegisterId.a);
        }

        public void Run(string input, ILogger logger)
        {
            // var prog = "cpy 41 a\ninc a\ninc a\ndec a\njnz a 2\ndec a";

            // var cpu = new BunniTek.BunnyCPU(prog);
            // cpu.Run();
            // Console.WriteLine(cpu.Get("a"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}