using AoC.Utils.Collections;
using System;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day09 : IPuzzle
    {
        public string Name => "2018-09";

        public static UInt64 MarbleGame(int players, int marbles)
        {
            uint nextMarble = 1;
            int player = -1;

            var circle = new Circle<uint>(0);
            var scores = new UInt64[players];

            var current = circle;

            while (nextMarble <= marbles)
            {
                if (nextMarble % 23 == 0)
                {
                    var removed = current.Back(7);
                    current = removed.Remove();

                    scores[player] += nextMarble + removed.Value;
                }
                else
                {
                    current = current.Next().InsertNext(nextMarble);
                }

                nextMarble++;
                player = (player + 1) % players;
            }

            var score = scores.Max();
            return score;
        }

        public static UInt64 Part1(string input)
        {
            var numbers = Util.ExtractNumbers(input);

            var numPlayers = numbers[0];
            var numMarbles = numbers[1];

            return MarbleGame(numPlayers, numMarbles);
        }

        public static UInt64 Part2(string input)
        {
            var numbers = Util.ExtractNumbers(input);

            var numPlayers = numbers[0];
            var numMarbles = numbers[1];

            return MarbleGame(numPlayers, numMarbles * 100);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}