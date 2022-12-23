using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2022
{
    public class Day23 : IPuzzle
    {
        public string Name => "2022-23";

        static bool HasNeighbour(HashSet<(int x, int y)> map, (int x, int y) pos)
        {
            for (int y = pos.y - 1; y <= pos.y + 1; ++y)
            {
                for (int x = pos.x - 1; x <= pos.x + 1; ++x)
                {
                    if ((x != pos.x || y != pos.y) && map.Contains((x, y))) return true;
                }
            }
            return false;
        }

        static bool DirectionFree(HashSet<(int x, int y)> map, (int x, int y) pos, int direction)
        {
            for (int i=0; i<3; ++i)
            {
                var check = CheckDirs[direction, i];
                if (map.Contains((pos.x + check.x, pos.y + check.y))) return false;
            }
            return true;
        }


        static (int x, int y)[,] CheckDirs = new (int x, int y)[,]
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

                //Console.WriteLine(moveIndex);
                //Console.WriteLine(Display(positions));
                //Console.WriteLine();

                foreach (var key in positions)
                {
                    if (HasNeighbour(positions, key))
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            int move = (moveIndex + i) % 4;
                            if (DirectionFree(positions, key, move))
                            {
                                var delta = CheckDirs[move, 0];
                                var proposal = (key.x + delta.x, key.y + delta.y);
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

                if (moveIndex == maxSteps)
                {
                    //Console.WriteLine(Display(positions));
                    //Console.WriteLine();
                    return CountEmpty(positions);
                }

            }

            //Console.WriteLine(Display(positions));

            return moveIndex;
        }

        static string Display(HashSet<(int x, int y)> dots)
        {
            StringBuilder sb = new();
            sb.AppendLine();

            var minX = dots.Min(v => v.x);
            var minY = dots.Min(v => v.y);

            var maxX = dots.Max(v => v.x);
            var maxY = dots.Max(v => v.y);

            for (int y = minY-2; y <= maxY+2; ++y)
            {
                for (int x = minX-2; x <= maxX+2; ++x)
                {
                    sb.Append(dots.Contains((x, y)) ? "#" : ".");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }


        static int CountEmpty(HashSet<(int x, int y)> dots)
        {

            var minX = dots.Min(v => v.x);
            var minY = dots.Min(v => v.y);

            var maxX = dots.Max(v => v.x);
            var maxY = dots.Max(v => v.y);

            int empty = 0;

            for (int y = minY ; y <= maxY ; ++y)
            {
                for (int x = minX; x <= maxX; ++x)
                {
                    if (!dots.Contains((x, y))) empty++;
                }
            }

            return empty;
        }

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

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}

// 4414 too high