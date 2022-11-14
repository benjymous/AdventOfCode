using System;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day08 : IPuzzle
    {
        public string Name => "2015-08";

        public static string ReplaceHexChars(string input)
        {
            if (!input.Contains("\\x")) return input;

            string output = "";
            for (var i = 0; i < input.Length; ++i)
            {
                if (i < input.Length - 3 && input[i] == '\\' && input[i + 1] == 'x')
                {
                    string hex = $"{input[i + 2]}{input[i + 3]}";

                    if (Int32.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int parsed))
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

        public static string TrimOuterQuotes(string input)
        {
            if (input.StartsWith("\"") && input.EndsWith("\""))
            {
                return input[1..^1];
            }
            return input;
        }

        public static string Unescape(string input)
        {
            return ReplaceHexChars(TrimOuterQuotes(input).Replace("\\\\", "|").Replace("\\\"", "\"")).Replace("|", "\\");
        }

        public static string Encode(char input)
        {
            return input switch
            {
                '\"' => "\\\"",
                '\\' => "\\\\",
                _ => $"{input}",
            };
        }

        public static string Escape(string input)
        {
            return '\"' + String.Join("", input.Select(c => Encode(c))) + '\"';
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input, '\n');

            var result = lines.Select(line => (line, Unescape(line)));

            // foreach (var r in result)
            // {
            //     Console.WriteLine($"{r.Item1} - >{r.Item2}<");
            // }

            return result.Select(x => x.line.Length - x.Item2.Length).Sum();
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input, '\n');

            var result = lines.Select(line => (line, Escape(line)));

            // foreach (var r in result)
            // {
            //     Console.WriteLine($"{r.Item1} - >{r.Item2}<");
            // }

            return result.Select(x => x.Item2.Length - x.line.Length).Sum();
        }

        public void Run(string input, ILogger logger)
        {
            // Console.WriteLine(Unescape("\"\""));
            // Console.WriteLine(Unescape("\"abc\""));
            // Console.WriteLine(Unescape("\"aaa\\\"aaa\""));
            // Console.WriteLine(Unescape("\"\\x27\""));

            // Console.WriteLine(Part1("\"\"\n\"abc\"\n\"aaa\\\"aaa\"\n\"\\x27\"\n"));

            //Console.WriteLine(Escape("\"\"").Length);
            //Console.WriteLine(Escape("\"abc\"").Length);
            //Console.WriteLine(Escape("\"aaa\\\"aaa\"").Length);
            //Console.WriteLine(Escape("\"\\x27\"").Length);

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}