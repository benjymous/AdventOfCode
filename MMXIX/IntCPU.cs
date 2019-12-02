
namespace Advent.MMXIX
{
    public class IntCPU
    {
        public int[] Memory {get;set;}
        int PC {get;set;} = 0;

        public IntCPU (string program)
        {
            Memory = Util.Parse(Util.Split(program));
        }

        public bool Step() 
        {
            var instruction = Memory[PC];
            switch(instruction) {
                case 99: // stop
                //Console.WriteLine("Halt");
                return false;

                case 1: // add
                {
                    int addr1 = Memory[PC+1];
                    int addr2 = Memory[PC+2];
                    int addr3 = Memory[PC+3];

                    //Console.WriteLine("Add "+addr1+" "+addr2+" "+addr3);

                    int val1 = Memory[addr1];
                    int val2 = Memory[addr2];

                    int result = val1+val2;

                    //Console.WriteLine("["+addr3+"] =  "+val1+"+"+val2 +" == "+result );

                    Memory[addr3] = result;

                    PC += 4;
                }
                break;

                case 2: // mul
                {
                    int addr1 = Memory[PC+1];
                    int addr2 = Memory[PC+2];
                    int addr3 = Memory[PC+3];

                    //Console.WriteLine("Mul "+addr1+" "+addr2+" "+addr3);

                    int val1 = Memory[addr1];
                    int val2 = Memory[addr2];

                    int result = val1*val2;

                    //Console.WriteLine("["+addr3+"] =  "+val1+"*"+val2 +" == "+result );

                    Memory[addr3] = result;

                    PC += 4;
                }
                break;
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