using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day08 : IPuzzle
    {
        public string Name => "2017-08";

        enum Operator
        {
            Eq,
            Neq,
            Lt,
            Gt,
            Leq,
            Geq
        }

        static Operator Parse(string op)
        {
            switch (op)
            {
                case "==":
                    return Operator.Eq;

                case "!=":
                    return Operator.Neq;

                case "<":
                    return Operator.Lt;

                case ">":
                    return Operator.Gt;

                case "<=":
                    return Operator.Leq;

                case ">=":
                    return Operator.Geq;

                //case ""
                //    return val1  val2;

                default:
                    throw new Exception("Unknown operator");
            }
        }

        class Instruction
        {
            Dictionary<string, int> RegLookup;

            public Instruction(string line, Dictionary<string, int> lookup)
            {
                RegLookup = lookup;
                Instr = Decode(line);
            }

            private int RegIndex(string name)
            {
                if (!RegLookup.ContainsKey(name))
                {
                    RegLookup[name] = RegLookup.Count;
                }
                return RegLookup[name];
            }

            int MaxRegs => RegLookup.Count();

            // b inc 5 if a > 1
            private (int RegToChange, int Amount, int RegToCheck, Operator Operator, int CheckValue) Decode(string line)
            {
                var bits = line.Split(" ");
                int val = int.Parse(bits[2]);
                if (bits[1] == "dec") val = -val;
                return (RegIndex(bits[0]), val, RegIndex(bits[4]), Parse(bits[5]), int.Parse(bits[6]));
            }

            private bool Perform(int val1, Operator op, int val2)
            {
                switch (op)
                {
                    case Operator.Eq:
                        return val1 == val2;

                    case Operator.Neq:
                        return val1 != val2;

                    case Operator.Lt:
                        return val1 < val2;

                    case Operator.Gt:
                        return val1 > val2;

                    case Operator.Leq:
                        return val1 <= val2;

                    case Operator.Geq:
                        return val1 >= val2;

                    default:
                        throw new Exception("Unknown operator");
                }
            }

            public void Act(int[] regs)
            {
                int checkVal = regs[Instr.RegToCheck];
                if (Perform(checkVal, Instr.Operator, Instr.CheckValue))
                {
                    regs[Instr.RegToChange] += Instr.Amount;
                }
            }

            private (int RegToChange, int Amount, int RegToCheck, Operator Operator, int CheckValue) Instr;
        }

        public static (int largestEnd, int largestRecord) Run(string input)
        {
            Dictionary<string, int> regLookup = new Dictionary<string, int>();
            var instructions = Util.Parse<Instruction, Dictionary<string, int>>(input, regLookup);
            var values = new int[1000];

            int runningMax = 0;

            foreach (var instr in instructions)
            {
                instr.Act(values);

                runningMax = Math.Max(runningMax, values.Max());
            }

            return (values.Max(), runningMax);
        }

        public static int Part1(string input)
        {
            return Run(input).largestEnd;
        }

        public static int Part2(string input)
        {
            return Run(input).largestRecord;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}