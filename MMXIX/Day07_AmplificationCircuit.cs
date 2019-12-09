using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day07 : IPuzzle
    {
        public string Name { get { return "2019-07";} }
 
        public static Int64 RunAmplifiers01(string program, IEnumerable<int> inputs)
        {
            Int64 signal = 0;

            foreach (var phase in inputs)
            {
                var cpu = new NPSA.IntCPU(program);
                cpu.Input.Enqueue(phase);
                cpu.Input.Enqueue(signal);
                cpu.Run();
                signal = cpu.Output.First();
            }

            return signal;
        }

        public static Int64 RunAmplifiers02(string program, IEnumerable<int> inputs)
        {
            Int64 signal = 0;

            var cpus = new List<NPSA.IntCPU>();

            foreach (var phase in inputs)
            {
                var cpu = new NPSA.IntCPU(program);
                cpu.Input.Enqueue(phase);

                cpus.Add(cpu);
            }

            var current = 0;

            int steps = 0;

            Int64 output = 0;
            
            while (true)
            {
                bool running = true;
                cpus[current].Input.Enqueue(signal);
                while (running && cpus[current].Output.Count==0)
                {
                    running = cpus[current].Step();
                }
                if (!running)
                {
                    return output;
                }
                signal = cpus[current].Output.Dequeue();

                if (current == cpus.Count-1)
                {
                    output = signal;
                }

                current = (current+1) % cpus.Count;
                steps++;
            }
        }       

        public static Int64 Part1(string input)
        {
            var permutations = Permutations.Get(Enumerable.Range(0, 5));

            return permutations.Select(set => RunAmplifiers01(input, set)).Max();
        }

        public static Int64 Part2(string input)
        {
            var permutations = Permutations.Get(Enumerable.Range(5, 5));
          
            return permutations.Select(set => RunAmplifiers02(input, set)).Max();
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}