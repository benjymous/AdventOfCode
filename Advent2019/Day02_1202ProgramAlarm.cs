using System;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day02 : IPuzzle
    {
        public string Name => "2019-02";

        public static Int64 RunProgram(string input, int noun, int verb)
        {
            var cpu = new NPSA.IntCPU(input);
            cpu.Poke(1, noun);
            cpu.Poke(2, verb);
            cpu.Run();
            return cpu.Peek(0);
        }

        public static Int64 Part1(string input) => RunProgram(input, 12, 2);

        const int Part2_Target = 19690720;

        public static int Part2(string input) => Util.Matrix(100, 100)
                   .Where(val => RunProgram(input, val.x, val.y) == Part2_Target)
                   .Select(val => (100 * val.x) + val.y).FirstOrDefault();

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}