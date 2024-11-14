namespace AoC.Advent2020;
public class Day18 : IPuzzle
{
    enum Operation
    {
        blank,
        add,
        multiply
    }

    static long Solve(Queue<char> data, QuestionPart part)
    {
        var stack = new Stack<long>();
        long result = 0;
        Operation op = Operation.blank;
        while (data.TryDequeue(out var ch))
        {
            long val = -1;

            if (ch is >= '0' and <= '9')
            {
                val = ch.AsDigit();
            }
            else if (ch == '(')
            {
                // solve brackets
                val = Solve(data, part);
            }
            else if (ch == ')')
            {
                // end of bracketed section
                break;
            }
            else if (ch == '+')
            {
                op = Operation.add;
            }
            else if (ch == '*')
            {
                if (part.One())
                {
                    op = Operation.multiply;
                }
                else
                {
                    stack.Push(result);
                    result = 0;
                    op = Operation.blank;
                }
            }

            if (val != -1)
            {
                result = op switch
                {
                    Operation.add => result + val,
                    Operation.multiply => result * val,
                    _ => val,
                };
            }
        }

        while (stack.Count > 0)
        {
            result *= stack.Pop();
        }

        return result;
    }

    static Queue<char> ToQueue(string input) => new(input.Replace(" ", ""));

    public static long Solve1(string input) => Solve(ToQueue(input), QuestionPart.Part1);

    public static long Solve2(string input) => Solve(ToQueue(input), QuestionPart.Part2);

    public static long Part1(string input) => input.Split("\n").Sum(Solve1);

    public static long Part2(string input) => input.Split("\n").Sum(Solve2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}