using AoC.Utils;
using System;

namespace AoC.Advent2020.Elforola
{
    public enum OpCode
    {
        acc = 0,
        jmp = 1,
        nop = 2,
    }

    public enum RegisterId
    {
        pc = 0,
        acc = 1,
    };

  
    public class Instruction
    {
        public OpCode Opcode;
        public int Val;

        public Instruction(string line)
        {
            var bits = line.Split(" ");
            if (!Enum.TryParse(bits[0], out Opcode)) throw new Exception("Unknown opcode");

            Val = int.Parse(bits[1]);
        }

        public Instruction(Instruction other)
        {
            Opcode = other.Opcode;
            Val = other.Val;
        }

        public override string ToString() => $"{Opcode} {Val}";
    }

    public class Elf80
    {
        public Instruction[] Instructions { get; set; }
        int[] Registers = new int[] { 0, 0 };

        System.Diagnostics.Stopwatch sw;

        public int CycleCount = 0;

        public Elf80(string program)
        {
            Instructions = Util.Parse<Instruction>(program).ToArray();
        }

        public Elf80(Elf80 other)
        {
            Registers = (int[])other.Registers.Clone();
            Instructions = new Instruction[other.Instructions.Length];
            for (int i=0; i<other.Instructions.Length; ++i)
            {
                Instructions[i] = new Instruction(other.Instructions[i]);
            }
        }

        public int Get(RegisterId source) => Registers[(int)source];

        public void Set(RegisterId source, int val) => Registers[(int)source] = val;

        public void Mod(RegisterId source, int val) => Registers[(int)source] += val;

        public bool Step()
        {
            int programCounter = Get(RegisterId.pc);
            if (programCounter >= Instructions.Length) return false;
            CycleCount++;

            var instr = Instructions[programCounter];

            switch (instr.Opcode)
            {
                case OpCode.acc:
                    Mod(RegisterId.acc, instr.Val);
                    Mod(RegisterId.pc, 1);
                    break;

                case OpCode.jmp:
                    Mod(RegisterId.pc, instr.Val);
                    break;

                case OpCode.nop:
                    Mod(RegisterId.pc, 1);
                    break;
         
                default:
                    throw new Exception("Unknown opcode");
            }

            return true;
        }

        public void Run()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (Step()) ;
            sw.Stop();
        }

    }
}