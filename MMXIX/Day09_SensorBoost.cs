using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day09 : IPuzzle
    {
        public string Name { get { return "2019-09";} }
 
        public static string Run(string program, int input)
        {
            var cpu = new NPSA.IntCPU(program);
            cpu.Input.Enqueue(input);
            cpu.Run();
            return string.Join(",",cpu.Output);
        }

        public static string Part1(string input) => Run(input, 1);

        public static string Part2(string input) => Run(input, 2);

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}