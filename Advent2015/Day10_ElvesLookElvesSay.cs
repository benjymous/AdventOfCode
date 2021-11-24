using System;
using System.Text;

namespace AoC.Advent2015
{
    public class Day10 : IPuzzle
    {
        public string Name { get { return "2015-10"; } }

        public static string SayIt(string input)
        {
            StringBuilder builder = new StringBuilder();
            int run = 0;
            for (int i = 0; i < input.Length; i += run)
            {
                run = 0;

                while ((run + i < input.Length) && (input[i] == input[i + run]))
                {
                    run++;
                }
                run = Math.Max(1, run);

                builder.Append($"{run}{input[i]}");

            }

            //Console.WriteLine($"{input} - {outStr}");

            return builder.ToString();
        }

        public static int GetNth(string input, int iterations)
        {

            var result = input.Trim();

            for (int i = 0; i < iterations; ++i)
            {
                result = SayIt(result);
            }

            return result.Length;
        }

        public static int Part1(string input)
        {
            return GetNth(input, 40);
        }

        public static int Part2(string input)
        {
            return GetNth(input, 50);
        }

        public void Run(string input, ILogger logger)
        {
            //SayIt("1"); //11
            //SayIt("11"); //21
            //SayIt("21"); //1211
            //SayIt("1211"); //111221
            //SayIt("111221"); //312211

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}