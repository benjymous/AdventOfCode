namespace AoC.Advent2022;
public class Day05 : IPuzzle
{
    [method: Regex(@"move (\d+) from (\d+) to (\d+)")]
    readonly struct Instruction(int count, int from, int to)
    {
        readonly int From = from - 1, To = to - 1;

        public void ApplyV1(Stack<char>[] stacks) => Transfer(stacks[From], stacks[To], count);

        public void ApplyV2(Stack<char>[] stacks, Stack<char> grab)
        {
            Transfer(stacks[From], grab, count);
            Transfer(grab, stacks[To], count);
        }

        static void Transfer(Stack<char> from, Stack<char> to, int count)
        {
            for (int i = 0; i < count; ++i) to.Push(from.Pop());
        }
    }

    static (IEnumerable<Instruction>, Stack<char>[]) ParseData(string input)
    {
        var (layout, instructions) = input.ParseSections(stack => Util.Split(stack),
                                                         instr => Parser.Parse<Instruction>(instr));

        var stackCount = Util.ParseNumbers<int>(layout.Last(), " ").Last();
        var grid = Util.ParseMatrix<char>(layout.Reverse().Skip(1));
        var stacks = Enumerable.Range(0, stackCount)
                               .Select(i => grid.Column((i * 4) + 1).Where(c => c != ' ').ToStack())
                               .ToArray();

        return (instructions, stacks);
    }

    public static string Part1(string input)
    {
        var (instructions, stacks) = ParseData(input);

        instructions.ForEach(i => i.ApplyV1(stacks));

        return stacks.Select(s => s.Peek()).AsString();
    }

    public static string Part2(string input)
    {
        var (instructions, stacks) = ParseData(input);

        Stack<char> grab = new();

        instructions.ForEach(i => i.ApplyV2(stacks, grab));

        return stacks.Select(s => s.Peek()).AsString();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}