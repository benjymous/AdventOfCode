using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day20 : IPuzzle
    {
        public string Name => "2021-20";

        static (int minY, int maxY, int minX, int maxX) GetRange(IEnumerable<(int x, int y)> input, int margin) 
            => (minY: input.Min(k => k.y) - margin,
                maxY: input.Max(k => k.y) + margin,
                minX: input.Min(k => k.x) - margin,
                maxX: input.Max(k => k.x) + margin);

        static IEnumerable<(int x, int y)> Step(HashSet<(int x, int y)> input, bool[] rules, int margin) 
            => Util.Range2DInclusive(GetRange(input, margin))
                   .Where(pos => rules[GetRuleIndex(input, pos.x, pos.y)]);

        static int GetRuleIndex(HashSet<(int x, int y)> input, int x, int y)
        {
            int result = 0;
            for (int y1 = y - 1; y1 <= y + 1; ++y1)
            {
                for (int x1 = x - 1; x1 <= x + 1; ++x1)
                {
                    result = (result << 1) + (input.Contains((x1, y1)) ? 1 : 0);
                }
            }
            return result;
        }

        public static int Simulate(string input, int steps, int margin = 1, bool crop = false)
        {
            var bits = input.Split("\n\n");
            var rules = bits[0].Select(b => b == '#').ToArray();
            var map = Util.ParseSparseMatrix<bool>(bits[1]).Keys.ToHashSet();

            var (minY, maxY, minX, maxX) = GetRange(map, steps);

            for (int i = 0; i < steps; i++)
            {
                var next = Step(map, rules, margin);

                if (crop && (i % 2) == 1) // Pretend that infinite mess never happened!
                {
                    next = next.Where(key => key.x >= minX && key.x <= maxX && key.y >= minY && key.y <= maxY);
                }
                map = next.ToHashSet();
            }

            return map.Count;
        }

        public static int Part1(string input)
        {
            return Simulate(input, steps: 2, margin: 4, crop: true);
        }

        public static int Part2(string input)
        {
            return Simulate(input, steps: 50, margin: 75, crop: true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}