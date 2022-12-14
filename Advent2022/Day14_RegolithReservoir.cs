using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day14 : IPuzzle
    {
        public string Name => "2022-14";

        (int dx, int dy)[] moves = new[] { (0, 1), (-1, 1), (1, 1)};

        static (bool canMove, (int x, int y) pos) FindStep(Dictionary<(int x, int y), char> map, (int x, int y) pos)
        {
            foreach (var move in moves)
            {
                var next = (pos.x + move.dx, pos.y + move.dy);
                if (CanMove(map, next)) return (true, next);
            }
            return (false, (0, 0));
        }

        static bool CanMove(Dictionary<(int x, int y), char> map, (int x, int y) pos)
        {
            return !map.ContainsKey(pos);
        }

        public static int Part1(string input)
        {
            Dictionary<(int x, int y), char> map = new();
            var lines = Util.Split(input, '\n');
            foreach (var line in lines)
            {
                var pairs = Util.Parse<ManhattanVector2>(line.Split(" -> ")).OverlappingPairs();
                foreach (var pair in pairs) 
                {
                    var dx = Math.Sign(pair.second.X - pair.first.X);
                    var dy = Math.Sign(pair.second.Y - pair.first.Y);

                    var pos = pair.first.AsSimple();
                    var end = pair.second.AsSimple();

                    while (pos != end)
                    {
                        map[pos] = '#';
                        pos.x += dx;
                        pos.y += dy;
                    }
                    map[pos] = '#';
                }
            }

            int maxY = map.Max(kvp => kvp.Key.y);

            (int x, int y) sandPos = (500, 0);
            while (true)
            {
                while (true)
                {
                    var move = FindStep(map, sandPos);
                    if (!move.canMove)
                    {
                        map[sandPos] = 'o';
                        break;
                    }


                }

                if (sandPos.y >= maxY) break;
                
            }

            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {

            string test = "498,4 -> 498,6 -> 496,6\n503,4 -> 502,4 -> 502,9 -> 494,9";

            Console.WriteLine(Part1(test));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}