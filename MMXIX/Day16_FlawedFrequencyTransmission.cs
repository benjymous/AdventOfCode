using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day16 : IPuzzle
    {
        public string Name { get { return "2019-16";} }

        static int[] initialPattern = new int[] {0,1,0,-1};

        static IEnumerable<int> GetPattern(int element)
        {
            var sequence = Util.DuplicateDigits(Util.RepeatForever(initialPattern), element+1);

            return sequence.Skip(1);
        }

        static string ApplyFFT(string input)
        {
            StringBuilder sb = new StringBuilder();
            var ints = input.Trim().ToCharArray().Select(ch => (Int64)ch-'0').ToArray();

            for (int i=0; i<ints.Length; ++i)
            {
                Int64 outVal = 0;
                var sequence = GetPattern(i).Take(ints.Length).ToArray();
                for (int j=0; j<ints.Length; ++j)
                {
                    outVal += ints[j]*sequence[j];
                }
                var outStr = $"{outVal}";
                sb.Append(outStr.Last());

            }

            return sb.ToString();
        }

        public static string ApplyRepeatedFFT(string initial, int repeats)
        {
            string current = initial;
            for (var i=0; i<repeats; ++i)
            {
                current = ApplyFFT(current);
            }
            return current;
        }
 
        public static string Part1(string input)
        {
            return ApplyRepeatedFFT(input, 100).Substring(0, 8);
        }

        public static int Part2(string input)
        {
            var initial = ApplyRepeatedFFT(input, 100);

            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {

            // console.WriteLine(string.Join(", ", GetPattern(0).Take(10)));
            // console.WriteLine(string.Join(", ", GetPattern(1).Take(10)));
            // console.WriteLine(string.Join(", ", GetPattern(2).Take(10)));
            // console.WriteLine(string.Join(", ", GetPattern(3).Take(10)));

            //console.WriteLine(ApplyRepeatedFFT("12345678", 4));

            // console.WriteLine(Part1("80871224585914546619083218645595")); // 24176176
            // console.WriteLine(Part1("19617804207202209144916044189917")); // 73745418
            // console.WriteLine(Part1("69317163492948606335995924319873")); // 52432133

        

            console.WriteLine("- Pt1 - "+Part1(input));
            // console.WriteLine("- Pt2 - "+Part2(input));

            // 100 - 14735428 x  [ 147354284 x ]
            // 101 - 70262020 x
        }
    }
}