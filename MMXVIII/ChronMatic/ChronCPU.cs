using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Utils;

namespace Advent.MMXVIII.ChronMatic
{
    public interface IInstr
    {
        void Do(int a, int b, int c, ref int[] regs);
        string Dump(int a, int b, int c);
    }

    namespace Instructions
    {
        class addr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] + regs[b];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} + r{b};";
        }
        class addi : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] + b;
             public string Dump(int a, int b, int c) => $"r{c} = r{a} + {b};";
        }

        class mulr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] * regs[b];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} * r{b};";
        }
        class muli : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] * b;
            public string Dump(int a, int b, int c) => $"r{c} = r{a} * {b};";
        }

        class banr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] & regs[b];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} & r{b};";
        }
        class bani : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] & b;
            public string Dump(int a, int b, int c) => $"r{c} = r{a} & {b};";
        }

        class borr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] | regs[b];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} | r{b};";
        }
        class bori : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a] | b;
            public string Dump(int a, int b, int c) => $"r{c} = r{a} | {b};";
        }

        class setr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = regs[a];
            public string Dump(int a, int b, int c) => $"r{c} = r{a};";
        }
        class seti : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = a;
            public string Dump(int a, int b, int c) => $"r{c} = {a};";
        }

        class gtir : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = (a > regs[b]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = ({a} > r{b}) ? 1 : 0;";
        }
        class gtri : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = (regs[a] > b) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} > {b}) ? 1 : 0;";
        }
        class gtrr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = (regs[a] > regs[b]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} > r{b}) ? 1 : 0;";
        }

        class eqir : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = (a == regs[b]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = ({a} == r{b}) ? 1 : 0;";
        }
        class eqri : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = (regs[a] == b) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} == {b}) ? 1 : 0;";
        }
        class eqrr : IInstr
        {
            public void Do(int a, int b, int c, ref int[] regs) => regs[c] = (regs[a] == regs[b]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} == r{b}) ? 1 : 0;";
        }
    }

    public static class Extensions
    {
        public static string Name(this IInstr instr)
        {
            return instr.GetType().Name;
        }
    }

    struct InstructionLine
    {
        public InstructionLine(IInstr i, int[] v)
        {
            instr = i;
            values = v;
        }
        public IInstr instr;
        public int[] values;

        internal string Dump() => instr.Dump(values[0],values[1],values[2]);
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
        InstructionLine[] Instructions;

        int[] Registers = new int[]{0,0,0,0,0,0};

        System.Diagnostics.Stopwatch sw;
        Int64 CycleCount = 0;

    
        public ChronCPU(string program, Dictionary<int, IInstr> instrMap = null) : this(Util.Split(program, '\n'), instrMap)
        {}

        public ChronCPU(IEnumerable<string> program, Dictionary<int, IInstr> instrMap = null)
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

            List<InstructionLine> instrs = new List<InstructionLine>();
            foreach (var line in program)
            {
                if (string.IsNullOrEmpty(line)) continue;

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

                        instrs.Add(new InstructionLine(InstrMap[icode], rest));
                    }
                    else
                    {
                        var codes = Util.ExtractNumbers(line);
                        if (codes.Length == 4)
                        {
                            instrs.Add(new InstructionLine(InstrMap[codes[0]], codes.Skip(1).ToArray()));
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

        public int PeekTime = 0;
        public bool Log = false;

        public bool Step()
        {
            CycleCount++;
            if (InstructionPointer >= Instructions.Length) return false;


            if (PeekTime > 0 && CycleCount % PeekTime == 0)
            {
                Console.WriteLine(Speed());
                Console.WriteLine(string.Join(", ", Registers));
            }

            var line = Instructions[InstructionPointer];

            if (Log)
            {
                Console.WriteLine($"[{InstructionPointer}] [{string.Join(", ", Registers)}] {line.instr.Name()} : {string.Join(" ",line.values)}");
            }

            line.instr.Do(line.values[0], line.values[1], line.values[2], ref Registers);
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

        public IEnumerable<string> Dump()
        {
            for (int i=0; i<Instructions.Length; ++i)
            {
                yield return $"// {Instructions[i].instr.Name()} {string.Join(" ", Instructions[i].values)}";
                var dump = Instructions[i].Dump();
                
                if (dump.StartsWith("r1 ="))
                { 
                    dump = dump.Replace(";", " +1; break;\n");
                }
                else
                {
                    dump += $" r1++; goto case {i+1};";
                }
                yield return $"case {i}: {dump}";
            }
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