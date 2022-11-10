using AoC.Utils;
using AoC.Utils.Pathfinding;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day15 : IPuzzle
    {
        public string Name => "2021-15";

        class Map : IMap<(int x, int y)>
        {
            public Map(string input, QuestionPart part)
            {
                Data = Util.ParseSparseMatrix<byte>(input);

                if (part.One())
                {
                    RealX = MaxX = Data.Keys.Max(c => c.x);
                    RealY = MaxY = Data.Keys.Max(c => c.y);
                }
                else
                {
                    RealX = Data.Keys.Max(c => c.x) + 1;
                    RealY = Data.Keys.Max(c => c.y) + 1;
                    MaxX = (RealX * 5) - 1;
                    MaxY = (RealY * 5) - 1;
                }
            }

            public int MaxX { get; private set; }
            public int MaxY { get; private set; }

            readonly int RealX, RealY;
            readonly Dictionary<(int x, int y), byte> Data;

            public IEnumerable<(int x, int y)> GetNeighbours((int x, int y) location)
            {
                if (location.x < MaxX) yield return (location.x + 1, location.y);
                if (location.y < MaxY) yield return (location.x, location.y + 1);
                if (location.x > 0) yield return (location.x - 1, location.y);
                if (location.y > 0) yield return (location.x, location.y - 1);
            }

            public int GScore((int x, int y) pos)
            {
                return Data.GetOrCalculate(pos, pos =>
                {
                    int cellDist = pos.x / RealX + pos.y / RealY;
                    return (byte)(((Data[(pos.x % RealX, pos.y % RealY)] + cellDist - 1) % 9) + 1);
                });
            }
        }

        private static int Solve(Map map) => AStar<(int x, int y)>
                                                 .FindPath(map, (0, 0), (map.MaxX, map.MaxY))
                                                 .Sum(pos => map.GScore(pos));

        public static int Part1(string input)
        {
            return Solve(new Map(input, QuestionPart.Part1));
        }

        public static int Part2(string input)
        {
            return Solve(new Map(input, QuestionPart.Part2));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}