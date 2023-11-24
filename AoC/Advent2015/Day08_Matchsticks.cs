namespace AoC.Advent2015;
public class Day08 : IPuzzle
{
    public static string ReplaceHexChars(string input)
    {
        if (!input.Contains("\\x")) return input;

        string output = "";
        for (var i = 0; i < input.Length; ++i)
        {
            if (i < input.Length - 3 && input[i] == '\\' && input[i + 1] == 'x')
            {
                string hex = $"{input[i + 2]}{input[i + 3]}";

                if (int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int parsed))
                {
                    var decoded = (char)parsed;

                    output += decoded;
                    i += 3;
                }
                else
                {
                    output += input[i];
                }
            }
            else
            {
                output += input[i];
            }
        }
        return output;
    }

    public static string TrimOuterQuotes(string input) => input.StartsWith('\"') && input.EndsWith('\"') ? input[1..^1] : input;

    public static string Unescape(string input) => ReplaceHexChars(TrimOuterQuotes(input).Replace("\\\\", "|").Replace("\\\"", "\"")).Replace("|", "\\");

    public static string Encode(char input)
    {
        return input switch
        {
            '\"' => "\\\"",
            '\\' => "\\\\",
            _ => $"{input}",
        };
    }

    public static string Escape(string input) => '\"' + string.Join("", input.Select(Encode)) + '\"';

    public static int Part1(string input)
    {
        var lines = Util.Split(input, "\n");

        var result = lines.Select(line => (line, Unescape(line)));

        return result.Sum(x => x.line.Length - x.Item2.Length);
    }

    public static int Part2(string input)
    {
        var lines = Util.Split(input, "\n");

        var result = lines.Select(line => (line, Escape(line)));

        return result.Sum(x => x.Item2.Length - x.line.Length);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}