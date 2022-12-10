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
            public int Stride { get; }
            public void Act(Registers r);
        }

        struct Noop : IInstr
        {
            public int Stride => 1;
            public void Act(Registers r) { }
        }

        record struct Addx(int Val) : IInstr
        {
            public int Stride => 2;
            public void Act(Registers r) => r.x += Val;
        }

        public class Factory
        {
            [Regex("noop")] public static IInstr Noop() => new Noop();
            [Regex("addx (.+)")] public static IInstr Addx(int v) => new Addx(v);
        }

        private static IEnumerable<(int cycle, Registers r)> Simulate(string input)
        {
            var instructions = Util.RegexFactory<IInstr>(input, new Factory()).ToArray();

            int cycle = 1;

            Registers r = new ();
            foreach (var instr in instructions)
            {
                for (int i=0; i<instr.Stride; ++i) yield return(cycle+i, r);
                instr.Act(r);
                cycle += instr.Stride;
            }
        }

        public static int Part1(string input)
        {
            var watchValues = Simulate(input);

            HashSet<int> watchCycles = new() { 20, 60, 100, 140, 180, 220 };
            return watchValues.Where(kvp => watchCycles.Contains(kvp.cycle)).Sum(kvp => kvp.r.x * kvp.cycle);
        }

        public static string Part2(string input, ILogger logger)
        {
            var watchValues = Simulate(input);

            StringBuilder crt = new();

            foreach (var val in watchValues.Take(240))
            {
                int beamPos = val.cycle % 40;
                var spritePos = val.r.x;
                crt.Append((beamPos >= spritePos && beamPos <= spritePos + 2) ? '#' : '.');
                if (val.cycle % 40 == 0) crt.AppendLine();
            }

            logger.WriteLine(Environment.NewLine + crt.ToString());

            return crt.ToString().GetMD5String();
        }

        public void Run(string input, ILogger logger)
        {

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}