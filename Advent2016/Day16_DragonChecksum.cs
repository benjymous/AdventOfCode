using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day16 : IPuzzle
    {
        public string Name { get { return "2016-16"; } }

        public static bool[] pad = new bool[] { false };

        public static IEnumerable<bool> Iterate(IEnumerable<bool> input)
        {
            var a = input;
            var b = input.Reverse().Select(c => !c);
            return a.Concat(pad).Concat(b);
        }

        public static IEnumerable<bool> Fill(IEnumerable<bool> input, int size)
        {
            IEnumerable<bool> vals = input;
            while (input.Count() < size)
            {
                input = Iterate(input);
            }
            return input.Take(size);
        }

        public static IEnumerable<bool> Input(IEnumerable<char> input)
        {
            return input.Select(c => c == '1');
        }

        public static string Output(IEnumerable<bool> output)
        {
            return output.Select(b => b ? '1' : '0').AsString();
        }

        public static IEnumerable<bool> CheckSum(IEnumerable<bool> input)
        {
            var data = input.ToArray();
            while (true)
            {
                if (data.Count() % 2 != 0) return data;

                List<bool> result = new List<bool>();

                for (int i = 0; i < data.Length; i += 2)
                {
                    result.Add((data[i] == data[i + 1]));
                }
                data = result.ToArray();
            }
        }

        public static string WipeAndCheck(string start, int size)
        {
            var filled = Fill(Input(start), size);
            var checksum = CheckSum(filled);

            return Output(checksum);
        }

        public static string Part1(string input)
        {
            return WipeAndCheck(input.Trim(), 272);
        }

        public static string Part2(string input)
        {
            return WipeAndCheck(input.Trim(), 35651584);
        }
        public void Run(string input, ILogger logger)
        {
            // Util.Test(Iterate("1").AsString(), "100");
            // Util.Test(Iterate("0").AsString(), "001");
            // Util.Test(Iterate("11111").AsString(), "11111000000");
            // Util.Test(Iterate("111100001010").AsString(), "1111000010100101011110000");

            // Util.Test(Fill("10000", 20).AsString(), "10000011110010000111");

            // Util.Test(Output(CheckSum(Input("110010110100"))), "100");

            // var v1 = Fill(Input("10010000000110000"), 35651584);
            // logger.WriteLine("1");
            // var v2 = CheckSum(v1);
            // logger.WriteLine("2");

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}