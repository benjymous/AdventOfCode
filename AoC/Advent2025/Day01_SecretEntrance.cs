namespace AoC.Advent2025;

public class Day01 : IPuzzle
{
    [Regex("(.)(.+)")]
    public record class Instruction(char Dir, int Value);

    static int FindPasscode(Parser.AutoArray<Instruction> instructions, QuestionPart part)
    {
        int pos = 50, count = 0;
        foreach (var i in instructions)
        {
            (pos, count) = RotateDial(part, pos, count, i.Value * (i.Dir == 'R' ? 1 : -1));
        }

        return count;
    }

    static (int pos, int count) RotateDial(QuestionPart part, int position, int count, int delta)
    {
        if (part.One)
        {
            position += delta;
            position.ModWrap(100);
            return (position, position == 0 ? count + 1 : count);
        }
        else
        {
            var s = Math.Sign(delta);
            while (delta != 0)
            {
                position += s;
                delta -= s;
                if (position < 0) position = 99;
                if (position > 99) position = 0;
                if (position == 0) count++;
            }

            return (position, count);
        }
    }

    public static int Part1(string input) => FindPasscode(input, QuestionPart.Part1);

    public static int Part2(string input) => FindPasscode(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}