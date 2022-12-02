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
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        public class RulePt1
        {
            [Regex(@"(.) (.)")]
            public RulePt1(char first, char second)
            {
                First = first;
                Second = second; 
            }

            public char First;
            public char Second;

            RPS Decode(char c)
            {
                switch (c){
                    case 'A':
                    case 'X':
                        return RPS.Rock;
                    case 'B':
                    case 'Y':
                        return RPS.Paper;
                    case 'C':
                    case 'Z':
                        return RPS.Scissors;
                    default:
                        throw new ArgumentException("Unexpected char");          
                }
            }

            Dictionary<(RPS, RPS), int> scores = new()
            {
                {(RPS.Rock, RPS.Rock), 3},
                {(RPS.Rock, RPS.Paper), 0},
                {(RPS.Rock, RPS.Scissors), 6},

                {(RPS.Paper, RPS.Rock), 6},
                {(RPS.Paper, RPS.Paper), 3},
                {(RPS.Paper, RPS.Scissors), 0},

                {(RPS.Scissors, RPS.Rock), 0},
                {(RPS.Scissors, RPS.Paper), 6},
                {(RPS.Scissors, RPS.Scissors), 3},
            };

            public int Score()
            {
                var theirs = Decode(First);
                var yours = Decode(Second);

                return (int)yours + scores[(yours,theirs)];
            }
        }



        public static int Part1(string input)
        {
            var rules = Util.RegexParse<RulePt1>(input);
            return rules.Sum(r => r.Score());
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}