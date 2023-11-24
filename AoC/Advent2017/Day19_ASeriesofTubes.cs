namespace AoC.Advent2017;
public class Day19 : IPuzzle
{
    private static (string word, int length) FollowPath(string input)
    {
        var grid = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipChars());

        var pos = grid.Where(kvp => kvp.Key.y == 0).First().Key;

        Direction2 dir = new(0, 1);
        string word = "";
        int count = 0;

        while (true)
        {
            var next = pos.OffsetBy(dir);
            count++;

            if (!grid.TryGetValue(next, out var nextCh)) return (word, count);

            switch (nextCh)
            {
                case '+':
                    // turn a corner
                    var d1 = dir + 1;
                    dir = grid.ContainsKey(next.OffsetBy(d1)) ? d1 : dir - 1;
                    break;

                case >= 'A' and <= 'Z':
                    word += nextCh;
                    break;
            }

            pos = next;
        }
    }

    public static string Part1(string input) => FollowPath(input).word;

    public static int Part2(string input) => FollowPath(input).length;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}