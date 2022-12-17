using AoC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day17 : IPuzzle
    {
        public string Name => "2022-17";

        readonly static string shapeData = "####|.#.,###,.#.|###,..#,..#|#,#,#,#|##,##";

        static IEnumerable<int> WindSequence(string input) => input.Trim().Forever().Select(c => c == '<' ? -1 : 1);

        public static bool Blocked(HashSet<(int x, int y)> data, (int x, int y) pos)
        {
            return (pos.y <= 0 || pos.x <= 0 || pos.x >= 8 || data.Contains(pos));
        }

        private static bool CheckBlocked(HashSet<(int x, int y)> map, IEnumerable<(int x, int y)> translated, int dx, int dy)
        {
            foreach (var pos in translated)
            {
                if (Blocked(map, (pos.x + dx, pos.y + dy)))
                {
                    return true;
                }
            }
            return false;
        }

        private static void FinishBlock(HashSet<(int x, int y)> map, IEnumerable<(int, int)> translated)
        {
            foreach (var e in translated)
            {
                map.Add(e);
            }
        }

        //public static void Display(HashSet<(int x, int y)> map, HashSet<(int x, int y)> shape)
        //{
        //    return;
        //    var miny = 0;
        //    var maxy = map.Max(pos => pos.y) + 4;
        //    var minx = 1;
        //    var maxx = 7;

        //    for (var y = maxy; y > 0; y--)
        //    {
        //        for (var x = minx; x <= maxx; ++x)
        //        {
        //            if (shape.Contains((x, y)))
        //            {
        //                Console.Write('O');
        //            }
        //            else Console.Write((map.Contains((x, y)) ? '#' : '.'));
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //}

        public static int Part1(string input)
        {
            var wind = WindSequence(input).GetEnumerator();
            wind.MoveNext();

            var shapes = Util.Split(shapeData, '|').Select(part => Util.ParseSparseMatrix<bool>(part).Keys.ToArray()).ToArray();

            HashSet<(int x, int y)> map = new()
            {
                (0, 0)
            };

            int currentShape = 0;

            for (int i = 0; i < 2022; ++i)
            {
                (int x, int y) rockPos = (3, 4 + map.Max(p => p.y));
                while (true)
                {
                    var translated = shapes[currentShape].Select(pos => (x: pos.x + rockPos.x, y: pos.y + rockPos.y));

                    var dx = wind.Pop();
                    if (!CheckBlocked(map, translated, dx, 0))
                    {
                        rockPos.x += dx;
                    }

                    if (CheckBlocked(map, translated, 0, -1))
                    {
                        FinishBlock(map, translated);
                        break;
                    }
                    rockPos.y--;
                }
                currentShape = (currentShape + 1) % 5;
            }

            return map.Max(p => p.y);
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

            Console.WriteLine(Part1(test));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}