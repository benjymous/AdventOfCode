using AoC.Utils;
using AoC.Utils.Collections;

namespace AoC.Advent2016
{
    public class Day19 : IPuzzle
    {
        public string Name => "2016-19";

        class Elf
        {
            public Elf(int i, int n)
            {
                id = i;
                numPresents = n;
            }
            public int id;
            public int numPresents;

            public void TakePresents(Elf other)
            {
                numPresents += other.numPresents;
            }

            public override string ToString() => $"Elf {id} with {numPresents.MultipleWithS("present")}";
        }

        static Circle<Elf> CreateCircle(int numElves)
        {
            Circle<Elf> start = new(new Elf(1, 1));
            var node = start;
            for (int i = 1; i < numElves; ++i)
            {
                node = node.InsertNext(new Elf(i + 1, 1));
            }
            return start;
        }

        public static int Part1(string input)
        {
            var node = CreateCircle(int.Parse(input));

            while (!node.Solo())
            {
                var victim = node.Next();
                node.Value.TakePresents(victim.Value);
                victim.Remove();
                node = node.Next();
            }

            return node.Value.id;
        }

        public static int Part2(string input)
        {
            int count = int.Parse(input);
            var node = CreateCircle(count);

            var nextVictim = node.Forward(count / 2);

            while (count > 1)
            {
                var victim = nextVictim;
                node.Value.TakePresents(victim.Value);
                nextVictim = victim.Forward(1 + (count % 2));
                victim.Remove();

                count--;
                node = node.Next();
            }

            return node.Value.id;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}