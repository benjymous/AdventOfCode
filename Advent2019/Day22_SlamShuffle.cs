using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Advent2019
{
    public class Day22 : IPuzzle
    {
        public string Name => "2019-22";

        public class Deck
        {
            public IEnumerable<int> cards;
            readonly int size;

            public Deck(int deckSize)
            {
                size = deckSize;
                cards = Enumerable.Range(0, size).ToArray();
            }

            protected Deck(int deckSize, IEnumerable<int> seq)
            {
                size = deckSize;
                cards = seq.ToArray();
            }

            public Deck Stack() => new (size, cards.Reverse());

            public Deck Cut(int cut)
            {
                int front = cut >= 0 ? cut : size - Math.Abs(cut);
                return new Deck(size, cards.Skip(front).Concat(cards.Take(front)));
            }

            public Deck Deal(int increment)
            {
                var output = new int[size];
                int pos = 0;
                foreach (var card in cards)
                {
                    output[pos] = card;
                    pos = (pos + increment) % size;
                }
                return new Deck(size, output);
            }

            public int IndexOf(int card) => cards.ToList().IndexOf(card);

            public override string ToString() => string.Join(", ", cards);
        }

        private static Deck Shuffle(Deck deck, string input)
        {
            var lines = Util.Split(input);
            var current = deck;

            foreach (var line in lines)
            {
                if (line.StartsWith("deal with increment"))
                    current = current.Deal(int.Parse(line.Split(" ").Last()));
                else if (line.StartsWith("cut"))
                    current = current.Cut(int.Parse(line.Split(" ").Last()));
                else if (line.Contains("new stack"))
                    current = current.Stack();
            }

            return current;
        }

        public static Deck DoShuffle(int numCards, string input) => Shuffle(new Deck(numCards), input);

        const long numCards = 119315717514047;
        const long iterations = 101741582076661;

        record struct Matrix(BigInteger A, BigInteger B, BigInteger C, BigInteger D)
        {
            public static Matrix operator *(Matrix x, Matrix y) => new (
                ((x.A * y.A + x.B * y.C) % numCards + numCards) % numCards,
                ((x.A * y.B + x.B * y.D) % numCards + numCards) % numCards,
                ((x.C * y.A + x.D * y.C) % numCards + numCards) % numCards,
                ((x.C * y.B + x.D * y.D) % numCards + numCards) % numCards
            );
        };

        static Matrix AntiCut(BigInteger c) => new(1, c, 0, 1);

        static Matrix AntiRev() => new(-1, numCards - 1, 0, 1);

        static Matrix AntiInc(BigInteger num) => new(ModInverse(num), 0, 0, 1);

        static BigInteger ModInverse(BigInteger b) => Pow(b, numCards - 2);

        static BigInteger Pow(BigInteger baseVal, BigInteger exp)
        {
            if (exp == 0) return 1;
            var res = Pow(baseVal, exp / 2);
            res = res * res % numCards;
            return (exp & 1) > 0 ? (res * baseVal) % numCards : res;
        }

        static Matrix Pow(Matrix m, BigInteger exp)
        {
            if (exp == 0) return new Matrix(1, 0, 0, 1);
            var res = Pow(m, exp / 2);
            res *= res;
            return (exp & 1) > 0 ? m * res : res;
        }

        public static int Part1(string input)
        {
            return DoShuffle(10007, input).IndexOf(2019);
        }

        // lifted entirely from c++ answer here:
        // https://www.reddit.com/r/adventofcode/comments/ee0rqi/2019_day_22_solutions/fbqul0c/
        public static long Part2(string input)
        {
            var lines = Util.Split(input);

            var m = new Matrix(1, 0, 0, 1);

            foreach (var line in lines)
            {
                if (line.StartsWith("deal with increment"))
                    m *= AntiInc(int.Parse(line.Split(" ").Last()));
                else if (line.StartsWith("cut"))
                    m *= AntiCut(long.Parse(line.Split(" ").Last()));
                else if (line.Contains("new stack"))
                    m *= AntiRev();
            }

            m = Pow(m, iterations);

            return (long)((m.A * 2020 + m.B) % numCards);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}