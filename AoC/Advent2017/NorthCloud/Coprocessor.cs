namespace AoC.Advent2017.NorthCloud
{
    public interface IOutputPort
    {
        void Write(long value);
    }

    public interface IInputPort
    {
        bool HasData();
        long Read();
    }

    public class DataBus
    {
        public long[] Registers = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

        public IOutputPort Output;
        public IInputPort Input;

        public bool Waiting { get; internal set; } = false;
    }

    public class Variant
    {
        [Regex("(.+)")]
        public Variant(string input) =>
            (Value, IsReg) = (input.Length == 1 && input[0] >= 'a' && input[0] <= 'z')
                ? (input[0] - 'a', true)
                : (long.Parse(input), false);

        public DataBus Bus { get; set; }

        public bool IsReg { get; private set; } = false;
        public long Value { get; private set; } = 0;

        public static implicit operator long(Variant v) => v.IsReg ? v.Bus.Registers[v.Value] : v.Value;

        public override string ToString() => IsReg ? ((char)(Value + 'a')).ToString() : $"{Value}";
    }

    [Flags]
    public enum InstructionSet
    {
        Common = 1,
        SoundExtensionsBase = 2, // Day 18
        SoundExtensionsV1 = 4,
        SoundExtensionsV2 = 8,
        Alt = 16, // Day 23

        Day18Part1 = Common | SoundExtensionsBase | SoundExtensionsV1,
        Day18Part2 = Common | SoundExtensionsBase | SoundExtensionsV2,

        Day23 = Common | Alt,
    }

    internal class Instructions
    {
        public static IEnumerable<(string key, Func<Variant[], DataBus, long> instr)> Get(InstructionSet sets)
        {
            if (sets.HasFlag(InstructionSet.Common))
            {
                yield return ("set", (v, bus) =>
                {
                    bus.Registers[v[0].Value] = v[1];
                    return 1;
                }
                );

                yield return ("mul", (v, bus) =>
                {
                    bus.Registers[v[0].Value] *= v[1];
                    return 1;
                }
                );
            }

            if (sets.HasFlag(InstructionSet.SoundExtensionsBase))
            {
                yield return ("snd", (v, bus) =>
                {
                    bus.Output.Write(v[0]);
                    return 1;
                }
                );

                yield return ("add", (v, bus) =>
                {
                    bus.Registers[v[0].Value] += v[1];
                    return 1;
                }
                );

                yield return ("mod", (v, bus) =>
                {
                    bus.Registers[v[0].Value] %= v[1];
                    return 1;
                }
                );

                yield return ("jgz", (v, bus) => v[0] > 0 ? v[1] : 1);
            }

            if (sets.HasFlag(InstructionSet.SoundExtensionsV1))
            {
                yield return ("rcv", (v, bus) => v[0] != 0 ? 9999 : 1);
            }

            if (sets.HasFlag(InstructionSet.SoundExtensionsV2))
            {
                yield return ("rcv", (v, bus) =>
                {
                    if (bus?.Input.HasData() == true)
                    {
                        bus.Waiting = false;
                        bus.Registers[v[0].Value] = bus.Input.Read();
                        return 1;
                    }
                    else
                    {
                        bus.Waiting = true;
                        return 0;
                    }
                }
                );
            }

            if (sets.HasFlag(InstructionSet.Day23))
            {
                yield return ("jnz", (v, bus) => v[0] != 0 ? (int)v[1].Value : 1);

                yield return ("sub", (v, bus) =>
                {                   
                    bus.Registers[v[0].Value] -= v[1];
                    return 1;
                }
                );
            }
        }
    }

    readonly public record struct InstructionLine(int Index, string Name, Func<Variant[], DataBus, long> Instr, Variant[] Args)
    {
        public long Exec(DataBus bus) => Instr(Args, bus);
    }

    public class Coprocessor
    {
        private readonly InstructionLine[] Program;

        public readonly DataBus Bus = new();

        private System.Diagnostics.Stopwatch sw;
        private long CycleCount = 0;

        public Func<InstructionLine, bool> Debugger = null;

        public Coprocessor(string input, InstructionSet instructionSet = InstructionSet.Common) 
            => Program = [.. Parser.Factory<InstructionLine, Factory>(input, new Factory(instructionSet, Bus))];

        public class Factory(InstructionSet instructionSet, DataBus bus)
        {
            private readonly Dictionary<string, Func<Variant[], DataBus, long>> Opcodes = Instructions.Get(instructionSet).ToDictionary(i => i.key, i => i.instr);
            private readonly DataBus Bus = bus;
            int Count = 0;

            [Regex(@"(\S+) (.*)")]
            public InstructionLine ParseLine(string mnemonic, [Split(" ")] Variant[] opargs)
            {
                if (Opcodes.TryGetValue(mnemonic, out var func))
                {
                    opargs.ForEach(v => v.Bus = Bus);
                    return new InstructionLine(Count++, mnemonic, func, opargs);
                }
                else
                {
                    throw new Exception($"Unknown mnemonic {mnemonic}");
                }
            }
        }

        public bool Step()
        {
            CycleCount++;
            if (InstructionPointer >= Program.Length) return false;

            var line = Program[InstructionPointer];

            if (Debugger != null && (!Debugger(line))) return false;
            InstructionPointer += line.Exec(Bus);

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
            var speed = CycleCount / sw.Elapsed.TotalSeconds;
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz";
        }

        private long InstructionPointer = 0;

        public long Get(char idx) => Bus.Registers[idx - 'a'];
        public void Set(char idx, int val) => Bus.Registers[idx - 'a'] = val;

    }
}