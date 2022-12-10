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

        private static IEnumerable<(int cycle, int x)> Simulate(string input)
        {
            var instructions = Util.Split(input);

            int cycle = 1;
            int x = 1;
            foreach (var instr in instructions)
            {
                yield return(cycle, x);
                if (instr == "noop")
                {
                    cycle++;
                }
                else
                {
                    yield return (cycle + 1, x);
                    var bits = instr.Split(' ');
                    x += int.Parse(bits[1]);
                    cycle += 2;
                }
            }
        }

        public static int Part1(string input)
        {
            var watchValues = Simulate(input);

            List<int> watchCycles = new() { 20, 60, 100, 140, 180, 220 };
            return watchValues.Where(kvp => watchCycles.Contains(kvp.cycle)).Select(kvp => kvp.x * kvp.cycle).Sum();
        }

        public static string Part2(string input, ILogger logger)
        {
            var watchValues = Simulate(input);

            StringBuilder crt = new();

            foreach (var val in watchValues.Take(240))
            {
                int j = val.cycle % 40;
                var check = val.x;
                if (j >= check && j <= check + 2)
                    crt.Append('#');
                else
                    crt.Append('.');

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