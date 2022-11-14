using AoC.Utils;
using AoC.Utils.Vectors;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day22 : IPuzzle
    {
        public string Name => "2017-22";

        public static int Part1(string input)
        {
            var data = Util.ParseSparseMatrix<char>(input);

            var width = data.Max(kvp => kvp.Key.x);
            var height = data.Max(kvp => kvp.Key.y);

            var pos = (x: width / 2, y: height / 2);
            var dir = new Direction2(0, 0);
            dir.FaceNorth();

            var grid = data.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

            int infections = 0;

            for (int i = 0; i < 10000; ++i)
            {
                var infected = grid.Contains(pos);
                if (!infected)
                {
                    dir.TurnLeft();
                    grid.Add(pos);
                    infections++;
                }
                else
                {
                    dir.TurnRight();
                    grid.Remove(pos);
                }
                pos.x += dir.DX;
                pos.y += dir.DY;
            }

            return infections;
        }

        public static int Part2(string input)
        {
            var data = Util.ParseSparseMatrix<char>(input);

            var width = data.Max(kvp => kvp.Key.x);
            var height = data.Max(kvp => kvp.Key.y);

            var pos = (x: width / 2, y: height / 2);
            var dir = new Direction2(0, 0);
            dir.FaceNorth();

            var grid = data.Where(kvp => kvp.Value == '#').ToDictionary();

            int infections = 0;

            const char Clean = '.';
            const char Infected = '#';
            const char Weakened = 'W';
            const char Flagged = 'F';

            for (int i = 0; i < 10000000; ++i)
            {
                if (!grid.TryGetValue(pos, out var cell)) cell = Clean;

                switch (cell)
                {
                    case Clean:
                        dir.TurnLeft();
                        grid[pos] = Weakened;
                        break;

                    case Infected:
                        dir.TurnRight();
                        grid[pos] = Flagged;
                        break;

                    case Weakened:
                        grid[pos] = Infected;
                        infections++;
                        break;

                    case Flagged:
                        dir.Turn180();
                        grid.Remove(pos);
                        break;
                }

                pos.x += dir.DX;
                pos.y += dir.DY;
            }

            return infections;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}