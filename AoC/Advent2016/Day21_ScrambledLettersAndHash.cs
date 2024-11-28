using CharFunc = System.Func<char[], char[]>;

namespace AoC.Advent2016;
public class Day21 : IPuzzle
{
    class InstructionFactory
    {
        [Regex(@"rotate right (\d+) step")]
        public static CharFunc RotRight(int count) => pwd => RotateRight(pwd, count);

        [Regex(@"rotate left (\d+) step")]
        public static CharFunc RotLeft(int count) => pwd => RotateLeft(pwd, count);

        [Regex(@"swap letter (.) with letter (.)")]
        public static CharFunc SwapLetter(char from, char to) => pwd => pwd.SwapIndices(Array.IndexOf(pwd, from), Array.IndexOf(pwd, to));

        [Regex(@"swap position (.) with position (.)")]
        public static CharFunc SwapPosition(int from, int to) => pwd => pwd.SwapIndices(from, to);

        [Regex(@"reverse positions (.) through (.)")]
        public static CharFunc Reverse(int start, int end) => pwd => pwd.Take(start).Concat(pwd.Skip(start).Take(end - start + 1).Reverse()).Concat(pwd.Skip(end + 1)).ToArray();

        [Regex(@"move position (.) to position (.)")]
        public static CharFunc Move(int from, int to) => pwd => [.. pwd.ToList().MoveIndex(from, to)];

        [Regex(@"rotate based on position of letter (.)")]
        public static CharFunc RotateBased(char letter) => pwd => RotateRight(pwd, TranslatePos(pwd.IndexOf(letter)));

        [Regex(@"UNROTATE (.)")]
        public static CharFunc Unrotate(char letter) => pwd =>
        {
            for (int i = 1; i <= 8; ++i)
            {
                if (i != 5)
                {
                    int checkPos = DeTranslatePos(i);
                    var rot = RotateLeft(pwd, i);

                    if (checkPos == rot.IndexOf(letter) || (checkPos == 0 && rot.IndexOf(letter) == 7)) return rot;
                }
            }
            throw new Exception("fail");
        };
    }

    private static char[] RotateRight(char[] password, int dist) => [.. password[(^dist)..], .. password[..(8 - dist)]];

    private static char[] RotateLeft(char[] password, int dist) => [.. password[dist..], .. password[..dist]];

    private static int TranslatePos(int pos) => pos switch
    {
        0 or 7 => 1,
        1 or 2 or 3 => pos + 1,
        4 or 5 or 6 => pos + 2,
        _ => pos
    };

    private static int DeTranslatePos(int pos) => pos switch
    {
        1 or 2 or 3 or 4 => pos - 1,
        6 or 7 or 8 => pos - 2,
        0 => 7,
        _ => pos
    };

    private static string Scramble(CharFunc[] instructions, char[] password) => instructions.Aggregate(password, (current, instr) => instr(current)).AsString();

    public static CharFunc[] ParseInstructions(string input) => Parser.Factory<CharFunc, InstructionFactory>(input).ToArray();
    public static CharFunc[] ReverseInstructions(string input) => Parser.Factory<CharFunc, InstructionFactory>(input.Split('\n').Select(Reverse).Reverse(), new InstructionFactory()).ToArray();

    public static string Reverse(string rule) => rule.StartsWith("rotate based ")
            ? $"UNROTATE {rule[35]}"
            : rule.StartsWith("move position ")
                ? $"move position {rule[28]} to position {rule[14]}"
                : rule.Contains("right") ? rule.Replace("right", "left") : rule.Replace("left", "right");

    public static string Part1(string input) => Scramble(ParseInstructions(input), [.. "abcdefgh"]);

    public static string Part2(string input) => Scramble(ReverseInstructions(input), [.. "fbgdceah"]);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}