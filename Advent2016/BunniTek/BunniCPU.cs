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

    public struct Value
    {
        public bool IsInt { get; private set; }
        public int IntVal { get; private set; }

        public Value(int input)
        {
            IsInt = true;
            IntVal = input;
        }

        public Value(RegisterId input)
        {
            IsInt = false;
            IntVal = (int)input;
        }

        public Value(string input)
        {
            if (int.TryParse(input, out int v))
            {
                IsInt = true;
                IntVal = v;
            }
            else if (Enum.TryParse(input, out RegisterId r))
            {
                IntVal = (int)r;
                IsInt = false;
            }
            else
            {
                IsInt = true;
                IntVal = 0;
            }
        }

        public static implicit operator Value(int rhs) => new(rhs);
        public static implicit operator Value(RegisterId rhs) => new(rhs);

        public override string ToString()
        {
            if (IsInt)
            {
                return $"{IntVal}";
            }
            else
            {
                return ((RegisterId)IntVal).ToString();
            }
        }
    }

    class Instruction
    {
        public OpCode Opcode;
        public Value X;
        public Value? Y = null;

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

        //public Instruction(Instruction other)
        //{
        //    Opcode = other.Opcode;
        //    X = new Value(other.X);
        //    if (other.Y != null) Y = new Value(other.Y.Value);
        //}

        public override string ToString() => $"{Opcode} {X} {Y}";
    }

    public interface IOutput
    {
        bool Put(int i);
    }

    public class BunnyCPU
    {
        readonly Instruction[] Instructions;
        readonly int[] Registers = new int[] { 0, 0, 0, 0 };

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
            if (source.IsInt)
            {
                return source.IntVal;
            }

            return Registers[source.IntVal];
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
                    Set(instr.Y.Value.IntVal, instr.X);
                    break;

                case OpCode.inc:
                    Registers[instr.X.IntVal]++;
                    break;

                case OpCode.dec:
                    Registers[instr.X.IntVal]--;
                    break;

                case OpCode.jnz:
                    int val = Get(instr.X);
                    if (val != 0)
                    {
                        InstructionPointer += Get(instr.Y.Value);
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