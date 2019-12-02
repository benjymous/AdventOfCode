using System;

namespace Advent.MMXIX
{
    public class IntCPU
    {
        public enum Mnemonic
        {
            ADD = 1,
            MUL = 2,
            HALT = 99,
        }

        int[] Memory {get;set;}
        int InstructionPointer {get;set;} = 0;

        public IntCPU (string program)
        {
            Memory = Util.Parse(program);
        }

        // Read from the address referenced from the given PC offset
        int ValueRef(int offset) => Memory[Memory[InstructionPointer+offset]];

        // Write to the address referenced from the given PC offset
        void PutVia(int offset, int val) => Memory[Memory[InstructionPointer+offset]]=val;

        bool Step() 
        {
            switch((Mnemonic)(Memory[InstructionPointer])) 
            {
                case Mnemonic.ADD: // add PC+1 and PC+2 and put it in address PC+3
                {
                    PutVia(3, ValueRef(1)+ValueRef(2)); 
                    InstructionPointer += 4;
                }
                break;

                case Mnemonic.MUL: // multiply PC+1 and PC+2 and put it in address PC+3
                {
                    PutVia(3, ValueRef(1)*ValueRef(2));
                    InstructionPointer += 4;
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