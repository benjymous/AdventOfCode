using AoC.Advent2018.ChronMatic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day16 : IPuzzle
    {
        public string Name => "2018-16";

        public class Test
        {
            public readonly long[] before;
            public readonly long[] after;
            public readonly int instr;
            public int[] args;

            public Test(string b, string i, string a)
            {
                before = Util.ExtractLongNumbers(b);
                after = Util.ExtractLongNumbers(a);
                var data = Util.ExtractNumbers(i);
                instr = data[0];
                args = data.Skip(1).ToArray();
            }
        }

        private static List<Test> ParseTests(string[] testData)
        {
            List<Test> tests = new();

            foreach (var grp in testData)
            {
                var lines = grp.Split('\n');
                if (lines.Length == 3)
                {
                    tests.Add(new Test(lines[0], lines[1], lines[2]));
                }
            }

            return tests;
        }

        private static bool Match(long[] data, long[] after)
        {
            for (int i = 0; i < 4; ++i)
            {
                if (data[i] != after[i])
                {
                    return false;
                }
            }
            return true;
        }

        static bool DoTest(Test test, IInstr instr)
        {
            var data = test.before.ToArray();
            instr.Do(ref test.args, ref data);
            return Match(data, test.after);
        }

        static Dictionary<Test, HashSet<IInstr>> RunTests(string[] lines, ILogger logger = null)
        {
            var tests = ParseTests(lines);
            logger?.WriteLine("1a");

            var instrs = new HashSet<IInstr>(ChronCPU.GetInstructions());

            logger?.WriteLine("1b");

            Dictionary<Test, HashSet<IInstr>> testMatches = new();
            foreach (var test in tests)
            {
                testMatches[test] = new();
                foreach (var instr in instrs)
                {
                    if (DoTest(test, instr)) testMatches[test].Add(instr);
                }
            }

            return testMatches;
        }

        public static int Part1(string input)
        {
            var testLines = input.Split("\n\n");

            Dictionary<Test, HashSet<IInstr>> testMatches = RunTests(testLines);
            return testMatches.Count(kvp => kvp.Value.Count >= 3);
        }

        public static long Part2(string input, ILogger logger)
        {
            logger.WriteLine("0");
            var data = input.Split("\n\n\n");
            var testLines = data[0].Split("\n\n");

            logger.WriteLine("1");

            Dictionary<Test, HashSet<IInstr>> testMatches = RunTests(testLines, logger);

            logger.WriteLine("2");

            var mapping = new Dictionary<int, IInstr>();
            while (mapping.Count < 16)
            {
                foreach (var match in testMatches.Where(kvp => kvp.Value.Count == 1))
                {
                    var matched = match.Value.First();
                    mapping[match.Key.instr] = matched;
                    testMatches.Remove(match.Key);

                    foreach (var other in testMatches)
                    {
                        other.Value.Remove(matched);
                    }
                }
            }

            logger.WriteLine("3");

            var cpu = new ChronCPU(Util.Split(data[1], '\n'), mapping);

            logger.WriteLine("4");

            cpu.Run();
            Console.WriteLine(cpu.Speed());

            logger.WriteLine("5");

            return cpu.Get(0);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}
