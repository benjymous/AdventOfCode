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
            public Line(int x1, int y1, int x2, int y2) => (p1, p2) = ((x1, y1), (x2, y2));

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

                    yield return pos;

                    do
                    {
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

                        yield return pos;

                    } while (pos != p2); 
                }
            }

            public bool IsAxial => p1.X == p2.X || p1.Y == p2.Y;

            (int X, int Y) p1, p2;
        }

        private static int Solve(string input, QuestionPart part)
        {
            var lines = Util.RegexParse<Line>(input);
            if (part.One()) lines = lines.Where(l => l.IsAxial);

            return lines.Select(line => line.Points)
                        .SelectMany(p => p)
                        .GroupBy(p => p)
                        .Count(g => g.Count() >= 2);
        }

        public static int Part1(string input)
        {
            return Solve(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Solve(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}