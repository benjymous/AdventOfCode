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

        int InstructionSize(Opcode code)
        {
            switch(code)
            {
                case Opcode.ADD: return 4;
                case Opcode.MUL: return 4;

                case Opcode.GET: return 2;
                case Opcode.OUT: return 2;

                case Opcode.JNZ: return 3;
                case Opcode.JZ: return 3;

                case Opcode.LT: return 4;
                case Opcode.EQ: return 4;

                case Opcode.SETR: return 2;

                case Opcode.HALT: return 1; 
            }
            throw new Exception("Bad instruction");
        }

        enum ParamMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }

        AutoArray<Int64> Memory {get;set;}
        IEnumerable<long> initialState;
        int InstructionPointer {get;set;} = 0;
        int RelBase {get;set;} = 0;
        public Queue<Int64> Input {get;set;} = new Queue<Int64>();
        public Queue<Int64> Output {get;set;} = new Queue<Int64>();

        public ICPUInterrupt Interrupt {get;set;} = null;

        IEnumerable<int> PossibleInstructions()
        {
            yield return 99;
            for (int x=0; x<3; ++x)
            {
                for (int y=0; y<3; ++y)
                {
                    for (int z=0; z<3; ++z)
                    {
                        for (int i=1; i<10; ++i)
                        {
                            yield return i + (x*100) + (y*1000) + (z*10000);
                        }
                    }
                }
            }
        }

        public IntCPU(string program)
        {
            initialState = Util.Parse64(program);
            Reset();     

            lock (instructionCache)
            {
                foreach (var c in PossibleInstructions()) DecodeInstruction(c);
            }    
        }

        public void Reset()
        {
            Memory = new AutoArray<Int64>(initialState);
            InstructionPointer = 0;
            RelBase = 0;
            Input.Clear();
            Output.Clear();
        }

        public void Reserve(int memorySize) => Memory.Reserve(memorySize);

        Int64 GetValue(int paramIdx) => Memory[GetAddr(paramIdx)];
        void PutValue(int paramIdx, Int64 value) => Memory[GetAddr(paramIdx)] = value;
        int GetAddr(int paramIdx)
        {
            switch (instr.mode[paramIdx])
            {
                case ParamMode.Position:
                    return (int)Memory[InstructionPointer+paramIdx+1];

                case ParamMode.Immediate:
                    return (int)(InstructionPointer+paramIdx+1);

                case ParamMode.Relative:
                    return (int)(Memory[InstructionPointer+paramIdx+1]+RelBase);
            } 

            throw new Exception($"Unexpected ParamMode {instr.mode[paramIdx]}");
        }

        struct Instr
        {
            public Opcode code;
            public ParamMode[] mode;
            public int size;

            public override string ToString() => $"{code} - {string.Join(",",mode)}";
        }

        static int[] mods = {100, 1000, 10000, 100000};

        static ParamMode GetOpcode(int raw, int index) => (ParamMode)((raw % mods[index+1])/mods[index]);

        static Dictionary<int, Instr> instructionCache = new Dictionary<int, Instr>();

        Instr instr;

        void DecodeInstruction(int raw)
        {
            if (!instructionCache.TryGetValue(raw, out instr))
            {
                instr = new Instr();

                instr.code = (Opcode)(raw % 100);
                instr.size = InstructionSize(instr.code);

                instr.mode = new ParamMode[instr.size];
                for (var i=0; i<instr.size-1; ++i)
                {
                    instr.mode[i] = GetOpcode(raw, i);
                }
                instructionCache[raw] = instr;
            }
        }

        void DecodeInstruction() => DecodeInstruction((int)Memory[InstructionPointer]);

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
            DecodeInstruction();
            //Console.WriteLine($"{InstructionPointer}: {instr}");
            switch(instr.code) 
            {
                case Opcode.ADD: // add PC+1 and PC+2 and put it in address PC+3
                    PutValue(2, GetValue(0) + GetValue(1));
                break;

                case Opcode.MUL: // multiply PC+1 and PC+2 and put it in address PC+3
                    PutValue(2, GetValue(0) * GetValue(1));
                break;

                case Opcode.GET: // takes a single integer as input and saves it to the address given by its only parameter                
                    PutValue(0, ReadInput());
                break;

                case Opcode.OUT: // output the value of its only parameter.
                    Output.Enqueue(GetValue(0));
                    Interrupt?.HasPutOutput();
                break;

                case Opcode.JNZ: // if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                    if (GetValue(0) != 0) 
                    {
                        InstructionPointer = (int)GetValue(1);
                        return true;
                    }
                break;

                case Opcode.JZ: // if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                    if (GetValue(0) == 0) 
                    {
                        InstructionPointer = (int)GetValue(1);
                        return true;
                    }
                break;

                case Opcode.LT: // if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                    PutValue(2, GetValue(0) < GetValue(1) ? 1 : 0);
                break;

                case Opcode.EQ: // if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                    PutValue(2, GetValue(0) == GetValue(1) ? 1 : 0);
                break;

                case Opcode.SETR:
                    RelBase += (int)GetValue(0);
                break;

                case Opcode.HALT: // stop
                    return false;

                default:
                    throw new Exception("Unknown instruction "+Memory[InstructionPointer]);
            }

            InstructionPointer += instr.size;

            return true;
        }

        public void Run()
        {
            while (Step());
        }

        public void Poke(int addr, Int64 val) => Memory[addr] = val; 
        public Int64 Peek(int addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory);
    }
}