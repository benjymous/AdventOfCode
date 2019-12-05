using System;
using System.Collections.Generic;

namespace Advent.MMXIX
{
    public class IntCPU
    {
        public enum Opcode
        {
            ADD = 1,   
            MUL = 2,    

            PUT = 3,
            OUT = 4,

            JT = 5,     // Jump if true
            JF = 6,     // Jump if false

            LT = 7,
            EQ = 8,

            HALT = 99,
        }

        Dictionary<Opcode,int> InstructionSizes = new Dictionary<Opcode, int>()
        {
            {Opcode.ADD, 4},
            {Opcode.MUL, 4},

            {Opcode.PUT, 2},
            {Opcode.OUT, 2},

            {Opcode.JT, 3},
            {Opcode.JF, 3},

            {Opcode.LT, 4},
            {Opcode.EQ, 4},

            {Opcode.HALT, 1}
        };

        public enum ParamMode
        {
            Position = 0,
            Immediate = 1,
            Auto = 99,
        }

        int[] Memory {get;set;}
        int InstructionPointer {get;set;} = 0;
        public Queue<int> Input {get;set;} = new Queue<int>();
        public List<int> Output {get;set;} = new List<int>();

        public IntCPU (string program)
        {
            Memory = Util.Parse(program);
        }

        int GetValue(Instr i, int paramIdx, ParamMode paramMode = ParamMode.Auto)
        {
            if (paramMode == ParamMode.Auto) paramMode = i.mode[paramIdx];
            switch (paramMode)
            {
                case ParamMode.Position:
                {
                    var addr0 = InstructionPointer+paramIdx+1;
                    var addr1 = Memory[addr0];
                    var val = Memory[addr1];
                    return val;
                }
                    
                case ParamMode.Immediate:
                    return Memory[InstructionPointer+paramIdx+1];
            }

            throw new Exception("Unknown opmode");
        }

        class Instr
        {
            public Opcode code = 0;
            public ParamMode[] mode = {0,0,0}; 
        }

        Instr DecodeInstruction()
        {
            Instr instr = new Instr();
            var instructionRaw = Memory[InstructionPointer].ToString().PadLeft(5,'0');

            instr.code = (Opcode)(int.Parse($"{instructionRaw[3]}{instructionRaw[4]}"));

            if (!InstructionSizes.ContainsKey(instr.code))
            {
                throw new Exception($"Unknown instruction {instructionRaw} at {InstructionPointer}");
            }

            for (var i=0; i<InstructionSizes[instr.code]-1; ++i)
            {
                instr.mode[i] = (ParamMode)(int.Parse($"{instructionRaw[2-i]}"));
            }

            return instr;
        }

        bool Step() 
        {
            var instr = DecodeInstruction();
            switch(instr.code) 
            {
                case Opcode.ADD: // add PC+1 and PC+2 and put it in address PC+3
                {
                    var outPos = GetValue(instr, 2, ParamMode.Immediate);
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);
                    Memory[outPos] = val1+val2;
                    InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.MUL: // multiply PC+1 and PC+2 and put it in address PC+3
                {
                    var outPos = GetValue(instr, 2, ParamMode.Immediate);
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);
                    Memory[outPos] = val1*val2;
                    InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.PUT: // takes a single integer as input and saves it to the address given by its only parameter
                {
                    if (Input.Count == 0)
                    {
                        throw new Exception("Out of input!");
                    }
                    var v = Input.Dequeue();
                    Memory[GetValue(instr, 0, ParamMode.Immediate)] = v;
                    InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.OUT: // output the value of its only parameter.
                {
                    Output.Add(GetValue(instr, 0));
                    InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.JT: // if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                {
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);

                    if (val1 != 0) 
                        InstructionPointer = val2;
                    else
                        InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.JF: // if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                {
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);

                    if (val1 == 0) 
                        InstructionPointer = val2;
                    else
                        InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.LT: // if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                {
                    var outPos = GetValue(instr, 2, ParamMode.Immediate);
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);
                    Memory[outPos] = val1 < val2 ? 1 : 0;
                    InstructionPointer += InstructionSizes[instr.code];
                }
                break;

                case Opcode.EQ: // if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                {
                    var outPos = GetValue(instr, 2, ParamMode.Immediate);
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);
                    Memory[outPos] = val1 == val2 ? 1 : 0;
                    InstructionPointer += InstructionSizes[instr.code];
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

        public void Poke(int addr, int val) => Memory[addr] = val; 
        public int Peek(int addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory);
    }
}