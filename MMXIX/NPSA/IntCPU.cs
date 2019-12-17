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
        public enum Opcode
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

        Dictionary<Opcode,int> InstructionSizes = new Dictionary<Opcode, int>()
        {
            {Opcode.ADD, 4},
            {Opcode.MUL, 4},

            {Opcode.GET, 2},
            {Opcode.OUT, 2},

            {Opcode.JNZ, 3},
            {Opcode.JZ, 3},

            {Opcode.LT, 4},
            {Opcode.EQ, 4},

            {Opcode.SETR, 2},

            {Opcode.HALT, 1}
        };

        public enum ParamMode
        {
            Position = 0,
            Immediate = 1,
            Relative = 2,
        }

        AutoList<Int64> Memory {get;set;}
        Int64 InstructionPointer {get;set;} = 0;
        Int64 RelBase {get;set;} = 0;
        public Queue<Int64> Input {get;set;} = new Queue<Int64>();
        public Queue<Int64> Output {get;set;} = new Queue<Int64>();

        public ICPUInterrupt Interrupt {get;set;} = null;

        public IntCPU (string program)
        {
            Memory = new AutoList<Int64>(Util.Parse64(program));
        }

        Int64 GetValue(Instr i, int paramIdx)
        {
            switch (i.mode[paramIdx])
            {
                case ParamMode.Position:
                {
                    return Memory[Memory[InstructionPointer+paramIdx+1]];
                }
                    
                case ParamMode.Immediate:
                {
                    return Memory[InstructionPointer+paramIdx+1];
                }

                case ParamMode.Relative:
                {
                    return Memory[Memory[InstructionPointer+paramIdx+1]+RelBase];
                }
            }

            throw new Exception($"Unexpected ParamMode {i.mode[paramIdx]}");
        }

        Int64 GetOutPos(Instr i, int paramIdx)
        {
            switch (i.mode[paramIdx])
            {
                case ParamMode.Position:
                    return Memory[InstructionPointer+paramIdx+1];

                case ParamMode.Relative:
                    return Memory[InstructionPointer+paramIdx+1]+RelBase;
            } 

            throw new Exception($"Unexpected ParamMode {i.mode[paramIdx]}");
        }

        class Instr
        {
            //public string raw = "00000";
            public Opcode code = 0;
            public ParamMode[] mode = {0,0,0}; 
            public int size = 0;

            public override string ToString()
            {
                //return $"{raw} - {code} - {string.Join(",",mode)}";
                return $"{code} - {string.Join(",",mode)}";
            }
        }

        static int[] mods = {1000, 10000, 100000};

        static int GetOpcode(int raw, int index)
        {
            int mod = mods[index];
            int div = mod/10;
            return (int)(raw % mod)/div;
        }

        Dictionary<Int64, Instr> instructionCache = new Dictionary<long, Instr>();

        Instr DecodeInstruction()
        {
            int raw = (int)Memory[InstructionPointer];
            Instr instr;
            if (instructionCache.TryGetValue(raw, out instr))
            {
                return instr;
            }
            else
            {
                instr = new Instr();

                instr.code = (Opcode)(raw % 100);

                if (!InstructionSizes.TryGetValue(instr.code, out instr.size))
                {
                    throw new Exception($"Unknown instruction {raw} at {InstructionPointer}");
                }

                for (var i=0; i<instr.size-1; ++i)
                {
                    instr.mode[i] = (ParamMode)(GetOpcode(raw, i));
                }
                instructionCache[raw] = instr;
                return instr;
            }
        }

        public bool Step() 
        {     
            var instr = DecodeInstruction();
            //Console.WriteLine($"{InstructionPointer}: {instr}");
            switch(instr.code) 
            {
                case Opcode.ADD: // add PC+1 and PC+2 and put it in address PC+3
                {
                    Memory[GetOutPos(instr, 2)] = GetValue(instr, 0) + GetValue(instr, 1);
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.MUL: // multiply PC+1 and PC+2 and put it in address PC+3
                {
                    Memory[GetOutPos(instr, 2)] = GetValue(instr, 0)*GetValue(instr, 1);
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.GET: // takes a single integer as input and saves it to the address given by its only parameter
                {
                    if (Input.Count == 0)
                    {
                        Interrupt?.WillReadInput();
                        if (Input.Count == 0)
                        {
                            throw new Exception("Out of input!");
                        }
                    }
                    Memory[GetOutPos(instr, 0)] = Input.Dequeue();
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.OUT: // output the value of its only parameter.
                {
                    Output.Enqueue(GetValue(instr, 0));
                    Interrupt?.HasPutOutput();
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.JNZ: // if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                {
                    if (GetValue(instr, 0) != 0) 
                        InstructionPointer = GetValue(instr, 1);
                    else
                        InstructionPointer += instr.size;
                }
                break;

                case Opcode.JZ: // if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                {
                    if (GetValue(instr, 0) == 0) 
                        InstructionPointer = GetValue(instr, 1);
                    else
                        InstructionPointer += instr.size;
                }
                break;

                case Opcode.LT: // if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                {
                    Memory[GetOutPos(instr, 2)] = GetValue(instr, 0) < GetValue(instr, 1) ? 1 : 0;
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.EQ: // if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                {
                    Memory[GetOutPos(instr, 2)] = GetValue(instr, 0) == GetValue(instr, 1) ? 1 : 0;
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.SETR:
                {
                    RelBase += GetValue(instr,0);
                    InstructionPointer += instr.size;
                }
                break;

                case Opcode.HALT: // stop
                {
                    return false;
                }

                default:
                {
                    throw new Exception("Unknown instruction "+Memory[InstructionPointer]);
                }
            }

            return true;
        }

        public void Run()
        {
            while (Step());
        }

        public void Poke(Int64 addr, Int64 val) => Memory[addr] = val; 
        public Int64 Peek(Int64 addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory.Values);
    }
}