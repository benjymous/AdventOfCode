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
            public int[] before;
            public int[] after;
            public int instr;
            public int[] args;

            public Test(string b, string i, string a)
            {
                before = Util.ExtractNumbers(b);
                after = Util.ExtractNumbers(a);
                var data = Util.ExtractNumbers(i);
                instr = data[0];
                args = data.Skip(1).ToArray();
            }
        }


        private static List<Test> ParseTests(string[] lines)
        {
            List<Test> tests = new List<Test>();

            string before = null;
            string instr = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("Before"))
                {
                    before = line;
                }
                else if (line.StartsWith("After"))
                {
                    var test = new Test(before, instr, line);
                    tests.Add(test);
                }
                else
                {
                    instr = line;
                }
            }

            return tests;
        }

        private static bool Match(int[] data, int[] after)
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

        static bool DoTest(Test test, ChronMatic.IInstr instr)
        {
            var data = test.before.ToArray();
            instr.Do(test.args[0], test.args[1], test.args[2], ref data);
            return Match(data, test.after);
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input, '\n');

            List<Test> tests = ParseTests(lines);

            var instrs = ChronMatic.ChronCPU.GetInstructions();

            int count = 0;
            foreach (var test in tests)
            {
                int match = 0;
                foreach (var i in instrs)
                {
                    if (DoTest(test, i)) match++;
                }
                if (match >= 3) count++;
            }

            return count;
        }

        public static int Part2(string input)
        {
            var lines = input.Split('\n');

            IEnumerable<Test> tests = ParseTests(lines).OrderBy(t => t.instr);

            var instrs = new HashSet<ChronMatic.IInstr>(ChronMatic.ChronCPU.GetInstructions());

            var mapping = new Dictionary<int, ChronMatic.IInstr>();

            while (mapping.Count < 16)
            {
                for (int i = 0; i < 16; ++i)
                {
                    if (mapping.ContainsKey(i)) continue;
                    HashSet<ChronMatic.IInstr> potentials = new HashSet<ChronMatic.IInstr>(instrs);
                    foreach (var test in tests.Where(t => t.instr == i))
                    {
                        HashSet<ChronMatic.IInstr> pass = new HashSet<ChronMatic.IInstr>();
                        foreach (var instr in potentials)
                        {
                            if (DoTest(test, instr))
                            {
                                pass.Add(instr);
                            }
                        }
                        potentials = pass;
                    }

                    if (potentials.Count() == 0)
                    {
                        throw new Exception($"Failed to map {i}");
                    }
                    if (potentials.Count() == 1)
                    {
                        var instr = potentials.First();
                        //Console.WriteLine($"instr {i} is {instr.GetType().Name}");
                        mapping[i] = instr;
                        instrs.Remove(instr);
                    }
                    else
                    {
                        //Console.WriteLine($"instr {i} has {potentials.Count()} potentials"); 
                    }
                }
            }

            // find the three blank lines that indicate the start of the program
            int progStart = 0;
            for (int i = 3; i < lines.Length; ++i)
            {
                if (lines[i] == lines[i - 1] && lines[i] == lines[i - 2])
                {
                    progStart = i + 1;
                    break;
                }
            }

            var progLines = lines.Skip(progStart);

            var cpu = new ChronMatic.ChronCPU(progLines, mapping);
            cpu.Run();
            return cpu.Get(0);

            // List<int[]> program = new List<int[]>();
            // foreach (var line in progLines)
            // {
            //     if (!string.IsNullOrEmpty(line))
            //     {
            //         program.Add(Util.ExtractNumbers(line));
            //     }
            // }

            // var regs = new int[]{0,0,0,0};
            // foreach (var line in program)
            // {
            //     var instr = mapping[line[0]];
            //     instr.Do(line[1], line[2], line[3], ref regs);
            // }

            // return regs[0];
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
