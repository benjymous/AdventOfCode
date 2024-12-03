
namespace AoC.Advent2024;
public class Day03 : IPuzzle
{
    class Factory(QuestionPart part) : Parser.IAutoSplit
    {
        bool enabled = true;

        [Regex(@"mul\((\d+),(\d+)\)")]
        public int Multiply(int a, int b) => enabled ? a * b : 0;

        [Regex(@"do\(\)")]
        public void Activate() => enabled = true;

        [Regex(@"don't\(\)")]
        public void Deactivate() => enabled = part != QuestionPart.Part2;
    }

    public static int Solve(string input, QuestionPart part)
        => Parser.Factory<int, Factory>(input, new Factory(part)).Sum();

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}