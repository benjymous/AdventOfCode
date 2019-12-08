using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day08 : IPuzzle
    {
        public string Name { get { return "2019-08";} }
 
        public static IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> source, int grouping)
        {
            return  source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / grouping)
                .Select(x => x.Select(v => v.Value));
        }

        public static int Part1(string input)
        {
            var chars = input.Trim().ToCharArray();

            var groups = Split(chars, 25*6);

            int leastZeros = int.MaxValue;
            int onesByTwos = 0;

            foreach (var g in groups)
            {
                var zeroes = g.Where(i => i=='0').Count();

                if (zeroes < leastZeros)
                {
                    leastZeros = zeroes;
                    onesByTwos = g.Where(i => i=='1').Count() * g.Where(i => i=='2').Count();
                }
            }

            return onesByTwos;
        }

        public static int Part2(string input)
        {

            var chars = input.Trim().ToCharArray();

            var groups = Split(chars, 25*6);

            var output = new List<char>();

            foreach (var layer in groups.Reverse())
            {
                if (output.Count == 0)
                {
                    output.AddRange(layer);
                }
                else
                {
                    var l = layer.ToArray();
                    for (var i=0; i<l.Length; ++i)
                    {
                        if (l[i]!='2')
                        {
                            output[i]=l[i];
                        }
                    }
                }
            }

            var final = Split(output.Select(c => c=='1' ? "#" : " "), 25);
            foreach (var l in final)
            {
                Console.WriteLine(string.Join("", l));
            }

            return 0;
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}