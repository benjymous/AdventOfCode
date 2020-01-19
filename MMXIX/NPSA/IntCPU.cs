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
            switch(code)
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

        AutoArray<Int64> Memory {get;set;}
        IEnumerable<Int64> initialState;
        int InstructionPointer {get;set;} = 0;
        int RelBase {get;set;} = 0;
        public Queue<Int64> Input {get;set;} = new Queue<Int64>();
        public Queue<Int64> Output {get;set;} = new Queue<Int64>();

        public ICPUInterrupt Interrupt {get;set;} = null;

        int CycleCount = 0;
        double speed = 0;

        IEnumerable<int> PossibleInstructions()
        {
            for (int x=0; x<3; ++x)
                for (int y=0; y<3; ++y)
                    for (int z=0; z<3; ++z)
                        for (int i=1; i<10; ++i)
                            yield return i + (x*100) + (y*1000) + (z*10000);

            yield return 99;
        }

        public IntCPU(string program)
        {
            initialState = Util.Parse64(program);
            Reset();     

            lock (instructionCache)
            {
                foreach (var c in PossibleInstructions()) 
                    instructionCache[c]=new Instr(c);
            }    
        }

        public void Reset()
        {
            Memory = new AutoArray<Int64>(initialState);
            InstructionPointer = 0;
            RelBase = 0;
            CycleCount = 0;
            Input.Clear();
            Output.Clear();
        }

        public void Reserve(int memorySize) 
        {
            Memory.Reserve(memorySize);
        }

        Int64 GetValue(int paramIdx) => Memory[GetAddr(paramIdx)];
        void PutValue(int paramIdx, Int64 value) => Memory[GetAddr(paramIdx)] = value;
        int GetAddr(int paramIdx)
        {
            switch (instr.Mode[paramIdx-1])
            {
                case ParamMode.Position:
                    return (int)Memory[InstructionPointer+paramIdx];

                case ParamMode.Immediate:
                    return (int)(InstructionPointer+paramIdx);

                case ParamMode.Relative:
                    return (int)(Memory[InstructionPointer+paramIdx]+RelBase);
            } 

            throw new Exception($"Unexpected ParamMode {instr.Mode[paramIdx]}");
        }

        class Instr
        {
            public Opcode Code {get;private set;}
            public byte Size {get;private set;}
            public ParamMode[] Mode {get;private set;}

            public Instr(int raw)
            {
                Code = (Opcode)(raw % 100);
                Size = InstructionSize(Code);

                Mode = new ParamMode[Size-1];
                for (var i=0; i<Size-1; ++i)
                {
                    Mode[i] = GetOpcode(raw, i);
                }
            }

            static int[] mods = {100, 1000, 10000, 100000};

            static ParamMode GetOpcode(int raw, int index) => (ParamMode)((raw % mods[index+1])/mods[index]);

            public override string ToString() => $"{Code} - {string.Join(",",Mode)}";
        }

        static Dictionary<int, Instr> instructionCache = new Dictionary<int, Instr>();

        Instr instr;

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
            CycleCount++;

            instr = instructionCache[(int)Memory[InstructionPointer]];

            //Console.WriteLine($"{InstructionPointer}: {instr}");
            switch(instr.Code) 
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
                        InstructionPointer = (int)GetValue(2);
                        return true;
                    }
                break;

                case Opcode.JZ: // if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                    if (GetValue(1) == 0) 
                    {
                        InstructionPointer = (int)GetValue(2);
                        return true;
                    }
                break;

                case Opcode.LT: // if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                    PutValue(3, GetValue(1) < GetValue(2) ? 1 : 0);
                break;

                case Opcode.EQ: // if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                    PutValue(3, GetValue(1) == GetValue(2) ? 1 : 0);
                break;

                case Opcode.SETR:
                    RelBase += (int)GetValue(1);
                break;

                case Opcode.HALT: // stop
                    return false;

                default:
                    throw new Exception("Unknown instruction "+Memory[InstructionPointer]);
            }

            InstructionPointer += instr.Size;

            return true;
        }

        public void Run()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while(Step());
            var time = sw.Elapsed.TotalSeconds;
            speed = (double)CycleCount / time;
        }

        public string Speed()
        {
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz";
        }

        public void Poke(int addr, Int64 val) => Memory[addr] = val; 
        public Int64 Peek(int addr) => Memory[addr];

        public override string ToString() => string.Join(",", Memory);
    }
}