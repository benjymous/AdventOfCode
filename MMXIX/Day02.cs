using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.MMXIX
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2019-02";} }


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
                    }
                    break;
                }

                PC += 4;

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

        public int RunProgram(string input, int noun, int verb)
        {
            var cpu = new IntCPU(input);
            cpu.Poke(1, noun);
            cpu.Poke(2, verb);
            cpu.Run();
            return cpu.Peek(0);
        }

        public int Part1(string input)
        {
            return RunProgram(input, 12, 2);
        }

        public int Part2(string input)
        {
            for (int x=0; x<100; ++x)
            {
                for (int y=0; y<100; ++y)
                {
                    int v = RunProgram(input, x, y);
                    if (v == 19690720)
                    {
                        return (100*x)+y;
                    }
                }
            }
            return -1;
        }


        public void Run(string input)
        {
            Console.WriteLine("2019 Day02 Pt1 - " + Part1(input));
            Console.WriteLine("2019 Day02 Pt2 - " + Part2(input));
        }
    }
}