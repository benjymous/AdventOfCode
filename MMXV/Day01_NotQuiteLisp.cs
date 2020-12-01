namespace Advent.MMXV
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2015-01"; } }

        public static int Part1(string input)
        {
            var data = input.Trim();

            int count = 0;
            foreach (var c in data)
            {
                if (c == '(') count++;
                if (c == ')') count--;
            }

            return count;
        }

        public static int Part2(string input)
        {
            var data = input.Trim();

            int count = 0;
            int pos = 0;
            foreach (var c in data)
            {
                pos++;
                if (c == '(') count++;
                if (c == ')') count--;

                if (count == -1) return pos;
            }

            return -1;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}