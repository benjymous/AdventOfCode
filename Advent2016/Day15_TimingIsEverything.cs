using System.Linq;

namespace AoC.Advent2016
{
    public class Day15 : IPuzzle
    {
        public string Name => "2016-15";

        public class Disc
        {
            [Regex(@"Disc #(\d) has (\d+) positions; at time=0, it is at position (\d+).")]
            public Disc(int discNum, int numPos, int initialPos) => (NumPos, Offset) = (numPos, discNum + initialPos);

            readonly int NumPos, Offset;

            public bool CheckStep(int step) => (step + Offset) % NumPos == 0;
        }

        private static int FindDiscGap(string input)
        {
            var discs = Util.RegexParse<Disc>(input).ToArray();

            for (int i = 0; true; i++)
            {
                if (CheckDiscs(discs, i)) return i;
            }
        }

        private static bool CheckDiscs(Disc[] discs, int i)
        {
            foreach (var disc in discs)
            {
                if (!disc.CheckStep(i)) return false;
            }
            return true;
        }

        public static int Part1(string input)
        {
            return FindDiscGap(input);
        }

        public static int Part2(string input)
        {
            input += "Disc #7 has 11 positions; at time=0, it is at position 0.";
            return FindDiscGap(input);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}