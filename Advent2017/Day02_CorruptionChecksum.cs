using System;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day02 : IPuzzle
    {
        public string Name => "2017-02";

        public static int RowDifference(string line)
        {
            var data = Util.Parse32(line, '\t');
            return data.Max() - data.Min();
        }

        public static int RowMultiple(string line)
        {
            var data = Util.Parse32(line, '\t');
            for (var x = 0; x < data.Length; ++x)
            {
                for (var y = 0; y < data.Length; ++y)
                {
                    if (x != y)
                    {
                        if (data[x] % data[y] == 0)
                        {
                            return data[x] / data[y];
                        }
                    }
                }
            }
            throw new Exception("Couldn't find multiple!");
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            return lines.Sum(RowDifference);
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input);
            return lines.Sum(RowMultiple);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}