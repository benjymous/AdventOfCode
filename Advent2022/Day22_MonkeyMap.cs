using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day22 : IPuzzle
    {
        public string Name => "2022-22";

        static IEnumerable<string> GetInstructions(string tape)
        {
            var current = "";
            foreach (var c in tape)
            {
                if (c >= '0' && c <='9')
                {
                    current += c;
                }
                else
                {
                    if (current != "") yield return current;
                    yield return c.ToString();
                    current = "";
                }
            }
            if (current != "") yield return current;
        }

        public static int Part1(string input)
        {
            var bits = input.Split("\n\n");
            var map = Util.ParseSparseMatrix<char>(bits[0]).Where(kvp => kvp.Value != ' ').ToDictionary();
            var tape = GetInstructions(bits[1]);

            var maxX = map.Max(kvp => kvp.Key.x);
            var maxY = map.Max(kvp => kvp.Key.y);

            Dictionary<int, int> rowMin = new(), rowMax = new(), colMin = new(), colMax = new(); 

            for (int y=0; y<=maxY; ++y)
            {
                rowMin[y] = map.Where(kvp => kvp.Key.y == y).Min(kvp => kvp.Key.x);
                rowMax[y] = map.Where(kvp => kvp.Key.y == y).Max(kvp => kvp.Key.x);
            }
            for (int x=0; x<=maxX; ++x)
            {
                colMin[x] = map.Where(kvp => kvp.Key.x == x).Min(kvp => kvp.Key.y);
                colMax[x] = map.Where(kvp => kvp.Key.x == x).Min(kvp => kvp.Key.y);
            }

            var xStart = rowMin[0];

            ManhattanVector2 pos = new(xStart, 0);
            Direction2 dir = new(1, 0);

            foreach (var instr in tape)
            {
                if (instr == "L") dir.TurnLeft();
                else if (instr == "R") dir.TurnRight();
                else
                {
                    var steps = int.Parse(instr);
                    for (int i=0; i<steps; ++i)
                    {
                        var next = pos + dir;
                        if (!map.ContainsKey(next))
                        {
                            if (next.Y >= 0 && next.Y <= maxY)
                            {
                                if (next.X > rowMax[next.Y]) next.X = rowMin[next.Y];
                                else if (next.X < rowMin[next.Y]) next.X = rowMax[next.Y];
                            }
                            else if (next.X >=0 && next.X <=maxX)
                            {
                                if (next.Y > colMax[next.X]) next.Y = colMin[next.X];
                                else if (next.Y < colMin[next.Y]) next.Y = colMax[next.X];
                            }
                            else
                                Console.WriteLine("!!");
                        }
   
                        var c = map[next];
                        if (c == '#') break;
                        
                        pos = next;
                    }
                }
            }

            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5".Replace("\r", "");

            Console.WriteLine(Part1(test));


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}