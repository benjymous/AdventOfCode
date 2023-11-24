namespace AoC.Advent2016.BunniTek
{
    public enum OpCode
    {
        inc = 0,
        dec = 1,
        cpy = 2,
        jnz = 3,
        tgl = 4,
        @out = 5,

        mul = 6,
        nop = 7,
    }

    public enum RegisterId
    {
        a = 0, b = 1, c = 2, d = 3
    };

    public readonly record struct Value(int IntVal, bool IsInt)
    {
        public static implicit operator Value(int intVal) => new(intVal, true);
        public static implicit operator Value(RegisterId regIndex) => new((int)regIndex, false);
        public static implicit operator Value(string input) => input[0] >= 'a' ? new Value(input[0] - 'a', false) : new Value(int.Parse(input), true);
    }

    public class Instruction
    {
        public OpCode Opcode;
        public readonly Value X, Y;

        public Instruction(string line)
        {
            var bits = line.Split(" ");
            Opcode = Enum.Parse<OpCode>(bits[0]);

            if (bits.Length >= 2) X = bits[1];
            if (bits.Length == 3) Y = bits[2];
        }
    }

    public class BunnyCPU(Instruction[] program)
    {
        readonly Instruction[] Instructions = [.. program];
        readonly int[] Registers = [0, 0, 0, 0];

        System.Diagnostics.Stopwatch sw;
        long CycleCount = 0;

        public Func<int, bool> Output = null;

        public static Instruction[] Compile(string program) => [.. Util.Parse<Instruction>(program)];

        public BunnyCPU(string program) : this(Compile(program)) { }

        public void Set(RegisterId id, Value source) => Registers[(int)id] = Get(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get(Value source) => source.IsInt ? source.IntVal : Registers[source.IntVal];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool GetInstr(int idx, out Instruction instr) => (instr = idx >= 0 && idx < Instructions.Length ? Instructions[idx] : null) != null;

        public void Run()
        {
            sw = new();
            sw.Start();
            bool running = true;
            int ip = 0;
            while (running)
            {
                CycleCount++;

                if (!GetInstr(ip, out var instr)) break;

                switch (instr.Opcode)
                {
                    case OpCode.inc:
                        Registers[instr.X.IntVal]++;
                        break;

                    case OpCode.dec:
                        Registers[instr.X.IntVal]--;
                        break;

                    case OpCode.cpy:
                        Registers[instr.Y.IntVal] = Get(instr.X);
                        break;

                    case OpCode.jnz:
                        if (Get(instr.X) != 0) ip += Get(instr.Y) - 1;
                        break;

                    case OpCode.tgl:
                        if (GetInstr(ip + Get(instr.X), out var otherInstr)) otherInstr.Opcode += ((int)otherInstr.Opcode & 1) == 0 ? 1 : -1;
                        break;

                    case OpCode.@out:
                        if (Output != null && !Output(Get(instr.X))) running = false;
                        break;

                    case OpCode.mul:
                        Registers[instr.Y.IntVal] *= Get(instr.X);
                        break;

                    case OpCode.nop:
                        break;
                }

                ip++;
            }
            sw.Stop();
        }

        public string Speed() => $"{CycleCount} cycles - {((double)(CycleCount / sw.Elapsed.TotalSeconds)).ToEngineeringNotation()}hz";
    }
}