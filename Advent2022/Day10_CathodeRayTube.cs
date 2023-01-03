using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day10 : IPuzzle
    {
        public string Name => "2022-10";

        class Factory
        {
            [Regex("noop")] public static Func<int, int> Noop() => null;
            [Regex("addx (.+)")] public static Func<int, int> Addx(int v) => (current) => current + v;
        }

        private static IEnumerable<(int cycle, int val)> SimulateCPU(string input)
        {
            var instructions = Util.RegexFactory<Func<int, int>, Factory>(input);
            int cycle = 1, x = 1;
            foreach (var instr in instructions)
            {
                yield return (cycle, x);
                if (instr != null)
                {
                    for (int i = 0; i < 1; ++i) yield return (cycle + i + 1, x);
                    x = instr(x);
                    cycle++;
                }
                cycle++;
            }
        }

        public static IEnumerable<char> SimulateCRT(IEnumerable<(int cycle, int x)> watch)
        {
            foreach (var (cycle, x) in watch)
            {
                int beamPos = cycle % 40;
                yield return((beamPos >= x && beamPos <= x + 2) ? '#' : '.');
                if (cycle % 40 == 0) yield return '\n';
            }
        }

        public static int Part1(string input)
        {
            var watchValues = SimulateCPU(input);

            HashSet<int> watchCycles = new() { 20, 60, 100, 140, 180, 220 };
            return watchValues.Where(slice => watchCycles.Contains(slice.cycle))
                              .Sum(slice => slice.val * slice.cycle);
        }

        public static string Part2(string input, ILogger logger)
        {
            var crt = SimulateCRT(SimulateCPU(input)).AsString();

            logger.WriteLine(Environment.NewLine + crt);

            return crt.GetMD5String();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}