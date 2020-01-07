using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day09 : IPuzzle
    {
        public string Name { get { return "2018-09"; } }

        class Node
        {
            public Node(uint v, Node p=null, Node n=null)
            {
                val = v;
                prev=p;
                next=n;

                if (prev!=null)
                {
                    prev.next = this;
                }

                if (next!=null)
                {
                    next.prev = this;
                }
            }
            public uint val;
            public Node prev;
            public Node next;
        };

        static Node back(Node node, int distance)
        {
            for (int i = 0; i < distance; ++i)
            {
                node = node.prev;
            }
            return node;
        }

        static Node forward(Node node, int distance)
        {
            for (int i = 0; i < distance; ++i)
            {
                node = node.next;
            }
            return node;
        }

        public static UInt64 MarbleGame(int players, int marbles)
        {

            uint nextMarble = 1;
            int player = -1;

            var circle = new Node(0);
            circle.prev = circle;
            circle.next = circle;
            var scores = new Dictionary<int, UInt64>();

            var current = circle;

            while (nextMarble <= marbles)
            {
                if (nextMarble % 23 == 0)
                {
                    var removed = back(current, 7);
                    removed.prev.next = removed.next;
                    removed.next.prev = removed.prev;

                    scores.IncrementAtIndex(player, nextMarble + removed.val);

                    current = removed.next;
                }
                else
                {
                    current = new Node(nextMarble, current.next, current.next.next);
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

            return MarbleGame(numPlayers, numMarbles*100);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}