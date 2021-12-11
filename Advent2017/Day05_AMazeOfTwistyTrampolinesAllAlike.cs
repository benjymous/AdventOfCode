namespace AoC.Advent2017
{
    public class Day05 : IPuzzle
    {
        public string Name => "2017-05";

        public static int Run(string input, bool mode2)
        {
            var instructions = Util.Parse32(input);

            int position = 0;
            int steps = 0;

            while (position >= 0 && position < instructions.Length)
            {
                int offset = instructions[position];

                if (mode2 && (offset >= 3))
                {
                    instructions[position]--;
                }
                else
                {
                    instructions[position]++;
                }

                position += offset;
                steps++;
            }

            return steps;
        }

        public static int Part1(string input)
        {
            return Run(input, false);
        }

        public static int Part2(string input)
        {
            return Run(input, true);
        }

        public void Run(string input, ILogger logger)
        {


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}