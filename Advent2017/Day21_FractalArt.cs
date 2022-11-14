using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day21 : IPuzzle
    {
        public string Name => "2017-21";

        public class Rule
        {
            [Regex(@"(..)\/(..) => (...)\/(...)\/(...)")]
            public Rule(string in1, string in2, string out1, string out2, string out3)
            {
                In.Add(in1);
                In.Add(in2);

                var outLines = $"{out1}\n{out2}\n{out3}";

                Out = Util.ParseSparseMatrix<char>(outLines).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();
            }

            [Regex(@"(...)\/(...)\/(...) => (....)\/(....)\/(....)\/(....)")]
            public Rule(string in1, string in2, string in3, string out1, string out2, string out3, string out4)
            {
                In.Add(in1);
                In.Add(in2);
                In.Add(in3);

                var outLines = $"{out1}\n{out2}\n{out3}\n{out4}";

                Out = Util.ParseSparseMatrix<char>(outLines).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();
            }

            readonly List<string> In = new();
            public readonly HashSet<(int x, int y)> Out = new();


            public IEnumerable<int> Rotations()
            {
                var data = string.Join("\n", In);
                var matrix = Util.ParseSparseMatrix<char>(data).Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

                int size = In.Count;

                if (matrix.Count == 0)
                {
                    yield return (size == 3 ? 16 : 0);
                }
                else
                {
                    for (int i = 0; i < 8; ++i)
                    {
                        int res = 0;

                        for (int y = 0; y < size; y++)
                        {
                            for (int x = 0; x < size; ++x)
                            {
                                res <<= 1;
                                if (matrix.Contains((x, y)))
                                {
                                    res += 1;
                                }

                            }
                        }
                        yield return res + (size == 3 ? 16 : 0);

                        matrix = Rotate(matrix, size - 1);
                        if (i == 3)
                        {
                            matrix = Flip(matrix, size - 1);
                        }
                    }
                }
            }
        }
        static HashSet<(int x, int y)> Rotate(HashSet<(int x, int y)> matrix, int max)
        {
            return matrix.Select(v => (max - v.y, v.x)).ToHashSet();
        }

        static HashSet<(int x, int y)> Flip(HashSet<(int x, int y)> matrix, int max)
        {
            return matrix.Select(v => (max - v.x, v.y)).ToHashSet();
        }

        private static IEnumerable<(int x, int y)> ApplyRules(HashSet<(int x, int y)> map, Rule[] rules)
        {
            int size = map.Max(v => v.x) + 1;
            var result = new HashSet<(int x, int y)>();

            if (size % 2 == 0)
            {
                var cells = size / 2;

                for (int celly = 0; celly < cells; celly++)
                {
                    for (int cellx = 0; cellx < cells; cellx++)
                    {
                        int cell = 0;

                        for (int y = 0; y < 2; ++y)
                        {
                            for (int x = 0; x < 2; ++x)
                            {
                                cell <<= 1;
                                if (map.Contains((x + (cellx * 2), y + (celly * 2)))) cell++;
                            }
                        }

                        foreach (var (x, y) in rules[cell].Out)
                        {
                            yield return (x + (cellx * 3), y + (celly * 3));
                        }
                    }
                }
            }
            else
            {
                var cells = size / 3;

                for (int celly = 0; celly < cells; celly++)
                {
                    for (int cellx = 0; cellx < cells; cellx++)
                    {
                        int cell = 0;

                        for (int y = 0; y < 3; ++y)
                        {
                            for (int x = 0; x < 3; ++x)
                            {
                                cell <<= 1;
                                if (map.Contains((x + (cellx * 3), y + (celly * 3)))) cell++;
                            }
                        }

                        foreach (var (x, y) in rules[cell + 16].Out)
                        {
                            yield return (x + (cellx * 4), y + (celly * 4));
                        }
                    }
                }
            }
        }

        public static int RunRules(string input, int iterations)
        {
            var rules = Util.RegexParse<Rule>(input).Select(rule => rule.Rotations().ToHashSet().Select(key => (key, rule))).SelectMany(x => x).OrderBy(x => x.key).Select(x => x.rule).ToArray();

            var map = Util.ParseSparseMatrix<char>(".#.\n..#\n###").Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();

            for (int i = 0; i < iterations; ++i)
            {
                map = ApplyRules(map, rules).ToHashSet();
            }

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