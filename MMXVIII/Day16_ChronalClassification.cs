using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day16 : IPuzzle
    {
        public string Name { get { return "2018-16";} }
 
        public interface IInstr
        {
            void Do(int a, int b, int c, ref int[] regs);
        }

        class addr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]+regs[b];
            }
        }
        class addi : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]+b;
            }
        }

        class mukr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]*regs[b];
            }
        }
        class muli : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]*b;
            }
        }

        class banr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]&regs[b];
            }
        }
        class bani : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]&b;
            }
        }

        class borr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]|regs[b];
            }
        }
        class bori : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a]|b;
            }
        }

        class setr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a];
            }
        }
        class seti : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = a;
            }
        }

        class gtir : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (a>regs[b]) ? 1 : 0;
            }
        }
        class gtri : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a]>b) ? 1 : 0;
            }
        }
        class gtrr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a]>regs[b]) ? 1 : 0;
            }
        }

        class eqir : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (a==regs[b]) ? 1 : 0;
            }
        }
        class eqri : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a]==b) ? 1 : 0;
            }
        }
        class eqrr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a]==regs[b]) ? 1 : 0;
            }
        }

        static IEnumerable<IInstr> GetInstructions()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IInstr).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (IInstr)Activator.CreateInstance(x));
        }

        public class Test
        {
            public int[] before;
            public int[] after;
            public int[] instr;

            public Test(string b, string i, string a)
            {
                before = Util.ExtractNumbers(b);
                after = Util.ExtractNumbers(a);
                instr = Util.ExtractNumbers(i);
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
            for (int i=0; i<4; ++i)
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
            instr.Do(test.instr[1], test.instr[2], test.instr[3], ref data);
            return Match(data, test.after);
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input, '\n');

            List<Test> tests = ParseTests(lines);

            var instrs = GetInstructions();

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

            IEnumerable<Test> tests = ParseTests(lines).OrderBy(t => t.instr[0]);

            var instrs = new HashSet<IInstr>(GetInstructions());

            var mapping = new Dictionary<int, IInstr>();

            while (mapping.Count < 16)
            {
                for (int i=0; i<16; ++i)
                {
                    if (mapping.ContainsKey(i)) continue;
                    HashSet<IInstr> potentials = new HashSet<IInstr>(instrs);
                    foreach (var test in tests.Where(t => t.instr[0]==i))
                    {
                        HashSet<IInstr> pass = new HashSet<IInstr>();
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
                        mapping[i]=instr;
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
            for (int i=3; i<lines.Length; ++i)
            {
                if (lines[i]==lines[i-1] && lines[i]==lines[i-2])
                {
                    progStart = i+1;
                    break;
                }
            }

            var progLines = lines.Skip(progStart);

            List<int[]> program = new List<int[]>();
            foreach (var line in progLines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                program.Add(Util.ExtractNumbers(line));
                }
            }

            var regs = new int[]{0,0,0,0};
            foreach (var line in program)
            {
                var instr = mapping[line[0]];
                instr.Do(line[1], line[2], line[3], ref regs);
            }

            return regs[0];
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
