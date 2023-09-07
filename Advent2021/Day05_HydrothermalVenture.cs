using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day05 : IPuzzle
    {
        public string Name => "2021-05";

        [Regex(@"(\d+),(\d+) -> (\d+),(\d+)")]
        record struct Line(int X1, int Y1, int X2, int Y2)
        {
            public IEnumerable<(int, int)> Points
            {
                get
                {
                    var (x,y) = (X1, Y1);

                    int dx = Math.Abs(X2 - X1);
                    int sx = Math.Sign(X2 - X1);
                    int dy = -Math.Abs(Y2 - Y1);
                    int sy = Math.Sign(Y2 - Y1);

                    int err = dx + dy;

                    yield return (x, y);

                    do
                    {
                        int e2 = 2 * err;
                        if (e2 >= dy)
                        {
                            err += dy;
                            x += sx;
                        }
                        if (e2 <= dx)
                        {
                            err += dx;
                            y += sy;
                        }

                        yield return (x, y);

                    } while (x != X2 || y != Y2);
                }
            }

            public bool IsAxial => X1 == X2 || Y1 == Y2;
        }

        private static int Solve(string input, QuestionPart part)
        {
            var lines = Util.RegexParse<Line>(input).ToArray();
            if (part.One()) lines = lines.Where(l => l.IsAxial).ToArray();

            return lines.SelectMany(line => line.Points)
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