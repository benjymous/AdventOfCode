using AoC.Utils.Collections;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day09 : IPuzzle
    {
        public string Name => "2018-09";

        public static ulong MarbleGame(int players, int marbles)
        {
            int player = -1;

            var circle = new Circle<uint>(0);
            var scores = new ulong[players];

            var current = circle;

            for (uint m = 1; m <= marbles; ++m)
            {
                if (m % 23 == 0)
                {
                    var removed = current.Back(7);
                    current = removed.Remove();

                    scores[player] += m + removed.Value;
                }
                else
                {
                    current = current.Next().InsertNext(m);
                }

                player = (player + 1) % players;
            }

            var score = scores.Max();
            return score;
        }

        public static ulong Part1(string input)
        {
            var numbers = Util.ExtractNumbers(input);

            var numPlayers = numbers[0];
            var numMarbles = numbers[1];

            return MarbleGame(numPlayers, numMarbles);
        }

        public static ulong Part2(string input)
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