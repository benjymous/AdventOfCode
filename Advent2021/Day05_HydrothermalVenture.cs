using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day05 : IPuzzle
    {
        public string Name { get { return "2021-05"; } }

        struct Line
        {
            [Regex(@"(\d+),(\d+) -> (\d+),(\d+)")]
            public Line (int x1, int y1, int x2, int y2)
            {
                p1 = (x1, y1);
                p2 = (x2, y2);
            }

            public IEnumerable<(int,int)> Points
            {
                get
                {
                    var pos = (p1.X, p1.Y);
  
                    int dx = Math.Abs(p2.X - p1.X);
                    int sx = Math.Sign(p2.X - p1.X);
                    int dy = -Math.Abs(p2.Y - p1.Y);
                    int sy = Math.Sign(p2.Y - p1.Y);

                    int err = dx + dy; 

                    while (true)
                    {
                        yield return pos;

                        if (pos == p2) break;

                        int e2 = 2 * err;
                        if (e2 >= dy)
                        {
                            err += dy;
                            pos.X += sx;
                        }
                        if (e2 <= dx)
                        {
                            err += dx;
                            pos.Y += sy;
                        }
                    }
                }
            }

            public bool IsAxial => p1.X == p2.X || p1.Y == p2.Y;

            (int X, int Y) p1;
            (int X, int Y) p2;
        }

        private static int Solve(string input, Util.QuestionPart part)
        {
            var lines = Util.RegexParse<Line>(input);
            if (part == Util.QuestionPart.Part1) lines = lines.Where(l => l.IsAxial);

            return lines.Select(line => line.Points)
                .SelectMany(p => p)
                .GroupBy(p => p)
                .Count(g => g.Count() >= 2);
        }

        public static int Part1(string input)
        {
            return Solve(input, Util.QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Solve(input, Util.QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}