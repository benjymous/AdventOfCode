namespace AoC.Advent2015
{
    public class Day01 : IPuzzle
    {
        public string Name => "2015-01";

        private static int Solve(string input, QuestionPart part)
        {
            var data = input.Trim();

            int count = 0;
            int pos = 0;
            foreach (var c in data)
            {
                pos++;
                if (c == '(') count++;
                if (c == ')') count--;

                if (part.Two() && count == -1) return pos;
            }

            return count;
        }

        public static int Part1(string input)
        {
            return Solve(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Solve(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}