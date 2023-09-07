using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using OperatorFunc = System.Func<int, int, bool>;

namespace AoC.Advent2017
{
    public class Day08 : IPuzzle
    {
        public string Name => "2017-08";

        static OperatorFunc ParseOperator(string op) => op switch
        {
            "==" => (int lhs, int rhs) => lhs == rhs,
            "!=" => (int lhs, int rhs) => lhs != rhs,
            "<" => (int lhs, int rhs) => lhs < rhs,
            ">" => (int lhs, int rhs) => lhs > rhs,
            "<=" => (int lhs, int rhs) => lhs <= rhs,
            ">=" => (int lhs, int rhs) => lhs >= rhs,
            _ => throw new Exception("Unknown operator"),
        };

        class Instruction
        {
            readonly Dictionary<string, int> RegLookup;

            public Instruction(string line, Dictionary<string, int> lookup)
            {
                RegLookup = lookup;
                Instr = Decode(line);
            }

            private int RegIndex(string name) => RegLookup.GetOrCalculate(name, _ => RegLookup.Count);

            private (int RegToChange, int Amount, int RegToCheck, OperatorFunc Operator, int CheckValue) Decode(string line)
            {
                var bits = line.Split(" ");
                int val = int.Parse(bits[2]);
                if (bits[1] == "dec") val = -val;
                return (RegIndex(bits[0]), val, RegIndex(bits[4]), ParseOperator(bits[5]), int.Parse(bits[6]));
            }

            public void Act(int[] regs) => regs[Instr.RegToChange] += Instr.Operator(regs[Instr.RegToCheck], Instr.CheckValue) ? Instr.Amount : 0;

            private (int RegToChange, int Amount, int RegToCheck, OperatorFunc Operator, int CheckValue) Instr;
        }

        public static (int largestEnd, int largestRecord) Run(string input)
        {
            Dictionary<string, int> regLookup = new();
            var instructions = Util.Parse<Instruction, Dictionary<string, int>>(input, regLookup);
            var values = new int[regLookup.Count];

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