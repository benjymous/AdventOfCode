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

        public int[] Memory {get;set;}
        int PC {get;set;} = 0;

        public IntCPU (string program)
        {
            Memory = Util.Parse(Util.Split(program));
        }

        // Read from the address referenced from the given PC offset
        public int ValueRef(int offset)
        {
            return Memory[Memory[PC+offset]];
        }

        // Write to the address referenced from the given PC offset
        public void PutVia(int offset, int val)
        {
            Memory[Memory[PC+offset]]=val;
        }

        public bool Step() 
        {
            switch((Mnemonic)(Memory[PC])) 
            {
                case Mnemonic.HALT: // stop
                {
                    return false;
                }

                case Mnemonic.ADD: // add
                {

                    PutVia(3, ValueRef(1)+ValueRef(2));
                    PC += 4;
                }
                break;

                case Mnemonic.MUL: // mul
                {
                    PutVia(3, ValueRef(1)*ValueRef(2));
                    PC += 4;
                }
                break;

                default:
                {
                    throw new Exception("Unknown instruction "+Memory[PC]);
                }
            }

            return true;
        }

        public void Run()
        {
            bool running = true;
            while (running) running = Step();
        }

        public void Poke(int addr, int val) { Memory[addr] = val; }
        public int Peek(int addr) { return Memory[addr]; }

        public override string ToString()
        {
            return string.Join(",", Memory);
        }
    }
}