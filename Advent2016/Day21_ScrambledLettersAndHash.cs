using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

using CharFunc = System.Func<System.Collections.Generic.IEnumerable<char>, System.Collections.Generic.IEnumerable<char>>;

namespace AoC.Advent2016
{
    public class Day21 : IPuzzle
    {
        public string Name => "2016-21";

        class InstructionFactory
        {
            [Regex(@"rotate right (\d+) step")]
            public static CharFunc RotRight(int count) => pwd => RotateRight(pwd, count);

            [Regex(@"rotate left (\d+) step")]
            public static CharFunc RotLeft(int count) => pwd => RotateLeft(pwd, count);

            [Regex(@"swap letter (.) with letter (.)")]
            public static CharFunc SwapL(char from, char to) => pwd => SwapLetter(pwd, from, to);

            [Regex(@"swap position (.) with position (.)")]
            public static CharFunc SwapP(int from, int to) => pwd => SwapPosition(pwd, from, to);

            [Regex(@"reverse positions (.) through (.)")]
            public static CharFunc Rev(int start, int end) => pwd => Reverse(pwd, start, end);

            [Regex(@"move position (.) to position (.)")]
            public static CharFunc Mov(int from, int to) => pwd => Move(pwd, from, to);

            [Regex(@"rotate based on position of letter (.)")]
            public static CharFunc RotBased(char letter) => pwd => RotateRightBasedOnLetter(pwd, letter);
        }

        public static char Switch(char c, char from, char to)
        {
            if (c == from)
            {
                return to;
            }
            else if (c == to)
            {
                return from;
            }
            return c;
        }

        private static IEnumerable<char> SwapLetter(IEnumerable<char> password, char from, char to)
        {
            return password.Select(c => Switch(c, from, to));
        }

        private static IEnumerable<char> SwapPosition(IEnumerable<char> password, int from, int to)
        {
            var charFrom = password.ElementAt(from);
            var charTo = password.ElementAt(to);
            return SwapLetter(password, charFrom, charTo);
        }

        private static IEnumerable<char> RotateLeft(IEnumerable<char> password, int dist)
        {
            var first = password.Take(dist);
            var rest = password.Skip(dist);
            return rest.Concat(first);
        }

        private static IEnumerable<char> RotateRight(IEnumerable<char> password, int dist)
        {
            var last = password.Skip(8 - dist).Take(dist);
            var rest = password.Take(8 - dist);
            return last.Concat(rest);
        }

        static readonly Dictionary<int, int> RotateLetterForward = new()
        {
            {0,1},
            {1,2},
            {2,3},
            {3,4},
            {4,6},
            {5,7},
            {6,8},
            {7,9},
        };

        private static IEnumerable<char> RotateRightBasedOnLetter(IEnumerable<char> password, char letter)
        {            
            var pos = password.IndexOf(letter);
            var dist = RotateLetterForward[pos];
            return RotateRight(password, dist);
        }

        private static IEnumerable<char> Reverse(IEnumerable<char> password, int start, int end)
        {
            return password.Take(start).Concat(password.Skip(start).Take(end - start + 1).Reverse()).Concat(password.Skip(end + 1));
        }

        private static IEnumerable<char> Move(IEnumerable<char> password, int from, int to)
        {
            var fromChar = password.ElementAt(from);
            var filtered = password.Where(c => c != fromChar);
            return filtered.Take(to).Concat($"{fromChar}").Concat(filtered.Skip(to));
        }

        private static string Scramble(IEnumerable<CharFunc> instructions, IEnumerable<char> password)
        {
            foreach (var instr in instructions)
            {
                password = instr(password).ToList();
            }

            return password.AsString();
        }

        public static string Part1(string input)
        {
            var instructions = Util.RegexFactory<CharFunc, InstructionFactory>(input).ToArray();

            return Scramble(instructions, "abcdefgh");
        }

        public static string Part2(string input)
        {
            var instructions = Util.RegexFactory<CharFunc, InstructionFactory>(input).ToArray();

            var scrambled = "fbgdceah";

            var perms = scrambled.Permutations();

            return perms.AsParallel().Where(p => Scramble(instructions, p) == scrambled).First().AsString();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}