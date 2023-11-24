namespace AoC.Advent2018;
public class Day02 : IPuzzle
{
    public static int Part1(string input)
    {
        var keys = Util.Split(input);

        int doubles = 0;
        int triples = 0;

        foreach (var id in keys)
        {
            var grp = id.GroupBy(c => c);
            doubles += grp.Any(g => g.Count() == 2) ? 1 : 0;
            triples += grp.Any(g => g.Count() == 3) ? 1 : 0;
        }

        return doubles * triples;
    }

    public static string Part2(string input)
    {
        var keys = Util.Split(input);
        for (int i = 0; i < keys.Length; i++)
        {
            string s1 = keys[i];
            for (int j = i + 1; j < keys.Length; j++)
            {
                string s2 = keys[j];
                var diff = 0;
                var answer = "";

                for (int k = 0; k < s1.Length; ++k)
                {
                    if (s1[k] != s2[k])
                    {
                        diff++;
                        if (diff > 1) break;
                    }
                    else
                    {
                        answer += s1[k];
                    }
                }

                if (diff == 1)
                {
                    return answer;
                }
            }
        }
        return "FAIL";

    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
