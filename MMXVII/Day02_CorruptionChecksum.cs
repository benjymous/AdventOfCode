using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVII
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2017-02";} }
 
        public static int RowDifference(string line)
        {
            var data = Util.Parse(line, '\t');
            return data.Max()-data.Min();
        }

        public static int RowMultiple(string line)
        {
            var data = Util.Parse(line, '\t');
            for (var x = 0; x < data.Length; ++x)
            {
                for (var y=0; y<data.Length; ++y)
                {
                    if (x!=y)
                    {
                        if (data[x] % data[y] == 0)
                        {
                            return data[x]/data[y];
                        }
                    }
                }
            }
            throw new Exception("Couldn't find multiple!");
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            return lines.Select(line => RowDifference(line)).Sum();
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input);
            return lines.Select(line => RowMultiple(line)).Sum();
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}