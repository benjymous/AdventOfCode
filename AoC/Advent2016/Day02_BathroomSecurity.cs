namespace AoC.Advent2016;
public class Day02 : IPuzzle
{
    static readonly string keypad1 =
                            "123\n" +
                            "456\n" +
                            "789";

    static readonly string keypad2 =
                            "  1  \n" +
                            " 234 \n" +
                            "56789\n" +
                            " ABC \n" +
                            "  D  \n";

    static Dictionary<(int x, int y), char> ParseKeypad(string kp) => Util.ParseSparseMatrix<char>(kp, new Util.Convertomatic.SkipChars());

    public static IEnumerable<char> SimulateKeypad(string input, string keypadLayout)
    {
        var keypad = ParseKeypad(keypadLayout);

        var position = keypad.First(kvp => kvp.Value == '5').Key;

        var lines = Util.Split(input);

        foreach (var line in lines)
        {
            foreach (var offset in line.Select(c => new Direction2(c)))
            {
                var newPos = position.OffsetBy(offset);
                if (keypad.ContainsKey(newPos))
                {
                    position = newPos;
                }
            }
            yield return keypad[position];
        }
    }

    public static string Part1(string input) => SimulateKeypad(input, keypad1).AsString();

    public static string Part2(string input) => SimulateKeypad(input, keypad2).AsString();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}