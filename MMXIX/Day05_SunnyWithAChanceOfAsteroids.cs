using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day05 : IPuzzle
    {
        public string Name { get { return "2019-05";} }
 
        public static string RunProgram(string program, int input)
        {
            var cpu = new NPSA.IntCPU(program);
            cpu.Input.Enqueue(input);
            cpu.Run();
            return string.Join(",", cpu.Output);
        }

        public static string Part1(string input)
        {
            return RunProgram(input, 1);
        }

        public static string Part2(string input)
        {
            return RunProgram(input, 5);
        }

        public void Run(string input, ILogger logger)
        {
            //var cpu = new NPSA.IntCPU("1002,4,3,4,33");
            //cpu.Run();


            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
