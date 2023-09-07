namespace AoC.Advent2016
{
    public class Day15 : IPuzzle
    {
        public string Name => "2016-15";

        public class Disc
        {
            [Regex(@"Disc #(\d) .+ (\d+) .+ (\d+).")]
            public Disc(int discNum, int numPos, int initialPos) => (NumPos, Offset) = (numPos, discNum + initialPos);

            public readonly int NumPos, Offset;

            public bool CheckAlignment(int delay) => (delay + Offset) % NumPos == 0;
        }

        private static int FindAlignment(string input)
        {
            int i = 0, inc=1;
            foreach (var disc in Util.RegexParse<Disc>(input))
            {
                while (!disc.CheckAlignment(i += inc)) ;
                inc *= disc.NumPos;
            }
            return i;
        }

        public static int Part1(string input)
        {
            return FindAlignment(input);
        }

        public static int Part2(string input)
        {
            return FindAlignment(input + "Disc #7 has 11 .. 0.");
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}