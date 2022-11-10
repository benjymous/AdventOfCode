using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;

namespace AoC.Advent2017
{
    public class Day19 : IPuzzle
    {
        public string Name => "2017-19";



        private static (string word, int length) FollowPath(string input)
        {
            var lines = Util.Split(input);
            //var grid = new char[lines[0].Length, lines.Length];

            var grid = new Dictionary<string, char>();

            ManhattanVector2 pos = null;

            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                foreach (var cell in line)
                {
                    grid[$"{x},{y}"] = cell;

                    if (y == 0 && cell != ' ') pos = new ManhattanVector2(x, y);

                    ++x;
                }
                ++y;
            }

            Direction2 dir = new(0, 1);
            string word = "";
            int count = 0;

            while (true)
            {
                //Console.WriteLine($"{pos} - {grid[pos.ToString()]}");
                var next = pos + dir;
                count++;

                var nextCh = grid[next.ToString()];

                switch (nextCh)
                {
                    case ' ':
                        return (word, count);

                    case '|':
                    case '-':
                        break;

                    case '+':
                        // turn a corner
                        var d1 = new Direction2(dir); d1.TurnRight();
                        var d2 = new Direction2(dir); d2.TurnLeft();
                        if (grid[(next + d1).ToString()] != ' ')
                        {
                            dir = d1;
                        }
                        else if (grid[(next + d2).ToString()] != ' ')
                        {
                            dir = d2;
                        }
                        else
                        {
                            Console.WriteLine("Lost!");
                            break;
                        }


                        break;

                    default:
                        word += nextCh;
                        //Console.WriteLine("fail");
                        break;
                }

                pos = next;

            }
        }

        public static string Part1(string input)
        {
            return FollowPath(input).word;
        }

        public static int Part2(string input)
        {
            return FollowPath(input).length;
        }

        public void Run(string input, ILogger logger)
        {

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}