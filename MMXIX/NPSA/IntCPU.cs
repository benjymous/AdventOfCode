using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXIX.NPSA
{
    public interface ICPUInterrupt
    {
        void WillReadInput();
        void HasPutOutput();
    }

    public class IntCPU
    {
        enum Opcode : byte
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

        static byte InstructionSize(Opcode code)
        {
            switch (code)
            {
                case Opcode.HALT:
                    return 1;

                case Opcode.GET:
                case Opcode.OUT:
                case Opcode.SETR:
                    return 2;

                case Opcode.JNZ:
                case Opcode.JZ:
                    return 3;

                case Opcode.ADD:
                case Opcode.MUL:
                case Opcode.LT:
                case Opcode.EQ:
                    return 4;
            }
            throw new Exception("Bad instruction");
        }

        enum ParamMode : byte
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }

        Int64[] Memory;
        IEnumerable<Int64> initialState;
        Int64 InstructionPointer = 0;
        Int64 RelBase = 0;
        public Queue<Int64> Input { get; set; } = new Queue<Int64>();
        public Queue<Int64> Output { get; set; } = new Queue<Int64>();

        public ICPUInterrupt Interrupt { get; set; } = null;

        int CycleCount = 0;
        double speed = 0;
        double runTimeSecs = 0;

        IEnumerable<int> PossibleParamModes()
        {
            for (int x = 0; x < 3; ++x)
                for (int y = 0; y < 3; ++y)
                    for (int z = 0; z < 3; ++z)
                        yield return (x * 100) + (y * 1000) + (z * 10000);
        }

        public IntCPU(string program)
        {
            initialState = Util.Parse64(program);
            Reset();

            lock (paramCache)
            {
                foreach (var c in PossibleParamModes())
                    paramCache[c / 100] = DecodeParams(c);
            }
        }

        public void Reset()
        {
            Memory = initialState.ToArray();
            InstructionPointer = 0;
            RelBase = 0;
            CycleCount = 0;
            Input.Clear();
            Output.Clear();
        }

        public void Reserve(int memorySize) => Array.Resize(ref Memory, Math.Max(memorySize, Memory.Length));

        Int64 GetValue(int paramIdx) => Memory[GetAddr(paramIdx)];
        void PutValue(int paramIdx, Int64 value) => Memory[GetAddr(paramIdx)] = value;
        Int64 GetAddr(int paramIdx)
        {
            Int64 offset = InstructionPointer + paramIdx;
            switch (paramMode[paramIdx - 1])
            {
                case ParamMode.Position:
                    return Memory[offset];

                case ParamMode.Immediate:
                    return offset;

                case ParamMode.Relative:
                    return (Memory[offset] + RelBase);
            }

            throw new Exception($"Unexpected ParamMode {paramMode[paramIdx - 1]}");
        }

        static readonly int[] mods = { 100, 1000, 10000, 100000 };
        static readonly int MAX_PARAMS = 3;

        ParamMode[] DecodeParams(int raw)
        {
            ParamMode[] mode = new ParamMode[MAX_PARAMS];
            for (var i = 0; i < MAX_PARAMS; ++i)
            {
                mode[i] = (ParamMode)((raw % mods[i + 1]) / mods[i]);
            }
            return mode;
        }

        static ParamMode[][] paramCache = new ParamMode[333][];

        ParamMode[] paramMode;

        Int64 ReadInput()
        {
            if (Input.Count == 0)
            {
                Interrupt?.WillReadInput();
                if (Input.Count == 0)
                {
                    throw new Exception("Out of input!");
                }
            }
            return Input.Dequeue();
        }

        public bool Step()
        {
            try
            {
                CycleCount++;

                Int64 raw = Memory[InstructionPointer];
                Opcode Code = (Opcode)(raw % 100);
                paramMode = paramCache[raw / 100];

                //Console.WriteLine($"{InstructionPointer}: {instr}");
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
                        Interrupt?.HasPutOutput();
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
            catch (System.IndexOutOfRangeException)
            {
                // If the previous instruction failed, reserve more memory and try again!
                // This is faster than checking for out of bounds errors on each access!
                Reserve(Memory.Length + (Memory.Length / 4));
                return true;
            }
        }

        public void Run()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            while (Step());

            sw.Stop();
            runTimeSecs = sw.Elapsed.TotalSeconds;
            speed = (double)CycleCount / runTimeSecs;
        }

        public string Speed()
        {
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz [{runTimeSecs}S]";
        }

        public void Poke(int addr, Int64 val) => Memory[addr] = val;
        public Int64 Peek(int addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory);
    }
}