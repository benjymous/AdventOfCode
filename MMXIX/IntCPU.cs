using System;
using System.Collections.Generic;

namespace Advent.MMXIX
{
    public class IntCPU
    {
        public enum Mnemonic
        {
            ADD = 1,
            MUL = 2,
            PUT = 3,
            OUT = 4,
            HALT = 99,
        }

        Dictionary<Mnemonic,int> InstructionSizes = new Dictionary<Mnemonic, int>()
        {
            {Mnemonic.ADD, 4},
            {Mnemonic.MUL, 4},
            {Mnemonic.PUT, 2},
            {Mnemonic.OUT, 2},

            {Mnemonic.HALT, 1}
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
            if (paramMode == ParamMode.Auto) paramMode = i.opmode[paramIdx];
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
            public Mnemonic instruction = 0;
            public ParamMode[] opmode = {0,0,0}; 
        }

        Instr DecodeInstruction()
        {
            Instr instr = new Instr();
            var instructionRaw = Memory[InstructionPointer].ToString().PadLeft(5,'0');

            instr.instruction = (Mnemonic)(int.Parse($"{instructionRaw[3]}{instructionRaw[4]}"));

            for (var i=0; i<InstructionSizes[instr.instruction]-1; ++i)
            {
                instr.opmode[i] = (ParamMode)(int.Parse($"{instructionRaw[2-i]}"));
            }

            return instr;
        }

        bool Step() 
        {
            var instr = DecodeInstruction();
            switch(instr.instruction) 
            {
                case Mnemonic.ADD: // add PC+1 and PC+2 and put it in address PC+3
                {
                    var outPos = GetValue(instr, 2, ParamMode.Immediate);
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);
                    Memory[outPos] = val1+val2;
                }
                break;

                case Mnemonic.MUL: // multiply PC+1 and PC+2 and put it in address PC+3
                {
                    var outPos = GetValue(instr, 2, ParamMode.Immediate);
                    var val1 = GetValue(instr, 0);
                    var val2 = GetValue(instr, 1);
                    Memory[outPos] = val1*val2;
                }
                break;

                case Mnemonic.PUT: // takes a single integer as input and saves it to the address given by its only parameter
                {
                    if (Input.Count == 0)
                    {
                        throw new Exception("Out of input!");
                    }
                    var v = Input.Dequeue();
                    Memory[GetValue(instr, 0, ParamMode.Immediate)] = v;
                }
                break;

                case Mnemonic.OUT: // output the value of its only parameter.
                {
                    Output.Add(GetValue(instr, 0));
                }
                break;

                case Mnemonic.HALT: // stop
                {
                    return false;
                }

                default:
                {
                    throw new Exception("Unknown instruction "+Memory[InstructionPointer]);
                }
            }

            InstructionPointer += InstructionSizes[instr.instruction];

            return true;
        }

        public void Run()
        {
            bool running = true;
            while (running = Step());
        }

        public void Poke(int addr, int val) => Memory[addr] = val; 
        public int Peek(int addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory);
    }
}