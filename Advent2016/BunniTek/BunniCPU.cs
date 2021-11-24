using AoC.Utils;
using System;

namespace AoC.Advent2016.BunniTek
{
    enum OpCode
    {
        cpy = 0,
        inc = 1,
        dec = 2,
        jnz = 3,
        tgl = 4,
        @out = 5
    }

    public enum RegisterId
    {
        a = 0,
        b = 1,
        c = 2,
        d = 3
    };

    public class Value
    {
        public bool isInt { get; private set; }
        public int intVal { get; private set; }

        public Value(int input)
        {
            isInt = true;
            intVal = input;
        }

        public Value(RegisterId input)
        {
            isInt = false;
            intVal = (int)input;
        }

        public Value(string input)
        {
            if (int.TryParse(input, out int v))
            {
                isInt = true;
                intVal = v;
            }
            else if (Enum.TryParse(input, out RegisterId r))
            {
                intVal = (int)r;
                isInt = false;
            }
            else
            {
                isInt = true;
                intVal = 0;
            }
        }

        public Value(Value other)
        {
            intVal = other.intVal;
            isInt = other.isInt;
        }

        public static implicit operator Value(int rhs) => new Value(rhs);
        public static implicit operator Value(RegisterId rhs) => new Value(rhs);

        public override string ToString()
        {
            if (isInt)
            {
                return $"{intVal}";
            }
            else
            {
                return ((RegisterId)intVal).ToString();
            }
        }
    }

    class Instruction
    {
        public OpCode Opcode;
        public Value X;
        public Value Y;

        public Instruction(string line)
        {
            var bits = line.Split(" ");
            if (!Enum.TryParse(bits[0], out Opcode)) throw new Exception("Unknown opcode");

            X = new Value(bits[1]);

            if (bits.Length == 3)
            {
                Y = new Value(bits[2]);
            }
        }

        public Instruction(Instruction other)
        {
            Opcode = other.Opcode;
            X = new Value(other.X);
            if (other.Y != null) Y = new Value(other.Y);
        }

        public override string ToString() => $"{Opcode} {X} {Y}";
    }

    public interface IOutput
    {
        bool Put(int i);
    }

    public class BunnyCPU
    {
        Instruction[] Instructions;
        int[] Registers = new int[] { 0, 0, 0, 0 };

        System.Diagnostics.Stopwatch sw;
        Int64 CycleCount = 0;

        public IOutput Output = null;

        int InstructionPointer = 0;

        public BunnyCPU(string program)
        {
            Instructions = Util.Parse<Instruction>(program).ToArray();
        }

        public void Set(RegisterId id, Value source) => Set((int)id, source);

        public void Set(int destination, Value source)
        {
            Registers[destination] = Get(source);
        }

        public int Get(Value source)
        {
            if (source.isInt)
            {
                return source.intVal;
            }

            return Registers[source.intVal];
        }

        public bool Step()
        {
            CycleCount++;
            if (InstructionPointer >= Instructions.Length) return false;

            //if (CycleCount % 100000000 == 0)
            //{
            //    Console.WriteLine(Speed());
            //    Console.WriteLine(string.Join(", ", Registers));
            //}

            var instr = Instructions[InstructionPointer];

            switch (instr.Opcode)
            {
                case OpCode.cpy:
                    Set(instr.Y.intVal, instr.X);
                    break;

                case OpCode.inc:
                    Registers[instr.X.intVal]++;
                    break;

                case OpCode.dec:
                    Registers[instr.X.intVal]--;
                    break;

                case OpCode.jnz:
                    int val = Get(instr.X);
                    if (val != 0)
                    {
                        InstructionPointer += Get(instr.Y);
                        return true;
                    }
                    break;

                case OpCode.tgl:
                    int instrToChange = InstructionPointer + Get(instr.X);
                    if (instrToChange >= 0 && instrToChange < Instructions.Length)
                    {
                        var otherInstr = Instructions[instrToChange];

                        if (otherInstr.Y == null)
                        {
                            if (otherInstr.Opcode == OpCode.inc)
                            {
                                otherInstr.Opcode = OpCode.dec;
                            }
                            else
                            {
                                otherInstr.Opcode = OpCode.inc;
                            }
                        }
                        else
                        {
                            if (otherInstr.Opcode == OpCode.jnz)
                            {
                                otherInstr.Opcode = OpCode.cpy;
                            }
                            else
                            {
                                otherInstr.Opcode = OpCode.jnz;
                            }
                        }
                    }
                    break;
                case OpCode.@out:
                    if (Output != null)
                    {
                        if (!Output.Put(Get(instr.X)))
                        {
                            return false;
                        }
                    }
                    break;

                default:
                    throw new Exception("Unknown opcode");
            }

            InstructionPointer++;
            return true;
        }

        public void Run()
        {
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            while (Step()) ;
            sw.Stop();
        }

        public string Speed()
        {
            var speed = (double)CycleCount / sw.Elapsed.TotalSeconds;
            return $"{CycleCount} cycles - {speed.ToEngineeringNotation()}hz";
        }
    }
}