using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day18 : IPuzzle
    {
        public string Name { get { return "2020-18"; } }

        static Int64 Solve1(Queue<char> data)
        {
            Int64 sum = 0;
            char op = ' ';
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
                    val = Solve1(data);
                }
                else if (ch == ')')
                {
                    break;
                }
                else if (ch == '+' || ch == '*')
                {
                    op = ch;
                }

                if (val != -1)
                {
                    if (op == ' ')
                    {
                        sum = val;
                    }
                    else if (op == '+')
                    {
                        sum += val;
                    }
                    else
                    {
                        sum *= val;
                    }
                }
            }

            return sum;
        }

        public static Int64 Solve1(string sum)
        {
            sum = sum.Replace(" ", "");
            return Solve1(new Queue<char>(sum));
        }

        static Int64 Solve2(Queue<char> data)
        {
            Stack<Int64> stack = new Stack<Int64>();

            Int64 sum = 0;
            char op = ' ';
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
                    val = Solve2(data);
                }
                else if (ch == ')')
                {
                    break;
                }
                else if (ch == '+')
                {
                    op = ch;
                }
                else if (ch =='*')
                {
                    stack.Push(sum);
                    sum = 0;
                    op = ' ';
                }

                if (val != -1)
                {
                    if (op == ' ')
                    {
                        sum = val;
                    }
                    else if (op == '+')
                    {
                        sum += val;
                    }
                }
            }

            while (stack.Count>0)
            {
                sum *= stack.Pop();
            }

            return sum;
        }

        public static Int64 Solve2(string sum)
        {
            sum = sum.Replace(" ", "");
            return Solve2(new Queue<char>(sum));
        }

        public static Int64 Part1(string input)
        {
            var lines = input.Split("\n");
            return lines.Select(line => Solve1(line)).Sum();
        }

        public static Int64 Part2(string input)
        {
            var lines = input.Split("\n");
            return lines.Select(line => Solve2(line)).Sum();
        }


        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}