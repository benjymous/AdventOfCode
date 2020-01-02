using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2015-25";} }

        static IEnumerable<(int x, int y, Int64 code)> NumberSequence()
        {
            int x=1;
            int y=1;
            Int64 code = 20151125;
            yield return (x,y,code);

            while (true)
            {
                y--;
                if (y==0)
                {
                    y=x+1;
                    x=0;
                }
                x++;
                code *= 252533;
                code %= 33554393;
                yield return (x,y,code);
            }
        }
 
        public static int Part1(string input)
        {
            var bits = input.Where(c => (c==' ' || (c>='0' && c<='9'))).AsString().Trim().Split(" ").Where(w => !string.IsNullOrEmpty(w)).ToArray();

            var row = int.Parse(bits[0]);
            var col = int.Parse(bits[1]);

            var code = NumberSequence().Where(val => val.x == col && val.y == row).First().code;

            return (int)code;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
        }
    }
}