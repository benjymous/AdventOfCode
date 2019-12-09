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
            var cpu = new NPSA.IntCPU(program, 10240);
            cpu.Input.Enqueue(input);
            cpu.Run();
            return string.Join(",",cpu.Output);
        }

        public static string Part1(string input)
        {
            return Run(input, 1);
        }

        public static string Part2(string input)
        {
            return Run(input, 2);
        }

        public void Run(string input)
        {
            //Console.WriteLine(Part1("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99"));

            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}