using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils;

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
        // based on algorithm here:
        // https://github.com/FirescuOvidiu/Advent-of-Code-2019/blob/master/Day%2016/day16-part2/day16-part2.cpp
        static IEnumerable<int> ProcessSignal(int[] sequence)
        {
            var newSequence = Enumerable.Repeat(0, sequence.Length).ToArray();
            int sizeSequence = sequence.Length;
            int sum = 0;
            int phase = 0;

            while (phase<100)
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
            var initial = string.Join("", Enumerable.Repeat(input.Trim(), 10000)).Select(c => (int)c);

            var signal = ProcessSignal(initial.ToArray());

            int messageOffset = int.Parse(input.Substring(0, 7));

            var outStr = signal.Skip(messageOffset).Take(8).Select(c => (char)('0'+c)).ToArray().AsString();

            return outStr;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}