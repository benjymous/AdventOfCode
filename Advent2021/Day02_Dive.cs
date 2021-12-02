namespace AoC.Advent2021
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2021-02"; } }

        struct Instruction
        {
            public Instruction(char command, int distance)
            {
                Command = command;
                Distance = distance;
            }

            public char Command;
            public int Distance;
        }

        public static int Part1(string input)
        {
            var data = Util.RegexParse<Instruction>(input, @"(.).+ (\d+)");

            int x = 0;
            int y = 0;

            foreach (var line in data)
            {
                switch (line.Command)
                {
                    case 'f': x+= line.Distance; break;
                    case 'u': y-= line.Distance; break;
                    case 'd': y+= line.Distance; break;
                }
            }

            return x*y;
        }

        public static int Part2(string input)
        {
            var data = Util.RegexParse<Instruction>(input, @"(.).+ (\d+)");

            int x = 0;
            int y = 0;
            int aim = 0;

            foreach (var line in data)
            {
                switch (line.Command)
                {
                    case 'f': x += line.Distance; y += (aim * line.Distance); break;
                    case 'u': aim -= line.Distance; break;
                    case 'd': aim += line.Distance; break;
                }
            }

            return x * y;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}