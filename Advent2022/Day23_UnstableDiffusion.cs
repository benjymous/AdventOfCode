using Advent.Utils.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day23 : IPuzzle
    {
        public string Name => "2022-23";

        static readonly int[] Neighbours = new[] { -1 - 10000, 0 - 10000, 1 - 10000, -1 + 0, 1 + 0, -1 + 10000, 0 + 10000, 1 + 10000 };
        static bool HasNeighbour(HashSet<int> map, int pos)
        {
            for (int i = 0; i < Neighbours.Length; ++i)
                if (map.Contains(pos + Neighbours[i])) return true;
            return false;
        }

        static bool DirectionFree(HashSet<int> map, int pos, int direction, out int newPos)
        {
            newPos = default;
            for (int i = 0; i < 3; ++i)
            {
                var move = pos + CheckDirs[direction, i];
                if (i == 0) newPos = move;
                if (map.Contains(move)) return false;
            }
            return true;
        }

        static readonly int[,] CheckDirs = new int[,]
        {
            { 0 -10000, -1 -10000, 1 -10000 }, // check North
            { 0 + 10000, -1+ 10000, 1+ 10000 }, // check South
            { -1 + 0, -1 -10000, -1 + 10000 }, // check West
            { 1 + 0, 1 -10000, 1 + 10000 } // check East
        };

        private static int RunSimulation(string input, int maxSteps)
        {
            var positions = Util.ParseSparseMatrix<bool>(input).Keys.Select(v => v.x + 100 + (v.y + 100) * 10000).ToHashSet();
            UniqueMap<int, int> potentialMoves = new(positions.Count);

            for (int moveIndex = 0; moveIndex < maxSteps; ++moveIndex)
            {
                foreach (var currentPos in positions)
                {
                    if (!HasNeighbour(positions, currentPos)) continue;
                    for (int i = 0; i < 4; i++)
                    {
                        if (DirectionFree(positions, currentPos, (moveIndex + i) % 4, out var newPos))
                        {
                            potentialMoves.UniqueAdd(newPos, currentPos);
                            break;
                        }
                    }
                }

                if (!potentialMoves.Any()) return moveIndex + 1;

                positions.ExceptWith(potentialMoves.Values);
                positions.UnionWith(potentialMoves.Keys);

                potentialMoves.Reset();
            }

            return CountEmpty(positions.Select(i => (x: i % 10000, y: i / 10000)));
        }

        static int CountEmpty(IEnumerable<(int x, int y)> positions) => ((positions.Max(v => v.x) - positions.Min(v => v.x) + 1) * (positions.Max(v => v.y) - positions.Min(v => v.y) + 1)) - positions.Count();

        public static int Part1(string input)
        {
            return RunSimulation(input, 10);
        }

        public static int Part2(string input)
        {
            return RunSimulation(input, int.MaxValue);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}