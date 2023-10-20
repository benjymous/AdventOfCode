using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day24 : IPuzzle
    {
        public string Name => "2022-24";

        static int ToKey((int x, int y) v) => v.x + (v.y * 1000);
        static int ToKey(char dir) => dir switch { '>' => 1, '<' => -1, '^' => -1000, 'v' => 1000, _ => 0 };

        static readonly int[] Directions = { -1000, 1, 1000, -1, 0 };

        static int Distance(int v) => Math.Abs(v % 1000) + Math.Abs(v / 1000);

        private static int Solve(string input, QuestionPart part)
        {
            var map = Util.ParseSparseMatrix<char>(input);

            (int w, int h) = (map.Keys.Max(v => v.x) - 1, map.Keys.Max(v => v.y) - 1);

            var walls = map.Where(kvp => kvp.Value == '#').Select(kvp => ToKey(kvp.Key)).Append(ToKey((1, -1))).Append(ToKey((w, h + 2))).ToHashSet();
            var blizzards = map.Where(kvp => kvp.Value != '#' && kvp.Value != '.').Select(kvp => (pos: ToKey(kvp.Key), dir: ToKey(kvp.Value))).ToArray();

            var blizH = blizzards.Where(b => b.dir == -1 || b.dir == 1).ToArray();
            var blizV = blizzards.Except(blizH).ToArray();

            HashSet<int>[] blizStepsH = new HashSet<int>[w + 1], blizStepsV = new HashSet<int>[h + 1];

            for (int i = 0; i <= Math.Max(w, h); ++i)
            {
                if (i <= w) blizStepsH[i] = StepBlizzards(blizH, w, h);
                if (i <= h) blizStepsV[i] = StepBlizzards(blizV, w, h);
            }

            var (start, end) = (ToKey((1, 0)), ToKey((w, h + 1)));
            int currentWaypoint = 0, targetWaypoint = part.One() ? 0 : 2;
            int[] waypoints = new[] { end, start, end };

            var generation = new[] { start };
            HashSet<int> nextGen = new(100);

            for (int step = 0; ; ++step)
            {
                foreach (var newPos in generation.SelectMany(p => Directions.Select(dir => p + dir)))
                {
                    if (newPos == waypoints[currentWaypoint])
                    {
                        if (++currentWaypoint > targetWaypoint) return step + 1;
                        nextGen.Clear();
                        nextGen.Add(newPos);
                        break;
                    }

                    if (blizStepsH[step % w].Contains(newPos) || blizStepsV[step % h].Contains(newPos) || walls.Contains(newPos)) continue;

                    nextGen.Add(newPos);
                }

                generation = nextGen.OrderBy(e => Distance(e - waypoints[currentWaypoint])).Take(50).ToArray();
                nextGen.Clear();
            }
        }

        private static HashSet<int> StepBlizzards((int pos, int dir)[] blizzards, int w, int h)
        {
            for (int i = 0; i < blizzards.Length; ++i)
            {
                var pos = blizzards[i].pos + blizzards[i].dir;
                blizzards[i].pos = ((pos % 1000 + w - 1) % w) + 1 + ((((pos / 1000 + h - 1) % h) + 1) * 1000);
            }
            return blizzards.Select(b => b.pos).ToHashSet();
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