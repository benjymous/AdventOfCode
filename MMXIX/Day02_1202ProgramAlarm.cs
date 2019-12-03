using System;
using System.Linq;

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

        public int Part1(string input) => RunProgram(input, 12, 2);

        const int Part2_Target = 19690720;

        public int Part2(string input) => Enumerable.Range(0,100).Select( 
            noun => Enumerable.Range(0,100).Select(
                verb => RunProgram(input,noun,verb) == Part2_Target ? (100*noun)+verb : 0
                ).Sum()
            ).Sum();

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}