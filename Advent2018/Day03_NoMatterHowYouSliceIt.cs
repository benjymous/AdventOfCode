using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day03 : IPuzzle
    {
        public string Name => "2018-03";

        const int FabricSize = 1000;

        struct Shape
        {
            public string ID;
            public int Left;
            public int Top;
            public int W;
            public int H;

            [Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")]
            public Shape(string id, int l, int t, int w, int h)
            {
                ID = id;
                Left = l;
                Top = t;
                W = w;
                H = h;
            }

            public IEnumerable<int> Squares()
            {
                for (var y = Top; y < Top + H; ++y)
                {
                    for (var x = Left; x < Left + W; ++x)
                    {
                        yield return GetKey(x, y);
                    }
                }
            }
        }

        static int GetKey(int x, int y)
        {
            return x + (y * FabricSize);
        }

        static HashSet<int> FindOverlaps(IEnumerable<Shape> shapes)
        {
            Dictionary<int, int> square = new();
            foreach (var shape in shapes)
            {
                foreach (var squareId in shape.Squares())
                {
                    square.IncrementAtIndex(squareId);
                }
            }

            return square.Where(kvp => kvp.Value > 1).Select(kvp => kvp.Key).ToHashSet();
        }

        public static int Part1(string input)
        {
            var shapes = Util.RegexParse<Shape>(input);
            return FindOverlaps(shapes).Count();
        }

        public static string Part2(string input)
        {
            var shapes = Util.RegexParse<Shape>(input);
            var overlaps = FindOverlaps(shapes);

            return shapes.First(shape => !shape.Squares().Intersect(overlaps).Any()).ID;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
