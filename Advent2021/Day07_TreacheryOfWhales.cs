using System;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day07 : IPuzzle
    {
        public string Name { get { return "2021-07"; } }

        public static int Solve(string input, Func<int, int> FuelCost)
        {
            var positions = Util.Parse32(input).OrderBy(x => x).ToArray();

            return Enumerable.Range(positions.First(), positions.Last())
                             .Select(x => positions.Select(crab => FuelCost(Math.Abs(crab - x))).Sum())
                             .Min();
        }


        public static int Part1(string input)
        {
            return Solve(input, x => x);
        }

        public static int Part2(string input)
        {
            return Solve(input, x => x * (x + 1) / 2);
        }

        public void Run(string input, ILogger logger)
        {
            //var test = new Advent2019.NPSA.InteractiveTerminal(input);
            //test.Run();

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}