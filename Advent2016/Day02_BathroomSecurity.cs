using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2016
{
    public class Day02 : IPuzzle
    {
        public string Name => "2016-02";

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


        static Dictionary<(int x, int y), char> ParseKeypad(string kp) => Util.ParseSparseMatrix<char>(kp, new Util.Convertomatic.SkipSpaces());

        public static string SimulateKeypad(string input, string keypadLayout)
        {
           var keypad = ParseKeypad(keypadLayout);

            var position = keypad.First(kvp => kvp.Value == '5').Key;

            var lines = Util.Split(input);
            StringBuilder code = new();

            ManhattanVector2 newPos = new(0, 0);

            foreach (var line in lines)
            {
                foreach (var offset in line.Select(c => new Direction2(c)))
                {
                    newPos = position;
                    newPos.Offset(offset);
                    if (keypad.ContainsKey(newPos))
                    {
                        position = newPos;
                    }
                }
                code.Append(keypad[position]);
            }

            return code.ToString();
        }

        public static string Part1(string input)
        {
            return SimulateKeypad(input, keypad1);
        }

        public static string Part2(string input)
        {
            return SimulateKeypad(input, keypad2);
        }

        public void Run(string input, ILogger logger)
        {

            //logger.WriteLine(Part1("ULL\nRRDDD\nLURDL\nUUUUD"));
            //logger.WriteLine(Part2("ULL\nRRDDD\nLURDL\nUUUUD"));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}