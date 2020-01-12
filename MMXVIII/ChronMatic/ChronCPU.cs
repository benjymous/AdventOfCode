using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXVIII.ChronMatic
{
    public interface IInstr
    {
        void Do(int a, int b, int c, ref int[] regs);
    }


    namespace Instructions
    {
        class addr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] + regs[b];
            }
        }
        class addi : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] + b;
            }
        }

        class mulr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] * regs[b];
            }
        }
        class muli : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] * b;
            }
        }

        class banr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] & regs[b];
            }
        }
        class bani : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] & b;
            }
        }

        class borr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] | regs[b];
            }
        }
        class bori : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = regs[a] | b;
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
                regs[c] = (a > regs[b]) ? 1 : 0;
            }
        }
        class gtri : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a] > b) ? 1 : 0;
            }
        }
        class gtrr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a] > regs[b]) ? 1 : 0;
            }
        }

        class eqir : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (a == regs[b]) ? 1 : 0;
            }
        }
        class eqri : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a] == b) ? 1 : 0;
            }
        }
        class eqrr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs)
            {
                regs[c] = (regs[a] == regs[b]) ? 1 : 0;
            }
        }
    }

    public static class Extensions
    {
        public static string Name(this IInstr instr)
        {
            return instr.GetType().Name;
        }
    }

    public class ChronCPU
    {

        public static IEnumerable<IInstr> GetInstructions()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IInstr).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (IInstr)Activator.CreateInstance(x));
        }

        Dictionary<int, IInstr> InstrMap;
        int[][] Instructions;

        int[] Registers = new int[]{0,0,0,0,0,0};

        System.Diagnostics.Stopwatch sw;
        Int64 CycleCount = 0;

        public ChronCPU(string program, Dictionary<int, IInstr> instrMap = null)
        {

            var opcodes = GetInstructions().ToDictionary(i => i.Name(), i => i);

            Dictionary<string, int> reverseMap;
            if (instrMap != null)
            {
                InstrMap = instrMap;
                reverseMap = instrMap.ToDictionary(kvp => kvp.Value.Name(), kvp => kvp.Key);
            }
            else
            {
                reverseMap = new Dictionary<string, int>();
                InstrMap = new Dictionary<int, IInstr>();
                foreach (var instr in opcodes)
                {
                    int idx = reverseMap.Count();
                    reverseMap[instr.Key] = reverseMap.Count();
                    InstrMap[idx] = instr.Value;
                }
            }

            var lines = Util.Split(program, '\n');
            List<int[]> instrs = new List<int[]>();
            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    Preprocess(line);
                }
                else
                {
                    var bits = line.Split(" ");
                    if (reverseMap.TryGetValue(bits[0], out int icode))
                    {
                        // text mnemonic;
                        var rest = Util.ExtractNumbers(line);

                        instrs.Add(new int[] { icode, rest[0], rest[1], rest[2] });
                    }
                    else
                    {
                        var codes = Util.ExtractNumbers(line);
                        if (codes.Length == 4)
                        {
                            instrs.Add(codes);
                        }
                        else
                        {
                            throw new Exception("Failed to parse instruction line");
                        }
                    }
                }
            }
            Instructions = instrs.ToArray();
        }

        public bool Step()
        {
            CycleCount++;
            if (InstructionPointer >= Instructions.Length) return false;

            var line = Instructions[InstructionPointer];
            var instr = InstrMap[line[0]];
            instr.Do(line[1], line[2], line[3], ref Registers);
            InstructionPointer++;

            return true;
        }

        public void Run()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while(Step());
            sw.Stop();
        }

        public string Speed()
        {
            var speed = (double)CycleCount / sw.Elapsed.TotalSeconds;
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz";
        }

        int pc = 0;
        bool pcIsRef = false;
        public int InstructionPointer 
        {
            get
            {
                if (pcIsRef)
                {
                    return Registers[pc];
                }
                else
                {
                    return pc;
                }
            }
            set
            {
                if (pcIsRef)
                {
                    Registers[pc] = value;
                }
                else
                {
                    pc = value;
                }
            }
        }

        public int Get(int idx) => Registers[idx];
        public void Set(int idx, int val) => Registers[idx] = val;

        void Preprocess(string line)
        {
            if (line.StartsWith("#ip"))
            {
                var nums = Util.ExtractNumbers(line);
                pcIsRef = true;
                pc = nums[0];
            }
        }

    }
}