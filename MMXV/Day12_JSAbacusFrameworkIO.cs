using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day12 : IPuzzle
    {
        public string Name { get { return "2015-12";} }

        public static IEnumerable<int> FindNumbers(string input)
        {
            StringBuilder current = new StringBuilder();
            for (int i=0; i<input.Length; ++i)
            {
                if (input[i]=='-' || (input[i]>='0' && input[i]<='9'))
                {
                    current.Append(input[i]);
                }
                else
                {
                    if (current.Length>0)
                    {
                        yield return int.Parse(current.ToString());
                        current = new StringBuilder();
                    }
                }
            }
            if (current.Length>0)
            {
                yield return int.Parse(current.ToString());
            }
        }
 
        public static int Part1(string input)
        {
            return FindNumbers(input).Sum();
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {

            // var data = FindNumbers(input);
            // foreach (var i in data)
            // {
            //     console.WriteLine(i);
            // }

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}