using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day23 : IPuzzle
    {
        public string Name => "2022-23";

        static bool HasNeighbour(HashSet<(int x, int y)> map, (int x, int y) pos)
        {
            for (int y = pos.y - 1; y <= pos.y + 1; ++y)
                for (int x = pos.x - 1; x <= pos.x + 1; ++x)
                    if ((x != pos.x || y != pos.y) && map.Contains((x, y))) return true;
 
            return false;
        }

        static bool DirectionFree(HashSet<(int x, int y)> map, (int x, int y) pos, int direction)
        {
            for (int i=0; i<3; ++i)
            {
                var (dx, dy) = CheckDirs[direction, i];
                if (map.Contains((pos.x + dx, pos.y + dy))) return false;
            }
            return true;
        }

        static readonly (int x, int y)[,] CheckDirs = new (int x, int y)[,]
        {
            { (0,-1), (-1,-1), (1,-1) }, // check North
            { (0,1), (-1,1),(1,1) }, // check South
            { (-1,0), (-1,-1), (-1,1) }, // check West
            { (1,0), (1,-1), (1,1) } // check East
        };

        private static int RunSimulation(string input, int maxSteps)
        {
            var positions = Util.ParseSparseMatrix<bool>(input).Keys.ToHashSet();
            int moveIndex = 0;

            while (true)
            {
                Dictionary<(int x, int y), List<(int x, int y)>> potentialMoves = new();

                foreach (var key in positions)
                {
                    if (HasNeighbour(positions, key))
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            int move = (moveIndex + i) % 4;
                            if (DirectionFree(positions, key, move))
                            {
                                var (dx, dy) = CheckDirs[move, 0];
                                var proposal = (key.x + dx, key.y + dy);
                                if (!potentialMoves.ContainsKey(proposal)) potentialMoves[proposal] = new();
                                potentialMoves[proposal].Add(key);
                                break;
                            }
                        }
                    }
                }
                moveIndex++;

                foreach (var proposal in potentialMoves)
                {
                    if (proposal.Value.Count == 1)
                    {
                        positions.Remove(proposal.Value.First());
                        positions.Add(proposal.Key);
                    }
                }
                if (!potentialMoves.Any()) break;

                if (moveIndex == maxSteps) return CountEmpty(positions);
            }

            return moveIndex;
        }

        static int CountEmpty(HashSet<(int x, int y)> dots) => Util.Range2DInclusive(((int, int, int, int))(dots.Min(v => v.y), dots.Max(v => v.y), dots.Min(v => v.x), dots.Max(v => v.x))).Count(pos => !dots.Contains(pos));

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

//            string test1 = ".....\r\n..##.\r\n..#..\r\n.....\r\n..##.\r\n.....";

//            string test2 = @"..............
//..............
//.......#......
//.....###.#....
//...#...#.#....
//....#...##....
//...#.###......
//...##.#.##....
//....#..#......
//..............
//..............
//..............".Replace("\r", "");

//            Console.WriteLine(Part2(test2));
