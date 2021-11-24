using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AoC.Util;

namespace AoC.Advent2020
{
    public class Day18 : IPuzzle
    {
        public string Name { get { return "2020-18"; } }

        enum Operation
        {
            blank,
            add,
            multiply
        }

        static Int64 Solve(Queue<char> data, QuestionPart part)
        {
            var stack = new Stack<Int64>();
            Int64 result = 0;
            Operation op = Operation.blank;
            while (data.Count > 0)
            {
                var ch = data.Dequeue();
                Int64 val = -1;

                if (ch >= '0' && ch <= '9')
                {
                    val = ch - '0';
                }
                else if (ch == '(')
                {
                    // solve brackets
                    val = Solve(data, part);
                }
                else if (ch == ')')
                {
                    // end of bracketed section
                    break;
                }
                else if (ch == '+')
                {
                    op = Operation.add;
                }
                else if (ch=='*')
                {
                    if (part == QuestionPart.Part1)
                    {
                        op = Operation.multiply;
                    }
                    else
                    {
                        stack.Push(result);
                        result = 0;
                        op = Operation.blank;
                    }
                }

                if (val != -1)
                {
                    switch(op)
                    {
                        case Operation.blank:
                            result = val;
                            break;

                        case Operation.add:
                            result += val;
                            break;

                        case Operation.multiply:
                            result *= val;
                            break;
                    }
                }
            }

            while (stack.Count > 0)
            {
                result *= stack.Pop();
            }

            return result;
        }

        static Queue<char> ToQueue(string input) 
            => new Queue<char>(input.Replace(" ", ""));

        public static Int64 Solve1(string input) 
            => Solve(ToQueue(input), QuestionPart.Part1);

        public static Int64 Solve2(string input)
            => Solve(ToQueue(input), QuestionPart.Part2);
        

        public static Int64 Part1(string input)
            => input.Split("\n").Select(line => Solve1(line)).Sum();

        public static Int64 Part2(string input)
            => input.Split("\n").Select(line => Solve2(line)).Sum();        


        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}