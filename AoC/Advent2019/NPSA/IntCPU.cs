using System.Threading;

namespace AoC.Advent2019.NPSA
{
    public interface ICPUInterrupt
    {
        void RequestInput();
        void OutputReady();
    }

    public class IntCPU
    {
        enum Opcode
        {
            ADD = 1,
            MUL = 2,

            GET = 3,
            OUT = 4,

            JNZ = 5,     // Jump if not zero 
            JZ = 6,      // Jump if zero

            LT = 7,
            EQ = 8,

            SETR = 9,

            HALT = 99,
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte InstructionSize(Opcode code) => code switch
        {
            Opcode.HALT => 1,
            Opcode.GET or Opcode.OUT or Opcode.SETR => 2,
            Opcode.JNZ or Opcode.JZ => 3,
            Opcode.ADD or Opcode.MUL or Opcode.LT or Opcode.EQ => 4,
            _ => throw new Exception("Bad instruction"),
        };

        enum ParamMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2
        }

        long[] Memory;
        readonly IEnumerable<long> initialState;
        long InstructionPointer = 0;
        long RelBase = 0;

        public void AddInput(params long[] values) => Input.EnqueueRange(values);
        public Queue<long> Input { get; set; } = [];
        public Queue<long> Output { get; set; } = [];

        ICPUInterrupt Interrupt { get; set; } = null;

        public int CycleCount { get; private set; } = 0;
        double speed = 0;
        double runTimeSecs = 0;

        public IntCPU(string program, int reserve = 0, long[] initialInput = null)
        {
            initialState = Util.ParseNumbers<long>(program);
            Reset(reserve);

            lock (paramLock)
            {
                if (paramCache == null)
                {
                    paramCache = new ParamMode[333][];

                    for (int x = 0; x < 3; ++x)
                        for (int y = 0; y < 3; ++y)
                            for (int z = 0; z < 3; ++z)
                                paramCache[(x * 1) + (y * 10) + (z * 100)] = [(ParamMode)x, (ParamMode)y, (ParamMode)z];
                }
            }

            if (this is ICPUInterrupt) Interrupt = this as ICPUInterrupt;
            if (initialInput != null) AddInput(initialInput);
        }

        public void Reset(int reserve = 0)
        {
            Memory = initialState.ToArray();
            InstructionPointer = 0;
            RelBase = 0;
            CycleCount = 0;
            Input.Clear();
            Output.Clear();
            if (reserve > 0) Reserve(reserve);
        }

        public void Reserve(int memorySize) => Array.Resize(ref Memory, Math.Max(memorySize, Memory.Length));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        long GetValue(int paramIdx) => Memory[GetAddr(paramIdx)];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void PutValue(int paramIdx, long value) => Memory[GetAddr(paramIdx)] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        long GetAddr(int paramIdx)
        {
            long offset = InstructionPointer + paramIdx;
            return paramMode[paramIdx - 1] switch
            {
                ParamMode.Position => Memory[offset],
                ParamMode.Immediate => offset,
                ParamMode.Relative => Memory[offset] + RelBase,
                _ => throw new NotImplementedException(),
            };
        }

        static ParamMode[][] paramCache = null;
        static readonly Lock paramLock = new();

        ParamMode[] paramMode;

        long ReadInput()
        {
            if (Input.Count == 0)
            {
                Interrupt?.RequestInput();
                if (Input.Count == 0)
                {
                    throw new Exception("Out of input!");
                }
            }
            return Input.Dequeue();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Step()
        {
            try
            {
                CycleCount++;

                long raw = Memory[InstructionPointer];
                Opcode Code = (Opcode)(raw % 100);
                paramMode = paramCache[raw / 100];

                switch (Code)
                {
                    case Opcode.ADD: // add PC+1 and PC+2 and put it in address PC+3
                        PutValue(3, GetValue(1) + GetValue(2));
                        break;

                    case Opcode.MUL: // multiply PC+1 and PC+2 and put it in address PC+3
                        PutValue(3, GetValue(1) * GetValue(2));
                        break;

                    case Opcode.GET: // takes a single integer as input and saves it to the address given by its only parameter
                        PutValue(1, ReadInput());
                        break;

                    case Opcode.OUT: // output the value of its only parameter.
                        Output.Enqueue(GetValue(1));
                        Interrupt?.OutputReady();
                        break;

                    case Opcode.JNZ: // if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                        if (GetValue(1) != 0)
                        {
                            InstructionPointer = GetValue(2);
                            return true; // don't move IP, because we've jumped
                        }
                        break;

                    case Opcode.JZ: // if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                        if (GetValue(1) == 0)
                        {
                            InstructionPointer = GetValue(2);
                            return true; // don't move IP, because we've jumped
                        }
                        break;

                    case Opcode.LT: // if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                        PutValue(3, GetValue(1) < GetValue(2) ? 1 : 0);
                        break;

                    case Opcode.EQ: // if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                        PutValue(3, GetValue(1) == GetValue(2) ? 1 : 0);
                        break;

                    case Opcode.SETR:
                        RelBase += GetValue(1);
                        break;

                    case Opcode.HALT: // stop
                        return false;

                    default:
                        throw new Exception("Unknown instruction " + Memory[InstructionPointer]);
                }

                InstructionPointer += InstructionSize(Code);

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                // If the previous instruction failed, reserve more memory and try again!
                // This is faster than checking for out of bounds errors on each access!
                Reserve(Memory.Length + (Memory.Length / 4));
                return true;
            }
        }

        public void Run()
        {
            System.Diagnostics.Stopwatch sw = new();
            sw.Start();

            while (Step()) ;

            sw.Stop();
            runTimeSecs = sw.Elapsed.TotalSeconds;
            speed = CycleCount / runTimeSecs;
        }

        public string Speed() => $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz [{runTimeSecs}S]";

        public void Poke(int addr, long val) => Memory[addr] = val;
        public long Peek(int addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory);
    }
}