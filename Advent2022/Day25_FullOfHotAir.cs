using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Advent2022
{
    public class Day25 : IPuzzle
    {
        public string Name => "2022-25";

        public static Snafu Part1(string input)
        {
            return Util.Parse<Snafu>(input).Sum();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }

    public class Snafu : ISummable<Snafu>
    {
        public Snafu(long value = 0) => components = value.While((value => value > 0), value => { var res = DivRemBalance(value, 5, 2, out value); return (value, (sbyte)res); }).ToArray();
        public Snafu(string value) => components = value.Reverse().Select(ToDecimal).ToArray();
        Snafu(IEnumerable<sbyte> comp) => (components, balanced) = (comp.ToArray(), false);

        readonly sbyte[] components;
        bool balanced = true;

        public static Snafu operator +(Snafu a, Snafu b) => new (a.components.ZipLongest(b.components).Select(pair => (sbyte)(pair.Item1 + pair.Item2)));

        Snafu Balance()
        {
            if (balanced) return this;
            for (int i = 0; i < components.Length; ++i)
            {
                components[i] = DivRemBalance(components[i], (sbyte)5, (sbyte)2, out var next);
                if (next != 0) components[i + 1] += next;
            }
            balanced = true;
            return this;
        }

        static T DivRemBalance<T>(T input, T five, T two, out T next) where T : IBinaryInteger<T>
        {
            next = input / five;
            input -= (next * five);

            if (input > two) { input -= five; next++; }
            if (input < -two) { input += five; next--; }
            return input;
        }

        public long ToDecimal() => components.Reverse().Aggregate(0L, (current, val) => current * 5 + val);
        public override string ToString() => Balance().components.Reverse().Select(ToChar).AsString();

        static sbyte ToDecimal(char c) => c switch { '=' => -2, '-' => -1, _ => (sbyte)(c - '0') };
        static char ToChar(sbyte v) => v switch { -2 => '=', -1 => '-', _ => (char)('0' + v) };
    }
}