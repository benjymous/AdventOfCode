using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018.ChronMatic
{
    public interface IInstr
    {
        void Do(ref int[] args, ref long[] regs);
        string Dump(int a, int b, int c);
    }

#pragma warning disable IDE1006 // Naming Styles
    namespace Instructions
    {
        class @addr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] + regs[args[1]];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} + r{b};";
        }
        class @addi : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] + args[1];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} + {b};";
        }

        class @mulr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] * regs[args[1]];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} * r{b};";
        }
        class @muli : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] * args[1];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} * {b};";
        }

        class @banr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] & regs[args[1]];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} & r{b};";
        }
        class @bani : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] & args[1];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} & {b};";
        }

        class @borr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] | regs[args[1]];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} | r{b};";
        }
        class @bori : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]] | (long)args[1];
            public string Dump(int a, int b, int c) => $"r{c} = r{a} | {b};";
        }

        class @setr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = regs[args[0]];
            public string Dump(int a, int b, int c) => $"r{c} = r{a};";
        }
        class @seti : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = args[0];
            public string Dump(int a, int b, int c) => $"r{c} = {a};";
        }

        class @gtir : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = (args[0] > regs[args[1]]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = ({a} > r{b}) ? 1 : 0;";
        }
        class @gtri : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = (regs[args[0]] > args[1]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} > {b}) ? 1 : 0;";
        }
        class @gtrr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = (regs[args[0]] > regs[args[1]]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} > r{b}) ? 1 : 0;";
        }

        class @eqir : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = (args[0] == regs[args[1]]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = ({a} == r{b}) ? 1 : 0;";
        }
        class @eqri : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = (regs[args[0]] == args[1]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} == {b}) ? 1 : 0;";
        }
        class @eqrr : IInstr
        {
            public void Do(ref int[] args, ref long[] regs) => regs[args[2]] = (regs[args[0]] == regs[args[1]]) ? 1 : 0;
            public string Dump(int a, int b, int c) => $"r{c} = (r{a} == r{b}) ? 1 : 0;";
        }

        public static class All
        {
            public static List<IInstr> Get()
            {
                return new List<IInstr> { new addr(), new addi(), new mulr(), new muli(), new banr(), new bani(), new borr(), new bori(), new setr(), new seti(), new gtir(), new gtri(), new gtrr(), new eqir(), new eqri(), new eqrr() };
            }
        }
    }
#pragma warning restore IDE1006 // Naming Styles

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

        internal readonly string Dump() => instr.Dump(values[0], values[1], values[2]);
    }

    public class ChronCPU
    {

        public static IEnumerable<IInstr> GetInstructions()
        {
            return ChronMatic.Instructions.All.Get();
        }

        readonly Dictionary<int, IInstr> InstrMap;
        readonly InstructionLine[] Instructions;

        long[] Registers = new long[] { 0, 0, 0, 0, 0, 0 };

        System.Diagnostics.Stopwatch sw;
        private int CycleCount = 0;


        public ChronCPU(string program, Dictionary<int, IInstr> instrMap = null) : this(Util.Split(program, '\n'), instrMap)
        { }

        public ChronCPU(IEnumerable<string> program, Dictionary<int, IInstr> instrMap = null)
        {

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
                var opcodes = GetInstructions().ToDictionary(i => i.Name(), i => i);
                foreach (var instr in opcodes)
                {
                    int idx = reverseMap.Count;
                    reverseMap[instr.Key] = reverseMap.Count;
                    InstrMap[idx] = instr.Value;
                }
            }

            List<InstructionLine> instrs = new();
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

        //public int PeekTime = 0;



        public void Run()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (InstructionPointer < Instructions.Length)
            {
                CycleCount++;

                var line = Instructions[InstructionPointer];
                line.instr.Do(ref line.values, ref Registers);
                InstructionPointer++;
            }

            sw.Stop();
        }

        public IEnumerable<string> Dump(bool useGotos)
        {
            for (int i = 0; i < Instructions.Length; ++i)
            {
                yield return $"// {Instructions[i].instr.Name()} {string.Join(" ", Instructions[i].values)}";
                var dump = Instructions[i].Dump();

                dump = dump.Replace(";", ";break;\n");

                //if (useGotos==false || dump.StartsWith("r1 ="))
                //{
                //    dump = dump.Replace(";", " +1; break;\n");
                //}
                //else
                //{
                //    dump += $" r1++; goto case {i + 1};";
                //}
                yield return $"case {i}: {dump}";
            }
        }

        public string Speed()
        {
            var speed = (double)CycleCount / sw.Elapsed.TotalSeconds;
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz";
        }

        long pc = 0;
        bool pcIsRef = false;
        public long InstructionPointer
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

        public long Get(int idx) => Registers[idx];
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