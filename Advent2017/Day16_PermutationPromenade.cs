using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2017
{
    public class Day16 : IPuzzle
    {
        public string Name { get { return "2017-16"; } }

        private static IEnumerable<string> ParseInput(string input)
        {
            return input.Trim().Split(",").Select(x => x.Replace('/', ' '));
        }

        public static string DoDance(string input, string players)
        {
            return DoDance(ParseInput(input), players);
        }

        public static string DoDance(IEnumerable<string> instructions, string players)
        { 

            char[] order = players.ToArray();
            int orderCount = players.Length;

            foreach (var instr in instructions)
            {
                switch (instr[0])
                {
                    case 's':
                        {
                            var count = Util.ExtractNumbers(instr)[0];
                            var start = order.Take(orderCount - count);
                            var end = order.Skip(orderCount - count).Take(count);
                            order = end.Concat(start).ToArray();
                        }
                        break;
                    case 'x':
                        {
                            var swaps = Util.ExtractNumbers(instr);
                            var c1 = order[swaps[0]];
                            var c2 = order[swaps[1]];
                            order[swaps[1]] = c1;
                            order[swaps[0]] = c2;
                        }
                        break;
                    case 'p':
                        {
                            var c1 = instr[1];
                            var c2 = instr[3];
                            var i1 = Array.IndexOf(order, c1);
                            var i2 = Array.IndexOf(order, c2);
                            order[i1] = c2;
                            order[i2] = c1;
                        }
                        break;
                }
            }

            return order.AsString();
        }

        public static string Part1(string input)
        {
            return DoDance(input, "abcdefghijklmnop");
        }


        public static string Part2(string input)
        {
            long expectedRounds = 1000000000;

            IEnumerable<string> instructions = ParseInput(input);

            var seen = new Dictionary<string, long>();
            Int64 round = 0;
            string order = "abcdefghijklmnop";
            while (!seen.ContainsKey(order))
            {
                seen[order] = round++;
                order = DoDance(instructions, order);
            }
            //Console.WriteLine($"Loop found after {round} rounds");
            Int64 remainder = expectedRounds % round;
            //Console.WriteLine($"So we want {remainder}th value");
            return seen.Where(kvp => kvp.Value == remainder).First().Key;
        
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}