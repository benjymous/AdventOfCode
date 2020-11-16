using Advent.Utils;
using Advent.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXVIII
{
    public class Day09 : IPuzzle
    {
        public string Name { get { return "2018-09"; } }

        public static UInt64 MarbleGame(int players, int marbles)
        {

            uint nextMarble = 1;
            int player = -1;

            var circle = new Circle<uint>(0);
            var scores = new Dictionary<int, UInt64>();

            var current = circle;

            while (nextMarble <= marbles)
            {
                if (nextMarble % 23 == 0)
                {
                    var removed = current.Back(7);
                    current = removed.Remove();

                    scores.IncrementAtIndex(player, nextMarble + removed.Value);
                }
                else
                {
                    current = current.Next().InsertNext(nextMarble);
                }

                nextMarble++;
                player = (player + 1) % players;

                //if ((nextMarble % 5000) == 0) std::cout << nextMarble << " " << ((nextMarble / marbles) * 100).toFixed(2) + "%" << std::endl;
            }

            var score = scores.Select(kvp => kvp.Value).Max();
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