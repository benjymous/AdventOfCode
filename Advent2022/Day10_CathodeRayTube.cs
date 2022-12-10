using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2022
{
    public class Day10 : IPuzzle
    {
        public string Name => "2022-10";

        public class Registers
        {
            public int x = 1;
        }

        public interface IInstr
        {
            public int ExtraCycles { get; }
            public void Act(Registers r);
        }

        readonly record struct Addx(int Val) : IInstr
        {
            public int ExtraCycles => 1;
            public void Act(Registers r) => r.x += Val;
        }

        public class Factory
        {
            [Regex("noop")] public static IInstr Noop() => null;
            [Regex("addx (.+)")] public static IInstr Addx(int v) => new Addx(v);
        }

        private static IEnumerable<(int cycle, Registers r)> SimulateCPU(string input)
        {
            var instructions = Util.RegexFactory<IInstr, Factory>(input);

            int cycle = 1;

            Registers r = new ();
            foreach (var instr in instructions)
            {
                yield return (cycle, r);
                if (instr != null)
                {
                    for (int i = 0; i < instr.ExtraCycles; ++i) yield return (cycle + i + 1, r);
                    instr.Act(r);
                    cycle += instr.ExtraCycles;
                }
                cycle++;
            }
        }

        public static IEnumerable<char> SimulateCRT(IEnumerable<(int cycle, Registers r)> watch)
        {
            foreach (var (cycle, r) in watch)
            {
                int beamPos = cycle % 40;
                yield return((beamPos >= r.x && beamPos <= r.x + 2) ? '#' : '.');
                if (cycle % 40 == 0) yield return '\n';
            }
        }

        public static int Part1(string input)
        {         
            var watchValues = SimulateCPU(input);

            HashSet<int> watchCycles = new() { 20, 60, 100, 140, 180, 220 };
            return watchValues.Where(slice => watchCycles.Contains(slice.cycle))
                              .Sum(slice => slice.r.x * slice.cycle);
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