using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day02 : IPuzzle
    {
        public string Name => "2022-02";

        enum RPS
        {
            Rock,
            Paper,
            Scissors
        }

        enum Result
        {
            Win,
            Lose,
            Draw,
        }

        readonly struct Rule
        {
            [Regex("(.) (.)")]
            public Rule(char first, char second) => (Theirs, YoursEncoded) = (DecodeMove(first), second);

            readonly RPS Theirs;
            readonly char YoursEncoded;

            static RPS DecodeMove(char c) => c switch
            {
                'A' or 'X' => RPS.Rock,
                'B' or 'Y' => RPS.Paper,
                'C' or 'Z' => RPS.Scissors,
                _ => throw new ArgumentException("Unexpected char"),
            };

            static Result DecodeResult(char c) => c switch
            {
                'X' => Result.Lose,
                'Y' => Result.Draw,
                'Z' => Result.Win,
                _ => throw new ArgumentException("Unexpected char"),
            };

            readonly static Dictionary<(RPS, RPS), int> scores = new()
            {
                {(RPS.Rock, RPS.Rock), 3 + 1},
                {(RPS.Rock, RPS.Paper), 0 + 1},
                {(RPS.Rock, RPS.Scissors), 6 + 1 },

                {(RPS.Paper, RPS.Rock), 6 + 2},
                {(RPS.Paper, RPS.Paper), 3 + 2},
                {(RPS.Paper, RPS.Scissors), 0 + 2},

                {(RPS.Scissors, RPS.Rock), 0 + 3 },
                {(RPS.Scissors, RPS.Paper), 6 + 3},
                {(RPS.Scissors, RPS.Scissors), 3 + 3},
            };
            readonly static Dictionary<(RPS, Result), RPS> neededMove = new()
            {
                {(RPS.Rock, Result.Lose), RPS.Scissors},
                {(RPS.Rock, Result.Draw), RPS.Rock},
                {(RPS.Rock, Result.Win), RPS.Paper},

                {(RPS.Paper, Result.Lose), RPS.Rock},
                {(RPS.Paper, Result.Draw), RPS.Paper},
                {(RPS.Paper, Result.Win), RPS.Scissors},

                {(RPS.Scissors, Result.Lose), RPS.Paper},
                {(RPS.Scissors, Result.Draw), RPS.Scissors},
                {(RPS.Scissors, Result.Win), RPS.Rock},
            };

            private int CalcScore(RPS yours) => scores[(yours, Theirs)];

            public int ScorePt1() => CalcScore(DecodeMove(YoursEncoded));

            public int ScorePt2() => CalcScore(neededMove[(Theirs, DecodeResult(YoursEncoded))]);
        }

        public static int Part1(string input) => Util.RegexParse<Rule>(input).Sum(r => r.ScorePt1());

        public static int Part2(string input) => Util.RegexParse<Rule>(input).Sum(r => r.ScorePt2());

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}