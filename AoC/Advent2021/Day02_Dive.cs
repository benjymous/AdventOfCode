namespace AoC.Advent2021;
public class Day02 : IPuzzle
{
    [Regex(@"(.).+ (\d+)")]
    public record struct Instruction(char Command, int Distance);

    public static int Part1(Parser.AutoArray<Instruction> data)
    {
        int x = 0, y = 0;

        foreach (var line in data)
        {
            switch (line.Command)
            {
                case 'f': x += line.Distance; break;
                case 'u': y -= line.Distance; break;
                case 'd': y += line.Distance; break;
            }
        }

        return x * y;
    }

    public static int Part2(Parser.AutoArray<Instruction> data)
    {
        int x = 0, y = 0, aim = 0;

        foreach (var line in data)
        {
            switch (line.Command)
            {
                case 'f': x += line.Distance; y += aim * line.Distance; break;
                case 'u': aim -= line.Distance; break;
                case 'd': aim += line.Distance; break;
            }
        }

        return x * y;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}