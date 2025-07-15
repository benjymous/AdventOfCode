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

    internal class Instructions
    {
        public static IEnumerable<(string key, Func<Variant[], DataBus, int> instr)> Get(string sets)
        {
            var isets = sets.Split(",").ToHashSet();

            if (isets.Contains("Common"))
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

            if (isets.Contains("Day18"))
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

                yield return ("jgz", (v, bus) => v[0] > 0 ? (int)v[1] : 1);
            }

            if (isets.Contains("Day18Part1"))
            {
                yield return ("rcv", (v, bus) => v[0] != 0 ? 9999 : 1);
            }

            if (isets.Contains("Day18Part2"))
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

            if (isets.Contains("Day23"))
            {
                yield return ("jnz", (v, bus) => v[0] != 0 ? (int)v[1] : 1);

                yield return ("sub", (v, bus) =>
                {
                    bus.Registers[v[0].Value] -= v[1];
                    return 1;
                }
                );
            }
        }
    }

    readonly public record struct InstructionLine(int Index, string Name, Func<Variant[], DataBus, int> Instr, Variant[] Args, DataBus Bus)
    {
        public int Exec() => Instr(Args, Bus);
    }

    public class Coprocessor
    {
        private readonly InstructionLine[] Program;

        public readonly DataBus Bus = new();

        private System.Diagnostics.Stopwatch sw;
        private long CycleCount = 0;

        public Func<InstructionLine, bool> Debugger = null;

        public Coprocessor(string input, string instructionSet = "Common")
        {
            var opcodes = Instructions.Get(instructionSet).ToDictionary(i => i.key, i => i.instr);

            List<InstructionLine> instrs = [];
            foreach (var line in Util.Split(input, "\n").WithoutNullOrWhiteSpace())
            {
                var mnemonic = line[..3];
                if (opcodes.TryGetValue(mnemonic, out var func))
                {
                    var opargs = Parser.Parse<Variant>(line[4..], " ").ToArray();
                    opargs.ForEach(v => v.Bus = Bus);

                    instrs.Add(new InstructionLine(instrs.Count, mnemonic, func, opargs, Bus));
                }
                else
                {
                    throw new Exception($"Unknown mnemonic {mnemonic}");
                }
            }
            Program = [.. instrs];
        }

        public bool Step()
        {
            CycleCount++;
            if (InstructionPointer >= Program.Length) return false;

            var line = Program[InstructionPointer];

            if (Debugger != null && (Debugger(line) == false)) return false;
            InstructionPointer += line.Exec();

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

        private int InstructionPointer = 0;

        public long Get(char idx) => Bus.Registers[idx - 'a'];
        public void Set(char idx, int val) => Bus.Registers[idx - 'a'] = val;

    }
}