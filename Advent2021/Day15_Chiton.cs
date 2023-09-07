using AoC.Utils;
using AoC.Utils.Pathfinding;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day15 : IPuzzle
    {
        public string Name => "2021-15";

        static int ToKey(int x, int y) => x + (y << 16);

        class Map : IMap<int>
        {
            public Map(string input, QuestionPart part)
            {
                var raw = Util.ParseMatrix<int>(input);
                Data = raw.Entries().ToDictionary(kvp => ToKey(kvp.key.x, kvp.key.y), kvp => kvp.value);

                if (part.One())
                {
                    (RealX, RealY) = (MaxX, MaxY) = (raw.Width() - 1, raw.Height() - 1);
                }
                else
                {
                    (RealX, RealY) = (raw.Width(), raw.Height());
                    (MaxX, MaxY) = ((RealX * 5) - 1, (RealY * 5) - 1);
                }

                for (int x = -1; x <= MaxX + 1; ++x) Data[ToKey(x, -1)] = Data[ToKey(x, MaxY + 1)] = 9999;
                for (int y = -1; y <= MaxY + 1; ++y) Data[ToKey(-1, y)] = Data[ToKey(MaxX + 1, y)] = 9999;
            }

            readonly int MaxX, MaxY, RealX, RealY;
            readonly Dictionary<int, int> Data;

            public IEnumerable<int> GetNeighbours(int location)
            {
                yield return location + 1;
                yield return location + (1 << 16);
                yield return location - 1;
                yield return location - (1 << 16);
            }

            public int GScore(int pos) => Data.GetOrCalculate(pos, pos =>
            {
                var (x, y) = (pos & 0xffff, pos >> 16);
                return ((Data[(x % RealX) + ((y % RealY) << 16)] + (x / RealX + y / RealY) - 1) % 9) + 1;
            });

            public static int Solve(string input, QuestionPart part)
            {
                var map = new Map(input, part);
                return map.FindPath(0, ToKey(map.MaxX, map.MaxY)).Sum(map.GScore);
            }
        }

        public static int Part1(string input)
        {
            return Map.Solve(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Map.Solve(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}