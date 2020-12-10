﻿using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day09 : IPuzzle
    {
        public string Name { get { return "2020-09"; } }

        static bool ValidateNumber(int index, int preamble, Int64[] numbers) =>
            numbers.Skip(index - preamble).Take(preamble)
                .Pairs().Where(p => p.Item1 + p.Item2 == numbers[index]).Any();

        static Int64 FindInvalid(Int64[] numbers, int preamble) =>
            Enumerable.Range(preamble, numbers.Length)
                .Where(i => !ValidateNumber(i, preamble, numbers))
                .Select(i => numbers[i]).First();


        class Accumulator
        {
            public Accumulator(Int64 initial)
            {
                Min = initial;
                Max = initial;
                Sum = initial;
            }

            public void Add(Int64 val)
            {
                Sum += val;
                Min = Math.Min(Min, val);
                Max = Math.Max(Max, val);
            }

            public Int64 Min { get; private set; }
            public Int64 Max { get; private set; }
            public Int64 Sum { get; private set; }
        }

        public static Int64 Part1(string input, int preamble = 25)
        {
            var numbers = Util.Parse64(input);

            return FindInvalid(numbers, preamble);
        }

        public static Int64 Part2(string input, int preamble=25)
        {
            var numbers = Util.Parse64(input);

            Int64 invalid = FindInvalid(numbers, preamble);

            for (var i=0; i<numbers.Length; ++i)
            {
                var accumulator = new Accumulator( numbers[i] );
                foreach (var n in numbers.Skip(i+1))
                {
                    accumulator.Add(n);
                    if (accumulator.Sum == invalid)
                    {
                        return accumulator.Max + accumulator.Min;
                    }
                    else if (accumulator.Sum > invalid)
                    {
                        break;
                    }
                }
            }

            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}