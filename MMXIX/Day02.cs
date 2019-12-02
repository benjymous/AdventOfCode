using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.MMXIX
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2019-02";} }

        public int RunProgram(string input, int noun, int verb)
        {
            var cpu = new IntCPU(input);
            cpu.Poke(1, noun);
            cpu.Poke(2, verb);
            cpu.Run();
            return cpu.Peek(0);
        }

        public int Part1(string input)
        {
            return RunProgram(input, 12, 2);
        }

        public int Part2(string input)
        {
            for (int noun=0; noun<100; ++noun)
            {
                for (int verb=0; verb<100; ++verb)
                {
                    int v = RunProgram(input, noun, verb);
                    if (v == 19690720)
                    {
                        return (100*noun)+verb;
                    }
                }
            }
            return -1;
        }


        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}