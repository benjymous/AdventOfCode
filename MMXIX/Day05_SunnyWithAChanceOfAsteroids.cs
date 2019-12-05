using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day05 : IPuzzle
    {
        public string Name { get { return "2019-05";} }
 
        public static string RunProgram(string program, IEnumerable<int> input)
        {
            var cpu = new IntCPU(program);
            foreach (var i in input) cpu.Input.Enqueue(i);
            cpu.Run();
            return string.Join(",", cpu.Output);
        }

        public static string Part1(string input)
        {
            return RunProgram(input, new List<int>{1});
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input)
        {
            var cpu = new IntCPU("1002,4,3,4,33");
            cpu.Run();
            Console.WriteLine(cpu.ToString());


            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
