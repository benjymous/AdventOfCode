using AoC.Utils;
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
        }

        static int GetKey(int x, int y)
        {
            return x + (y * FabricSize);
        }

        static Dictionary<int, int> FindOverlaps(IEnumerable<Shape> shapes)
        {
            Dictionary<int, int> square = new();
            foreach (var shape in shapes)
            {
                for (var y = shape.Top; y < shape.Top + shape.H; ++y)
                {
                    for (var x = shape.Left; x < shape.Left + shape.W; ++x)
                    {
                        square.IncrementAtIndex(GetKey(x, y));
                    }
                }
            }

            return square;
        }

        public static int Part1(string input)
        {
            var shapes = Util.RegexParse<Shape>(input);
            var square = FindOverlaps(shapes);

            var count = square.Select(i => i.Value > 1 ? 1 : 0).Sum();

            return count;
        }

        public static string Part2(string input)
        {
            var shapes = Util.RegexParse<Shape>(input);
            var square = FindOverlaps(shapes);

            foreach (var shape in shapes)
            {
                bool fail = false;
                for (var y = shape.Top; y < shape.Top + shape.H; ++y)
                {
                    for (var x = shape.Left; x < shape.Left + shape.W; ++x)
                    {
                        if (square[GetKey(x, y)] > 1)
                        {
                            fail = true;
                        }
                    }
                }
                if (!fail)
                {
                    return shape.ID;
                }
            }

            return "";
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
