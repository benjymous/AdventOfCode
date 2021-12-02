using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2021-02"; } }

        static IEnumerable<(char cmd,int dist)> Parse(string input)
        {
            var lines = input.Trim().Split("\n");
            foreach (var line in lines)
            {
                var bits = line.Split(" ");
                int dist = int.Parse(bits[1]);
                yield return (bits[0][0], dist);
            }
        }

        public static int Part1(string input)
        {
            var data = Parse(input);

            int x = 0;
            int y = 0;

            foreach (var line in data)
            {
                switch (line.cmd)
                {
                    case 'f': x+= line.dist; break;
                    case 'u': y-= line.dist; break;
                    case 'd': y+= line.dist; break;

                }
            }

            return x*y;
        }

        public static int Part2(string input)
        {
            var data = Parse(input);

            int x = 0;
            int y = 0;
            int aim = 0;

            foreach (var line in data)
            {
                switch (line.cmd)
                {
                    case 'f': x += line.dist; y += (aim * line.dist); break;
                    case 'u': aim -= line.dist; break;
                    case 'd': aim += line.dist; break;
                }
            }

            return x * y;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}