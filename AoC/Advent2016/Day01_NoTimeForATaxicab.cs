namespace AoC.Advent2016;
public class Day01 : IPuzzle
{
    [Regex(@"(L|R)(\d+)")]
    record class Instruction(char Turn, int Distance);

    private static int FollowPath(string input, QuestionPart part)
    {
        var instructions = Util.RegexParse<Instruction>(input, ",");

        var position = new ManhattanVector2(0, 0);
        var direction = new Direction2(0, -1);

        var seen = new HashSet<(int x, int y)>();

        foreach (var instruction in instructions)
        {
            direction.Turn(instruction.Turn);

            if (part.One())
            {
                position.Offset(direction, instruction.Distance);
            }
            else
            {
                for (int i = 0; i < instruction.Distance; ++i)
                {
                    position.Offset(direction);

                    if (seen.Contains(position))
                    {
                        return position.Length;
                    }
                    seen.Add(position);
                }
            }
        }

        return position.Length;
    }

    public static int Part1(string input) => FollowPath(input, QuestionPart.Part1);

    public static int Part2(string input) => FollowPath(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}