using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

                Out = Util.ParseSparseMatrix<char>(outLines);

            }

            [Regex(@"(...)\/(...)\/(...) => (....)\/(....)\/(....)\/(....)")]
            public Rule(string in1, string in2, string in3, string out1, string out2, string out3, string out4)
            {
                In.Add(in1);
                In.Add(in2);
                In.Add(in3);

                var outLines = $"{out1}\n{out2}\n{out3}\n{out4}";

                Out = Util.ParseSparseMatrix<char>(outLines);
            }

            List<string> In = new();
            public readonly Dictionary<(int x, int y), char> Out = new();


            public IEnumerable<string> Rotations()
            {
                var data = string.Join("\n", In);
                var matrix = Util.ParseSparseMatrix<char>(data);

                int maxx = matrix.Max(v => v.Key.x);
                int maxy = matrix.Max(x => x.Key.y);


                for (int i = 0; i < 8; ++i)
                {
                    //Console.Write(i);
                    var res = "";

                    for (int y = 0; y <= maxy; y++)
                    {
                        for (int x = 0; x <= maxx; ++x)
                        {
                            res += matrix[(x, y)];
                        }
                        if (y < maxy) res += "|";
                    }
                    yield return res;

                    //Console.WriteLine($"rotate");
                    matrix = Rotate(matrix);
                    if (i == 3)
                    {
                        //Console.WriteLine($"flip");
                        matrix = Flip(matrix);
                    }
                }
            }
        }
        static Dictionary<(int x, int y), char> Rotate(Dictionary<(int x, int y), char> matrix)
        {
            int max = matrix.Max(v => v.Key.x);
            return matrix.Select(kvp => ((max-kvp.Key.y, kvp.Key.x), kvp.Value)).ToDictionary(v => v.Item1, v => v.Item2);
        }

        static Dictionary<(int x, int y), char> Flip(Dictionary<(int x, int y), char> matrix)
        {
            int max = matrix.Max(v => v.Key.x);
            return matrix.Select(kvp => ((max - kvp.Key.x, kvp.Key.y), kvp.Value)).ToDictionary(v => v.Item1, v => v.Item2);
        }

        private static Dictionary<(int x, int y), char> ApplyRules(Dictionary<(int x, int y), char> map, Dictionary<string, Rule> rules)
        {
            int size = map.Max(v => v.Key.x)+1;
            var result = new Dictionary<(int x, int y), char>();

            if (size%3 == 0)
            {
                var cells = size / 3;

                for (int celly = 0; celly < cells; celly++)
                {
                    for (int cellx = 0; cellx < cells; cellx++)
                    {
                        var cell = "";

                        for (int y = 0; y < 3; ++y)
                        {
                            for (int x = 0; x < 3; ++x)
                            {
                                cell += map[(x+(cellx*3), y+(celly*3))];
                            }
                            if (y < 2) cell += "|";
                        }

                        if (rules.TryGetValue(cell, out var rule))
                        {
                            var ruleOut = rule.Out;

                            for (int y1 = 0; y1 < 4; ++y1)
                            {
                                for (int x1 = 0; x1 < 4; ++x1)
                                {
                                    result[(x1 + (cellx*4), y1+(celly*4))] = ruleOut[(x1,y1)];
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("!!!");
                        }

                    }
                }

                Debug.Assert(result.Count == cells * 4 * cells * 4);
            }
            else
            {
                var cells = size / 2;

                for (int celly = 0; celly < cells; celly++)
                {
                    for (int cellx = 0; cellx < cells; cellx++)
                    {
                        var cell = "";

                        for (int y = 0; y < 2; ++y)
                        {
                            for (int x = 0; x < 2; ++x)
                            {
                                cell += map[(x + (cellx * 2), y + (celly * 2))];
                            }
                            if (y < 1) cell += "|";
                        }

                        if (rules.TryGetValue(cell, out var rule))
                        {
                            var ruleOut = rule.Out;

                            for (int y1 = 0; y1 < 3; ++y1)
                            {
                                for (int x1 = 0; x1 < 3; ++x1)
                                {
                                    result[(x1 + (cellx * 3), y1 + (celly * 3))] = ruleOut[(x1, y1)];
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("!!!");
                        }

                    }
                }

                Debug.Assert(result.Count == cells * 3 * cells * 3);
            }

            return result;
        }

        public static int Part1(string input)
        {
            return RunRules(input, 5);
        }

        public static int RunRules(string input, int iterations)
        {
            var rules = Util.RegexParse<Rule>(input);

            var allRules = new Dictionary<string, Rule>();
            foreach (var rule in rules)
            {
                foreach (var key in rule.Rotations().ToArray())
                {
                    if (!allRules.ContainsKey(key))
                    {
                        allRules[key] = rule;
                    }
                }
            }

            var map = Util.ParseSparseMatrix<char>(".#.\n..#\n###");
 

            for (int i = 0; i < iterations; ++i)
            {
                Display(map);
                map = ApplyRules(map, allRules);
            }

            Display(map);

            return map.Values.Count(v => v == '#');
        }

        private static void Display(Dictionary<(int x, int y), char> map)
        {
            var size = map.Max(v => v.Key.x);

            for (int y=0; y<=size; ++y)
            {
                for (int x=0; x<=size; ++x)
                {
                    Console.Write(map[(x, y)]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {

            var test = @"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#".Replace("\r", "");

            Console.WriteLine(RunRules(test, 2));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));

            // 101 too low
            // 310 (iter6) too high (so not 1 off iterations)
        }
    }
}