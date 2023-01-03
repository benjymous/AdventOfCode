using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AoC.Advent2022
{
    public class Day24 : IPuzzle
    {
        public string Name => "2022-24";

        static readonly (int dx, int dy)[] Directions = { (0, -1), (1, 0), (0, 1), (-1, 0), (0, 0) };

        private static int Solve(string input, QuestionPart part)
        {
            var map = Util.ParseSparseMatrix<char>(input);
            var walls = map.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToArray();
            var blizzards = map.Where(kvp => !walls.Contains(kvp.Key) && kvp.Value != '.').Select(kvp => (pos: kvp.Key, dir: new Direction2(kvp.Value))).ToArray();
            (int maxX, int maxY) = (walls.Max(v => v.x), walls.Max(v => v.y));

            var (start, end) = ((pos: (x: 1, y: 0), 0),  (pos: (x: maxX - 1, y: maxY), part.One() ? 0 : 2));
            int step = 0, maxScore = 0;

            (int x, int y)[] destinations = new[] { end.pos, start.pos, end.pos };

            IEnumerable<((int x, int y) pos, int score)> generation = new ((int, int), int)[] { start };
            while (true)
            {
                blizzards = StepBlizzards(blizzards, maxX, maxY);
                var state = walls.Union(blizzards.Select(b => b.pos)).ToHashSet();
                List<((int x, int y) pos, int score)> nextGen = new();
                foreach (var entry in generation)
                {
                    if (entry == end) return step;
                    foreach (var (dx, dy) in Directions)
                    {
                        var test = (pos:(x: entry.pos.x + dx, y: entry.pos.y + dy), entry.score);

                        if (!state.Contains(test.pos) && test.score == maxScore && test.pos.x >= 0 && test.pos.y >= 0 && test.pos.x <= maxX && test.pos.y <= maxY)
                        {
                            if (part.Two() && test.score < 2 && test.pos == destinations[test.score])
                                maxScore = ++test.score;

                            nextGen.Add(test);
                        }
                    }
                }
                generation = nextGen.Where(s => s.score == maxScore).Distinct().OrderBy(e => e.pos.Distance(destinations[maxScore])).Take(50);
                step++;
            }
        }

        private static ((int x, int y) pos, Direction2 dir)[] StepBlizzards(((int x, int y) pos, Direction2 dir)[] blizzards, int maxX, int maxY) => blizzards.Select(b => ((((b.pos.x + b.dir.DX + maxX - 2) % (maxX - 1)) + 1, ((b.pos.y + b.dir.DY + maxY - 2) % (maxY - 1)) + 1), b.dir)).ToArray();

        public static int Part1(string input)
        {
            return Solve(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Solve(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}