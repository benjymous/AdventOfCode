using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2016-25";} }

        const int SAMPLE_SIZE = 10;

        class Outputter : BunniTek.IOutput
        {
            public List<int> values = new List<int>();
            public bool Put(int i)
            {
                values.Add(i);
                return values.Count < SAMPLE_SIZE;
            }
        }

        public static IEnumerable<int> Signal(string program, int input)
        {
            var cpu = new BunniTek.BunnyCPU(program);
            cpu.Set(BunniTek.RegisterId.a, input);
            var Outputter = new Outputter();
            cpu.Output = Outputter;
            cpu.Run();
            return Outputter.values;
        }

        public static IEnumerable<int> Expected()
        {
            for (int i=0; i<SAMPLE_SIZE/2; ++i)
            {
                yield return 0;
                yield return 1;
            }
        }

        public static int[] expected = Expected().ToArray();

        public static bool Test(string program, int input)
        {
            var signal = Signal(program, input).ToArray(); 
            for (int i=0; i<SAMPLE_SIZE; ++i)
            {
                if (signal[i] != expected[i]) return false;
            }
            return true;
        }
 
        public static int Part1(string input)
        {
            int blockSize = 50;

            int start = 0;
            while (true)
            {
                var res = Enumerable.Range(start, blockSize).AsParallel().Where(i => Test(input,i));
                if (res.Any())
                {
                    return res.Min();
                }
                start += blockSize;
            }

        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
        }
    }
}