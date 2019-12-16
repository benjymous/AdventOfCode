using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day16 : IPuzzle
    {
        public string Name { get { return "2019-16";} }

        static int[] initialPattern = new int[] {0, 1, 0, -1};

        public static int SequenceAt(int iter, int pos) => initialPattern[((pos+1)/(iter+1)%4)];

        static int[] ApplyFFT(int[] input)
        {          
            var output = new List<int>();

            for (int i=0; i<input.Length; ++i)
            {
                int outVal = 0;
                for (int j=i; j<input.Length; ++j)
                {
                    outVal += input[j]*SequenceAt(i, j);
                }

                var outDigit = Math.Abs(outVal) % 10;
                output.Add(outDigit);
            }

            return output.ToArray();
        }

        public static string ApplyRepeatedFFT(string initial, int repeats)
        {
            var current = initial.Trim().ToCharArray().Select(ch => (int)ch-'0').ToArray();
            for (var i=0; i<repeats; ++i)
            {
                current = ApplyFFT(current);
            }
            return string.Join("", current.Select(i => $"{i}"));
        }
 
        public static string Part1(string input)
        {
            return ApplyRepeatedFFT(input, 100).Substring(0, 8);
        }

        // Thanks to FirescuOvidiu
        // All 1s in second half, past leading zeroes
        static List<int> ProcessSignal(List<int> sequence)
        {

            List<int> newSequence = Enumerable.Repeat(0, sequence.Count).ToList();
            int sizeSequence = sequence.Count;
            int sum = 0;
            int phase = 0;

            while(phase<100)
            {
                sum = 0;
                for (int position = sizeSequence - 1; position >= sizeSequence / 2; position--)
                {
                    sum += sequence[position];
                    newSequence[position] = sum % 10;
                }
                sequence = newSequence;
                phase++;
            }

            return sequence;
        }

        public static string Part2(string input)
        {
            var initial = string.Join("", Enumerable.Repeat(input.Trim(), 10000)).Select(c => (int)c).ToList();

            var signal = ProcessSignal(initial);

            int messageOffset = int.Parse(input.Substring(0, 7));

            var outStr = signal.Skip(messageOffset).Take(8).Select(c => (char)('0'+c)).ToArray().AsString();

            return outStr;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            // for (int i=0; i<1000; ++i)
            // {
            //     TestSequence(i);
            // }

            //Console.WriteLine(Part1("80871224585914546619083218645595")); // 24176176

            //Console.WriteLine("98765432109876543210".Skip(7).Take(8).AsString());

            Console.WriteLine(Part2("03036732577212944063491565474664")); // 84462026
            Console.WriteLine(Part2("02935109699940807407585447034323")); // 78725270
            Console.WriteLine(Part2("03081770884921959731165446850517")); // 53553731

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}