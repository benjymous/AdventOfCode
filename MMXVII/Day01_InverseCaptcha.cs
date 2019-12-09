using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVII
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2017-01";} }
 
        public static int Captcha(string input, int offset)
        {
            input = input.Trim();

            int count = 0;
            for (int i=0; i<input.Length; ++i)
            {
                if (input[i]==input[(i+offset)%input.Length])
                {
                    count += int.Parse($"{input[i]}");
                }
            }

            return count;
        }

        public static int Part1(string input)
        {
            return Captcha(input, 1);
        }

        public static int Part2(string input)
        {
            return Captcha(input, input.Length/2);
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}