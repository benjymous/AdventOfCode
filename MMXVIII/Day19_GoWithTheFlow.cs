﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day19 : IPuzzle
    {
        public string Name { get { return "2018-19";} }
 
        public static int Part1(string input)
        {
            ChronMatic.ChronCPU cpu = new ChronMatic.ChronCPU(input);
            cpu.Run();
            Console.WriteLine(cpu.Speed());

            return cpu.Get(0);
        }

        public static int Part2(string input)
        {
            ChronMatic.ChronCPU cpu = new ChronMatic.ChronCPU(input);
            cpu.Set(0, 1);
            cpu.Run();
            Console.WriteLine(cpu.Speed());

            return cpu.Get(0);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
