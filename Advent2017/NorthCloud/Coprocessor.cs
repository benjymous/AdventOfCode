using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2017.NorthCloud
{
    public interface IOutputPort
    {
        void Write(Int64 value);
    }

    public interface IInputPort
    {
        bool Read(out Int64 value);
    }

    public class DataBus
    {
        public Int64[] Registers = new Int64[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public IOutputPort Output;
        public IInputPort Input;

        public bool Waiting { get; internal set; } = false;

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 26; ++i)
            {
                sb.Append(((char)(i + 'a')));
                sb.Append(":");
                sb.Append(Registers[i]);
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }

    public class Variant
    {
        public Variant(string input)
        {
            if (input.Length == 1 && input[0] >= 'a' && input[0] <= 'z')
            {
                Value = input[0] - 'a';
                IsReg = true;
            }
            else
            {
                Value = Int64.Parse(input);
            }
        }

        public bool IsReg { get; private set; } = false;
        public Int64 Value { get; private set; } = 0;

        public Int64 Read(DataBus bus)
        {
            if (IsReg)
            {
                return bus.Registers[Value];
            }
            else
            {
                return Value;
            }
        }

        public override string ToString()
        {
            if (IsReg)
            {
                return ((char)(Value + 'a')).ToString();
            }
            else
            {
                return $"{Value}";
            }
        }

        public static Variant Null { get; } = new Variant("0");
    }

    public interface IInstr
    {
        int Do(Variant x, Variant y, DataBus bus);
    }

    public interface IDebugger
    {
        void Next(int IP, IInstr instr, Variant x, Variant y, DataBus bus);
    }


    namespace Instructions
    {
        namespace Common
        {
            class set : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    bus.Registers[x.Value] = y.Read(bus);
                    return 1;
                }
            }

            class mul : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    bus.Registers[x.Value] *= y.Read(bus);
                    return 1;
                }
            }
        }

        namespace Day18
        {
            class snd : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    bus.Output.Write(x.Read(bus));
                    return 1;
                }
            }

            class add : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    bus.Registers[x.Value] += y.Read(bus);
                    return 1;
                }
            }

            class mod : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    bus.Registers[x.Value] %= y.Read(bus);
                    return 1;
                }
            }



            class jgz : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    if (x.Read(bus) > 0)
                    {
                        return (int)y.Read(bus);
                    }
                    return 1;
                }
            }

        }

        namespace Day18Part1
        {
            class rcv : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    if (x.Read(bus) != 0)
                    {
                        // halt execution
                        return 9999;
                    }
                    return 1;
                }
            }
        }

        namespace Day18Part2
        {
            class rcv : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    Int64 value = 0;
                    if (bus.Input == null || bus.Input.Read(out value) == false)
                    {
                        bus.Waiting = true;
                        return 0;
                    }

                    bus.Waiting = false;
                    bus.Registers[x.Value] = value;
                    return 1;
                }
            }
        }

        namespace Day23
        {
            class jnz : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    if (x.Read(bus) != 0)
                    {
                        return (int)y.Read(bus);
                    }
                    return 1;
                }
            }

            class sub : IInstr
            {
                public int Do(Variant x, Variant y, DataBus bus)
                {
                    bus.Registers[x.Value] -= y.Read(bus);
                    return 1;
                }
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

    struct InstructionLine
    {
        public InstructionLine(IInstr i, Variant[] v)
        {
            instr = i;
            if (v.Length == 2)
            {
                values = v;
            }
            else
            {
                values = new Variant[] { v[0], Variant.Null };
            }
        }
        public IInstr instr;
        public Variant[] values;

        public override string ToString() => $"{instr.Name()} {values[0]} {values[1]}";

    }

    public class Coprocessor
    {

        public static IEnumerable<IInstr> GetInstructions(string instructionSet)
        {
            var isets = instructionSet.Split(",");
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IInstr).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Where(x => isets.Where(n => x.Namespace.Contains(n)).Any())
                .Select(x => (IInstr)Activator.CreateInstance(x));
        }

        Dictionary<int, IInstr> InstrMap;
        InstructionLine[] Instructions;

        public DataBus Bus { get; private set; } = new DataBus();

        System.Diagnostics.Stopwatch sw;
        Int64 CycleCount = 0;

        public IDebugger Debugger = null;


        public Coprocessor(string input, string instructionSet = "Common")
        {
            var opcodes = GetInstructions(instructionSet).ToDictionary(i => i.Name(), i => i);

            Dictionary<string, int> reverseMap;
            reverseMap = new Dictionary<string, int>();
            InstrMap = new Dictionary<int, IInstr>();
            foreach (var instr in opcodes)
            {
                int idx = reverseMap.Count();
                reverseMap[instr.Key] = reverseMap.Count();
                InstrMap[idx] = instr.Value;
            }

            List<InstructionLine> instrs = new List<InstructionLine>();
            var program = Util.Split(input, '\n');
            foreach (var line in program)
            {
                if (string.IsNullOrEmpty(line)) continue;

                var bits = line.Split(" ");
                if (reverseMap.TryGetValue(bits[0], out int icode))
                {
                    // text mnemonic;
                    var rest = Util.Parse<Variant>(line.Substring(4), " ").ToArray();

                    instrs.Add(new InstructionLine(InstrMap[icode], rest));
                }
                else
                {
                    throw new Exception($"Unknown mnemonic {bits[0]}");
                }

            }
            Instructions = instrs.ToArray();
        }

        public int PeekTime = 0;

        public bool Step()
        {
            CycleCount++;
            if (InstructionPointer >= Instructions.Length)
                return false;

            //if (PeekTime > 0 && CycleCount % PeekTime == 0)
            //{
            //    Console.WriteLine(Speed());
            //    Console.WriteLine(string.Join(", ", Bus.Registers));
            //}

            var line = Instructions[InstructionPointer];

            Debugger?.Next(InstructionPointer, line.instr, line.values[0], line.values[1], Bus);
            InstructionPointer += line.instr.Do(line.values[0], line.values[1], Bus);

            return true;
        }

        public void Run()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (Step()) ;
            sw.Stop();
        }

        public string Speed()
        {
            var speed = (double)CycleCount / sw.Elapsed.TotalSeconds;
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz";
        }

        public int InstructionPointer { get; set; } = 0;


        public Int64 Get(char idx) => Bus.Registers[idx - 'a'];
        public void Set(char idx, int val) => Bus.Registers[idx - 'a'] = val;

    }
}