using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2020
{
    public class Day22 : IPuzzle
    {
        public string Name => "2020-22";

        static Queue<byte>[] ParseData(string input)
        {
            var groups = input.Split("\n\n");
            var decks = new List<Queue<byte>>();

            foreach (var group in groups)
            {
                var data = group.Split("\n");
                var deck = Util.Parse32(data.Skip(1)).Select(i => (byte)i).ToQueue();
                decks.Add(deck);
            }

            return decks.ToArray();
        }

        static int GetKey(Queue<byte>[] decks) => decks[0].GetCombinedHashCode();

        static int PlayRound(Queue<byte>[] decks, bool recursive = false, bool subgame = false)
        {
            var seen = new HashSet<int>();

            while (decks.Where(d => d.Count > 0).Count() == 2)
            {
                if (subgame)
                {
                    var key = GetKey(decks);
                    if (seen.Contains(key))
                    {
                        return 0;
                    }
                    seen.Add(key);
                }

                var taken = decks.Select(d => d.Dequeue()).ToArray();

                if (recursive &&
                    decks[0].Count >= taken[0] &&
                    decks[1].Count >= taken[1])
                {
                    var result = PlayRound(new Queue<byte>[]{
                        decks[0].Take(taken[0]).ToQueue(),
                        decks[1].Take(taken[1]).ToQueue()
                    }, true, true);

                    decks[result].EnqueueRange(result == 0 ? taken : taken.Reverse());
                }
                else
                {
                    decks[taken[0] > taken[1] ? 0 : 1].EnqueueRange(taken.OrderDescending());
                }
            }

            if (subgame)
            {
                return decks[0].Count == 0 ? 1 : 0;
            }
            else
            {
                var idx = 1;
                return decks.Where(d => d.Count > 0)
                    .First()
                    .Reverse()
                    .Select(card => card * idx++)
                    .Sum();
            }
        }

        public static int Part1(string input)
        {
            return PlayRound(ParseData(input));
        }


        public static int Part2(string input)
        {
            return PlayRound(ParseData(input), true, false);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}