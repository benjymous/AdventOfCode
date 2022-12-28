using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day24 : IPuzzle
    {
        public string Name => "2022-24";

        static readonly (int dx, int dy)[] Directions = { (0, -1), (1, 0), (0, 1), (-1, 0), (0, 0) };

        private static int Solve(string input, QuestionPart part)
        {
            var map = Util.ParseSparseMatrix<char>(input);
            var walls = map.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();
            var blizzards = map.Where(kvp => !walls.Contains(kvp.Key) && kvp.Value != '.').Select(kvp => (pos: kvp.Key, dir: Direction2.FromChar(kvp.Value))).ToArray();
            (int maxX, int maxY) = (walls.Max(v => v.x), walls.Max(v => v.y));

            var (start, end) = ((pos: (x: 1, y: 0), 0),  (pos: (x: maxX - 1, y: maxY), part.One() ? 0 : 2));
            int step = 0, maxScore = 0;

            (int x, int y)[] destinations = new[] { end.pos, start.pos, end.pos };

            IEnumerable<((int x, int y) pos, int score)> generation = new ((int, int), int)[] { start };
            while (true)
            {
                HashSet<(int x, int y)> state = StepState(walls, blizzards, maxX, maxY);
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

        private static HashSet<(int x, int y)> StepState(HashSet<(int x, int y)> walls, ((int x, int y) pos, Direction2 dir)[] blizzards, int maxX, int maxY)
        {
            for (int b = 0; b < blizzards.Length; ++b)
            {
                var bliz = blizzards[b];
                bliz.pos.x += bliz.dir.DX;
                bliz.pos.y += bliz.dir.DY;
                if (bliz.pos.x == maxX) bliz.pos.x = 1;
                if (bliz.pos.y == maxY) bliz.pos.y = 1;
                if (bliz.pos.x == 0) bliz.pos.x = maxX - 1;
                if (bliz.pos.y == 0) bliz.pos.y = maxY - 1;

                blizzards[b] = bliz;
            }
            return walls.Union(blizzards.Select(b => b.pos)).ToHashSet();
        }


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