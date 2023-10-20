using Advent.Utils;
using AoC.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC.Advent2017
{
    public class Day21 : IPuzzle
    {
        public string Name => "2017-21";

        public class Rule
        {
            [Regex(@"(..)\/(..) => (...)\/(...)\/(...)")]
            public Rule(string in1, string in2, string out1, string out2, string out3) => Init(Util.Values(in1, in2), Util.Values(out1, out2, out3));

            [Regex(@"(...)\/(...)\/(...) => (....)\/(....)\/(....)\/(....)")]
            public Rule(string in1, string in2, string in3, string out1, string out2, string out3, string out4) => Init(Util.Values(in1, in2, in3), Util.Values(out1, out2, out3, out4));

            void Init(string[] inputs, string[] outputs) => (Rotations, Out) = (CalcRotations(inputs).ToArray(), Util.ParseSparseMatrix<char>(outputs, new Util.Convertomatic.SkipSpaces('.')).Select(kvp => ToKey(kvp.Key.x, kvp.Key.y)).ToArray());

            public int[] Rotations, Out;

            static IEnumerable<int> CalcRotations(string[] input)
            {
                var matrix = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipSpaces('.')).Select(kvp => kvp.Key).ToArray();

                int size = input.Length;
                if (matrix.Length == 0) yield return size == 3 ? 16 : 0;

                var iter = (0, 0, size - 1, size - 1).Iterate().ToArray();

                for (int i = 0; i < 8; ++i)
                {
                    yield return iter.Select(cell => matrix.Contains(cell)).AsNumber<int>() + (size == 3 ? 16 : 0);

                    for (int j = 0; j < matrix.Length; ++j) matrix[j] = (size - 1 - matrix[j].y, matrix[j].x); // rotate
                    if (i == 3) for (int j = 0; j < matrix.Length; ++j) matrix[j] = (size - 1 - matrix[j].x, matrix[j].y); // flip
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToKey(int x, int y) => x + (y << 16);

        private static IEnumerable<int> ApplyRules(HashSet<int> map, int[][] rules)
        {
            int size = map.Max(v => v & 0xffff) + 1;
            var (cellSize, ruleOffset) = (size % 2 == 0) ? (2, 0) : (3, 16);

            var cells = size / cellSize;
            var iter = (0, 0, cellSize - 1, cellSize - 1).Iterate().Select(pos => ToKey(pos.x, pos.y)).ToArray();

            for (int celly = 0; celly < cells; celly++)
            {
                for (int cellx = 0; cellx < cells; cellx++)
                {
                    int inputPos = ToKey(cellx * cellSize, celly * cellSize), outputPos = ToKey(cellx * (cellSize + 1), celly * (cellSize + 1));
                    var ruleIndex = ruleOffset + iter.Select(c => map.Contains(c + inputPos)).AsNumber<int>();

                    foreach (var offset in rules[ruleIndex])
                        yield return offset + outputPos;
                }
            }
        }

        public static int RunRules(string input, int iterations)
        {
            var rules = Util.RegexParse<Rule>(input).Select(rule => rule.Rotations.ToHashSet().Select(key => (key, rule))).SelectMany(x => x).OrderBy(x => x.key).Select(x => x.rule.Out).ToArray();
            var map = Util.ParseSparseMatrix<char>(".#.\n..#\n###", new Util.Convertomatic.SkipSpaces('.')).Select(kvp => ToKey(kvp.Key.x, kvp.Key.y)).ToHashSet();

            for (int i = 0; i < iterations; ++i)
                map = ApplyRules(map, rules).ToHashSet();

            return map.Count;
        }

        public static int Part1(string input)
        {
            return RunRules(input, 5);
        }

        public static int Part2(string input)
        {
            return RunRules(input, 18);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}