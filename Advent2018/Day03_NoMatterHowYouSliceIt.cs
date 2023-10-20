using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day03 : IPuzzle
    {
        public string Name => "2018-03";

        const int FabricSize = 1000;

        [Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")]
        record struct Shape(string ID, int Left, int Top, int Width, int Height)
        {
            public readonly IEnumerable<int> Squares()
            {
                for (var y = Top; y < Top + Height; ++y)
                {
                    for (var x = Left; x < Left + Width; ++x)
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
            return FindOverlaps(shapes).Count;
        }

        public static string Part2(string input)
        {
            var shapes = Util.RegexParse<Shape>(input);
            var overlaps = FindOverlaps(shapes);

            return shapes.First(shape => !overlaps.Overlaps(shape.Squares())).ID;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
