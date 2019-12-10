using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day08 : IPuzzle
    {
        public string Name { get { return "2015-08";} }
 
        public static string ReplaceHexChars(string input)
        {
            if (!input.Contains("\\x")) return input;

            string output = "";
            for (var i=0; i<input.Length; ++i)
            {
                if (i < input.Length-3 && input[i]=='\\' && input[i+1]=='x')
                {
                    string hex = $"{input[i+2]}{input[i+3]}";

                    if (Int32.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int parsed))
                    {
                        var decoded = (char)parsed;

                        output += decoded;
                        i+=3;
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
                return input.Substring(1, input.Length-2);
            }
            return input;
        }

        public static string Unescape(string input)
        {
            return ReplaceHexChars(TrimOuterQuotes(input).Replace("\\\\", "|").Replace("\\\"","\"")).Replace("|", "\\");
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input, '\n');

            var result = lines.Select(line => Tuple.Create(line,Unescape(line)));

            foreach (var r in result)
            {
                Console.WriteLine($"{r.Item1} - >{r.Item2}<");
            }

            return result.Select(x => x.Item1.Length - x.Item2.Length).Sum();
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            // Console.WriteLine(Unescape("\"\""));
            // Console.WriteLine(Unescape("\"abc\""));
            // Console.WriteLine(Unescape("\"aaa\\\"aaa\""));
            // Console.WriteLine(Unescape("\"\\x27\""));

            // Console.WriteLine(Part1("\"\"\n\"abc\"\n\"aaa\\\"aaa\"\n\"\\x27\"\n"));

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}