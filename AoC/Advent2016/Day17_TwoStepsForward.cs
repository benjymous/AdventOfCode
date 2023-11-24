namespace AoC.Advent2016;
public class Day17 : IPuzzle
{
    const string direction = "UDLR";

    static readonly int[] offsets = [-4, 4, -1, 1];

    static readonly bool[][] availableDoors = GetAvailableDoors();

    static bool[][] GetAvailableDoors()
    {
        bool[][] result = new bool[16][];
        for (int i = 0; i < 16; ++i)
        {
            int x = i % 4, y = i / 4;
            result[i] = [y > 0, y < 3, x > 0, x < 3];
        }
        return result;
    }

    public static IEnumerable<int> Exits((string path, int position) state, string passcode)
    {
        var possible = availableDoors[state.position];
        var unlocked = $"{passcode}{state.path}".GetMD5Chars(2).Select(c => c >= 'B').ToArray();
        for (int i = 0; i < 4; ++i)
        {
            if (possible[i] && unlocked[i]) yield return i;
        }
    }

    private static string FindRoute(string input, QuestionPart part)
    {
        var passcode = input.Trim();

        var jobqueue = new Queue<(string path, int position)>(400) { { ("", 0) } };

        string best = "";
        var dest = 15; // bottom right cell

        while (jobqueue.Count != 0)
        {
            var entry = jobqueue.Dequeue();

            if (entry.position == dest)
            {
                if (part.One()) return entry.path;
                else if (entry.path.Length > best.Length) best = entry.path;
            }
            else
            {
                foreach (var i in Exits(entry, passcode))
                {
                    jobqueue.Enqueue((entry.path + direction[i], entry.position + offsets[i]));
                }
            }
        }

        return best;
    }

    public static string Part1(string input) => FindRoute(input, QuestionPart.Part1);

    public static int Part2(string input) => FindRoute(input, QuestionPart.Part2).Length;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}