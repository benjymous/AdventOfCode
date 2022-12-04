﻿using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day02 : IPuzzle
    {
        public string Name => "2022-02";

        struct MoveP1
        {
            internal const char Rock = 'A', Paper = 'B', Scissors = 'C';
        }
        struct MoveP2
        {
            internal const char Rock = 'X', Paper = 'Y', Scissors = 'Z';
        }

        struct Result
        {
            internal const char Lose = 'X', Draw = 'Y', Win = 'Z';
        }

        readonly struct Rule 
        {
            [Regex("(.) (.)")]
            public Rule(char theirs, char yours) => (Theirs, Yours) = (theirs, yours);

            readonly char Theirs;
            readonly char Yours;

            static int Score(char theirs, char yours ) => (theirs, yours) switch
            {
                (MoveP1.Rock, MoveP2.Rock ) => 3 + 1,
                (MoveP1.Paper, MoveP2.Rock ) => 0 + 1,
                (MoveP1.Scissors, MoveP2.Rock ) => 6 + 1,

                (MoveP1.Rock, MoveP2.Paper ) => 6 + 2,
                (MoveP1.Paper, MoveP2.Paper ) => 3 + 2,
                (MoveP1.Scissors, MoveP2.Paper ) => 0 + 2,

                (MoveP1.Rock, MoveP2.Scissors ) => 0 + 3,
                (MoveP1.Paper, MoveP2.Scissors ) => 6 + 3,
                (MoveP1.Scissors, MoveP2.Scissors ) => 3 + 3,

                _ => throw new ArgumentException("Invalid combination")
            };

            static char GetMove(char theirs, char neededResult) => (theirs, neededResult) switch
            {
                (MoveP1.Rock, Result.Lose) => MoveP2.Scissors,
                (MoveP1.Rock, Result.Draw) => MoveP2.Rock,
                (MoveP1.Rock, Result.Win) => MoveP2.Paper,

                (MoveP1.Paper, Result.Lose) => MoveP2.Rock,
                (MoveP1.Paper, Result.Draw) => MoveP2.Paper,
                (MoveP1.Paper, Result.Win) => MoveP2.Scissors,

                (MoveP1.Scissors, Result.Lose) => MoveP2.Paper,
                (MoveP1.Scissors, Result.Draw) => MoveP2.Scissors,
                (MoveP1.Scissors, Result.Win) => MoveP2.Rock,

                _ => throw new ArgumentException("Invalid combination")
            };

            public int ScorePt1() => Score(Theirs, Yours);

            public int ScorePt2() => Score(Theirs, GetMove(Theirs, Yours));
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