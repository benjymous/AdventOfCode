using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day18 : IPuzzle
    {
        public string Name => "2018-18";

        const char OPEN = '.';
        const char TREES = '|';
        const char LUMBERYARD = '#';

        public static char Step(char current, IEnumerable<char> neighbours) => current switch
        {
            // An acre filled with trees will become a lumberyard if three or more adjacent
            // acres were lumberyards. 
            // Otherwise, nothing happens.
            TREES => (neighbours.Count(n => n == LUMBERYARD) >= 3) ? LUMBERYARD : TREES,

            // An acre containing a lumberyard will remain a lumberyard if it was adjacent
            // to at least one other lumberyard and at least one acre containing trees.
            // Otherwise, it becomes open.
            LUMBERYARD => neighbours.Any(n => n == LUMBERYARD) && neighbours.Any(n => n == TREES) ? LUMBERYARD : OPEN,

            // An open acre will become filled with trees if three or more adjacent acres
            // contained trees. 
            // Otherwise, nothing happens.
            _ => (neighbours.Count(n => n == TREES) >= 3) ? TREES : OPEN,
        };

        public static int CalcHash(ref char[,] state) => state.Values().GetCombinedHashCode();

        static int Count(char type, ref char[,] state) => state.Values().Count(c => c == type);

        public static int Run(string input, int iterations)
        {
            var currentState = Util.ParseMatrix<char>(input);
            var (width, height) = currentState.Dimensions();
            var newState = new char[width, height];

            var previous = new Dictionary<int, int>();
            int targetStep = iterations < 100 ? iterations : -1;

            for (var i = 0; i < iterations; ++i)
            {
                if (targetStep == i) break;
                else if (targetStep == -1)
                {
                    var hash = CalcHash(ref currentState);

                    if (previous.TryGetValue(hash, out int idx))
                    {
                        var cycleLength = i - idx;
                        targetStep = cycleLength == 1 ? i + 1 : i + ((iterations - i % cycleLength) % cycleLength);
                    }

                    previous[hash] = i;
                }

                for (var y = 0; y < height; ++y)
                    for (var x = 0; x < width; ++x)
                        newState[x, y] = Step(currentState[x, y], GetNeighbours(currentState, x, y, width, height));

                (currentState, newState) = (newState, currentState);
            }

            return Count(TREES, ref currentState) * Count(LUMBERYARD, ref currentState);
        }

        private static IEnumerable<char> GetNeighbours(char[,] currentState, int x, int y, int width, int height)
        {
            var (minx, miny) = (Math.Max(0, x - 1), Math.Max(0, y - 1));
            var (maxx, maxy) = (Math.Min(height - 1, x + 1), Math.Min(width - 1, y + 1));
            for (var y1 = miny; y1 <= maxy; ++y1)
                for (var x1 = minx; x1 <= maxx; ++x1)
                    if (x != x1 || y != y1) yield return currentState[x1, y1];
        }

        public static int Part1(string input)
        {
            return Run(input, 10);
        }

        public static int Part2(string input)
        {
            return Run(input, 1000000000);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}