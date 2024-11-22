namespace AoC.Advent2017;
public class Day05 : IPuzzle
{
    public static int Run(string input, QuestionPart part)
    {
        var instructions = Util.ParseNumbers<int>(input);

        int position = 0;
        int steps = 0;

        while (position >= 0 && position < instructions.Length)
        {
            int offset = instructions[position];

            instructions[position] += (part.Two() && (offset >= 3)) ? -1 : 1;

            position += offset;
            steps++;
        }

        return steps;
    }

    public static int Part1(string input) => Run(input, QuestionPart.Part1);

    public static int Part2(string input) => Run(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}