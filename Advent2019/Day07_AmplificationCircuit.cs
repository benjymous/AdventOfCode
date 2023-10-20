using AoC.Advent2019.NPSA;
using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day07 : IPuzzle
    {
        public string Name => "2019-07";

        public static long RunAmplifiers01(string program, IEnumerable<int> inputs)
        {
            long signal = 0;

            foreach (var phase in inputs)
            {
                var cpu = new IntCPU(program);
                cpu.AddInput(phase, signal);
                cpu.Run();
                signal = cpu.Output.First();
            }

            return signal;
        }

        public static long RunAmplifiers02(string program, IEnumerable<int> inputs)
        {
            var cpus = inputs.Select(phase => new IntCPU(program) { Input = new() { phase } }).ToArray();

            var current = 0;

            long signal = 0, output = 0;

            while (true)
            {
                cpus[current].AddInput(signal);
                while (cpus[current].Output.Count == 0)
                {
                    if (!cpus[current].Step()) return output;
                }
                signal = cpus[current].Output.Dequeue();

                if (current == cpus.Length - 1) output = signal;

                current = (current + 1) % cpus.Length;
            }
        }

        public static long Part1(string input)
        {
            return Enumerable.Range(0, 5).Permutations().AsParallel().Max(set => RunAmplifiers01(input, set));
        }

        public static long Part2(string input)
        {
            return Enumerable.Range(5, 5).Permutations().AsParallel().Max(set => RunAmplifiers02(input, set));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}