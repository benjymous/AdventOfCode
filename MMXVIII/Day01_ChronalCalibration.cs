using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2018-01";} }

        public static int Part1(string input)
        {
            var parts = Util.Split(input);

            int sum = 0;

            foreach (var part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    sum += Int32.Parse(part.Trim());
                }
            }

            return sum;
        }

        public static int Part2(string input)
        {
            var seen = new HashSet<int>();

            var parts = Util.Split(input);

            int freq = 0;

            while (true)
            {

                foreach (var part in parts)
                {
                    if (!string.IsNullOrWhiteSpace(part))
                    {
                        seen.Add(freq);

                        freq += Int32.Parse(part.Trim());

                        if (seen.Contains(freq))
                        {
                            return freq;
                        }                        
                    }
                }
            }
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
