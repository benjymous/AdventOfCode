using System;
using System.Linq;

namespace Advent.MMXIX
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2019-02";} }

        public Int64 RunProgram(string input, int noun, int verb)
        {
            var cpu = new NPSA.IntCPU(input);
            cpu.Poke(1, noun);
            cpu.Poke(2, verb);
            cpu.Run();
            return cpu.Peek(0);
        }

        public Int64 Part1(string input) => RunProgram(input, 12, 2);

        const int Part2_Target = 19690720;

        public int Part2(string input) => Util.Matrix(100,100)
                   .Where(val => RunProgram(input, val.Item1, val.Item2) == Part2_Target)
                   .Select(val => (100*val.Item1)+val.Item2).FirstOrDefault();

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}