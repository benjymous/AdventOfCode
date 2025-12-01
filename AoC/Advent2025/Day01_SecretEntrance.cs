namespace AoC.Advent2025;

public class Day01 : IPuzzle
{
    [Regex(@"(L|R)(\d+)")]
    public class Instruction(char Dir, int Value)
    {
        public (int pos, int count) Apply(QuestionPart part, (int pos, int count) state)
        {
            int delta = Value * (Dir == 'R' ? 1 : -1);
            int newPos = (state.pos + delta).ModWrap(100);

            if (part.One) return (newPos, newPos == 0 ? state.count + 1 : state.count);

            int boundaryDistance = ((delta > 0) ? (100 - state.pos) : state.pos) % 100;
            if (boundaryDistance == 0) boundaryDistance = 100;

            return (newPos, state.count + (Value >= boundaryDistance ? 1 + (Value - boundaryDistance) / 100 : 0));
        }
    }

    static int FindPasscode(Parser.AutoArray<Instruction> instructions, QuestionPart part) 
        => instructions.Aggregate<Instruction, (int pos, int count)>((50, 0), (state, i) => i.Apply(part, state)).count;

    public static int Part1(string input) => FindPasscode(input, QuestionPart.Part1);

    public static int Part2(string input) => FindPasscode(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}