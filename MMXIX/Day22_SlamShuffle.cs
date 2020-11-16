using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Advent.MMXIX
{
    public class Day22 : IPuzzle
    {
        public string Name { get { return "2019-22"; } }

        public class Deck
        {
            public IEnumerable<int> cards;
            int size;

            public Deck(int deckSize)
            {
                size = deckSize;
                cards = Enumerable.Range(0, size);
            }

            protected Deck(int deckSize, IEnumerable<int> seq)
            {
                size = deckSize;
                cards = seq;
            }

            public Deck Stack()
            {
                return new Deck(size, cards.Reverse());
            }

            public Deck Cut(int cut)
            {
                int front = 0;
                int back = 0;

                if (cut >= 0)
                {
                    front = cut;
                    back = size - front;
                }
                else
                {
                    back = Math.Abs(cut);
                    front = size - back;
                }

                var frontCards = cards.Take(front);
                var backCards = cards.Skip(front);

                return new Deck(size, backCards.Union(frontCards));
            }

            public Deck Deal(int increment)
            {
                var output = Enumerable.Repeat(0, size).ToArray();

                int pos = 0;

                foreach (var card in cards)
                {
                    output[pos] = card;
                    pos = (pos + increment) % size;
                }

                return new Deck(size, output);
            }

            public int ElementAt(int index)
            {
                return cards.ElementAt(index);
            }

            public int IndexOf(int card)
            {
                return cards.ToList().IndexOf(card);
            }

            public override string ToString()
            {
                return string.Join(", ", cards);
            }
        }

        static HashSet<string> seen = new HashSet<string>();

        private static Deck Shuffle(Deck deck, string input)
        {
            var lines = Util.Split(input);
            var current = deck;

            foreach (var line in lines)
            {
                if (line.StartsWith("deal with increment"))
                {
                    current = current.Deal(int.Parse(line.Split(" ").Last()));
                }
                else if (line.StartsWith("cut"))
                {
                    current = current.Cut(int.Parse(line.Split(" ").Last()));
                }
                else if (line.Contains("new stack"))
                {
                    current = current.Stack();
                }
                else
                {
                    throw new Exception("Unexpected line");
                }

                var str = string.Join(", ", current.cards.Take(20));
                if (seen.Contains(str))
                {
                    Console.WriteLine();
                }
                seen.Add(str);


            }



            return current;
        }

        public static Deck DoShuffle(int numCards, string input)
        {
            return Shuffle(new Deck(numCards), input);
        }

        public static Deck DoShuffle(Deck deck, string input)
        {
            return Shuffle(deck, input);
        }

        public static int Part1(string input)
        {
            var deck = DoShuffle(10007, input);

            return deck.IndexOf(2019);
        }

        const Int64 numCards = 119315717514047;
        const Int64 iterations = 101741582076661;

        class Matrix
        {
            public BigInteger A, B, C, D;

            public Matrix() { }
            public Matrix(BigInteger a, BigInteger b, BigInteger c, BigInteger d)
            {
                A = a;
                B = b;
                C = c;
                D = d;
            }

            public static Matrix operator *(Matrix x, Matrix y)
            {
                return new Matrix(
                  ((x.A * y.A + x.B * y.C) % numCards + numCards) % numCards,
                  ((x.A * y.B + x.B * y.D) % numCards + numCards) % numCards,
                  ((x.C * y.A + x.D * y.C) % numCards + numCards) % numCards,
                  ((x.C * y.B + x.D * y.D) % numCards + numCards) % numCards
                );
            }
        };

        static Matrix antiCut(BigInteger c) => new Matrix(1, c, 0, 1);

        static Matrix antiRev() => new Matrix(-1, numCards - 1, 0, 1);

        static Matrix antiInc(BigInteger num) => new Matrix(modInverse(num), 0, 0, 1);

        static BigInteger modInverse(BigInteger b) => Pow(b, numCards - 2);

        static BigInteger Pow(BigInteger baseVal, BigInteger exp)
        {
            if (exp == 0)
                return 1;
            var a = Pow(baseVal, exp / 2);
            a = a * a % numCards;
            if ((exp & 1) > 0)
                return (a * baseVal) % numCards;
            else
                return a;
        }

        static Matrix Pow(Matrix m, BigInteger exp)
        {
            if (exp == 0)
            {
                return new Matrix(1, 0, 0, 1);
            }
            var ans = Pow(m, exp / 2);
            ans = ans * ans;
            if ((exp & 1) > 0)
            {
                return m * ans;
            }
            else
            {
                return ans;
            }
        }

        // lifted entirely from c++ answer here:
        // https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbqul0c/
        public static Int64 Part2(string input)
        {
            var lines = Util.Split(input);

            var m = new Matrix(1, 0, 0, 1);

            foreach (var line in lines)
            {
                if (line.StartsWith("deal with increment"))
                {
                    var value = int.Parse(line.Split(" ").Last());
                    m = m * antiInc(value);

                }
                else if (line.StartsWith("cut"))
                {
                    var value = Int64.Parse(line.Split(" ").Last());

                    m = m * antiCut(value);

                }
                else if (line.Contains("new stack"))
                {
                    m = m * antiRev();
                }
                else
                {
                    throw new Exception("Unexpected line");
                }

            }


            m = Pow(m, iterations);

            var answer = (m.A * 2020 + m.B) % numCards;

            return (Int64)answer;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}