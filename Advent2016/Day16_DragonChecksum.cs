﻿using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day16 : IPuzzle
    {
        public string Name => "2016-16";

        static readonly bool[] pad = new bool[] { false };

        public static IEnumerable<bool> Fill(bool[] input, int size)
        {
            int currentSize = input.Length;
            var result = new List<bool>(size);
            result.AddRange(input);
            while (currentSize < size)
            {
                var extra = pad.Concat(result.Select(c => !c).Reverse()).ToArray();
                result.AddRange(extra);
                currentSize += currentSize + 1;
            }
            return result.Take(size).ToArray();
        }

        public static bool[] Input(IEnumerable<char> input)
        {
            return input.Select(c => c == '1').ToArray();
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
                if (data.Length % 2 != 0) return data;

                List<bool> result = new(data.Length / 2);

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
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}