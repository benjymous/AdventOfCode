using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day21 : IPuzzle
    {
        public string Name => "2021-21";

        public class DeterministicDice
        {
            int Current = 1;
            public int Rolls { get; private set; } = 0;
            int Roll()
            {
                var next = Current;
                Rolls++;
                Current = ((Current) % 100) + 1;
                return next;
            }

            public int Roll3() => Roll() + Roll() + Roll();
        }

        static (int roll, int weight)[] weights = new (int roll, int weight)[] {
            (3,1), (4,3), (5,6), (6,7), (7,6), (8,3), (9,1)
        };

        static (long p1Won, long p2Won) DoStep(int p1Pos, int p2Pos, int p1Score, int p2Score, int turn, Dictionary<(int, int, int, int, int), (long,long)> cache)
        {
            if (p1Score >= 21) return (1, 0);
            if (p2Score >= 21) return (0, 1);

            return cache.GetOrCalculate((p1Pos, p2Pos, p1Score, p2Score, turn), _ =>
            {
                long p1Won = 0, p2Won = 0;

                foreach (var (roll, weight) in weights)
                {
                    var res = turn == 0
                        ? DoStep((p1Pos + roll) % 10, p2Pos, (p1Score + (p1Pos + roll) % 10 + 1), p2Score, (turn + 1) % 2, cache)
                        : DoStep(p1Pos, (p2Pos + roll) % 10, p1Score, (p2Score + (p2Pos + roll) % 10 + 1), (turn + 1) % 2, cache);

                    p1Won += res.p1Won * weight;
                    p2Won += res.p2Won * weight;
                }

                return (p1Won, p2Won);
            });
        }

        class Player : Boxed<int>
        {
            [Regex(@"Player . starting position: (\d+)")]
            public Player(int pos) : base(pos) { }
        }

        private static Player[] GetStartPositions(string input) => Util.RegexParse<Player>(input).ToArray();

        public static int Part1(string input)
        {
            var startPositions = GetStartPositions(input);

            var positions = new int[2] { startPositions[0] - 1, startPositions[1] - 1 };
            var scores = new int[2] { 0, 0 };
            int turn = 0;

            var die = new DeterministicDice();
            while (!scores.Any(v => v >= 1000))
            {
                positions[turn] = (positions[turn] + die.Roll3()) % 10;
                scores[turn] += positions[turn] + 1;

                turn = (turn + 1) % 2;
            }

            return scores.Min() * die.Rolls;
        }

        public static long Part2(string input)
        {
            var startPositions = GetStartPositions(input);

            var (p1Won, p2Won) = DoStep(startPositions[0] - 1, startPositions[1] - 1, 0, 0, 0, new());

            return Math.Max(p1Won, p2Won);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}