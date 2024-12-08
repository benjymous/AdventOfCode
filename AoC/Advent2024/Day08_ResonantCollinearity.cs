namespace AoC.Advent2024;
public class Day08 : IPuzzle
{
    private static int DetectNodes(string input, QuestionPart part)
    {
        var map = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars('.'));

        HashSet<(int, int)> nodes = [];

        foreach (var pair in map.GroupBy(kvp => kvp.Value)
            .SelectMany(group => group.Select(kvp => kvp.Key).Pairs().Where(pair => pair.Item1 != pair.Item2)))
        {
            var distance = pair.Item1.Subtract(pair.Item2);

            if (part.One())
            {
                var newPos = pair.Item1.OffsetBy(distance);
                if (map.IsInside(newPos))
                {
                    nodes.Add(newPos);
                }
            }
            else
            {
                var newPos = pair.Item1;
                while (map.IsInside(newPos))
                {
                    nodes.Add(newPos);
                    newPos = newPos.OffsetBy(distance);
                }
            }
        }

        return nodes.Count;
    }

    public static int Part1(string input) => DetectNodes(input, QuestionPart.Part1);
    public static int Part2(string input) => DetectNodes(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}