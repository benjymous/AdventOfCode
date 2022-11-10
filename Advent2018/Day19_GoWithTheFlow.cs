using System;
using System.Collections.Generic;

namespace AoC.Advent2018
{
    public class Day19 : IPuzzle
    {
        public string Name => "2018-19";

        public static long Part1(string input)
        {
            ChronMatic.ChronCPU cpu = new(input);
            //cpu.PeekTime = 10000;
            cpu.Run();
            //Console.WriteLine(cpu.Speed());
            return cpu.Get(0);
        }

        public static int GetFactorSum(int test)
        {
            int result = 0;
            for (int i = 1; i <= test; ++i)
            {
                if (test % i == 0)
                    result += i;
            }
            return result;
        }

        public static int Part2(string input)
        {
#if false // set to true to generate code for lower section
            ///////
            /// Self generating code ahoy!
            ChronMatic.ChronCPU cpu = new ChronMatic.ChronCPU(input);
            var program = cpu.Dump();

            foreach (var line in program)
            {
                Console.WriteLine(line);
            }
            ///////

            return 0;
#else

            HashSet<int> jumps = new();

            int r0 = 1, r1 = 0, r2 = 0, r3 = 0, r4 = 0, r5 = 0;
            Int64 cycle = 0;
            while (r1 < 36)
            {
                cycle++;

                if (cycle % 100000000 == 0)
                {
                    Console.WriteLine($"[{cycle}] {r0} {r1} {r2} {r3} {r4} {r5}");
                }

                //Console.WriteLine($"{cycle} - {r1} - {r0} {r1} {r2} {r3} {r4} {r5}");

                jumps.Add(r1);

                // if (r1 == 7)
                // {
                //     r0 = GetFactorSum(r2);
                //     r1 = 8;
                //     r3 = r2;
                // }

                switch (r1)
                {
                    ///////
                    /// Paste generated block here!
                    ///////

                    // addi 1 16 1
                    case 0: r1 = r1 + 16 + 1; break;

                    // seti 1 5 3
                    case 1: r3 = 1; r1++; goto case 2;
                    // seti 1 7 5
                    case 2: r5 = 1; r1++; goto case 3;
                    // mulr 3 5 4
                    case 3:
                        r4 = r3 * r5; r1++; //goto case 4;
                                            // eqrr 4 2 4
                        /*case 4:*/
                        r4 = (r4 == r2) ? 1 : 0; r1++; //goto case 5;
                                                       // addr 4 1 1
                        /*case 5:*/
                        r1 = r4 + r1 + 1; break;

                    // addi 1 1 1
                    case 6: r1 = r1 + 1 + 1; break;

                    // addr 3 0 0
                    case 7: r0 = r3 + r0; r1++; goto case 8;
                    // addi 5 1 5
                    case 8:
                        r5++; r1++; //goto case 9;
                                           // gtrr 5 2 4
                        /*case 9:*/
                        r4 = (r5 > r2) ? 1 : 0; r1++; //goto case 10;
                                                      // addr 1 4 1
                        /*case 10:*/
                        r1 = r1 + r4 + 1; break;

                    // seti 2 1 1
                    case 11: r1 = 2 + 1; break;

                    // addi 3 1 3
                    case 12:
                        r3++; r1++; //goto case 13;
                                           // gtrr 3 2 4
                        /*case 13:*/
                        r4 = (r3 > r2) ? 1 : 0; r1++; //goto case 14;
                                                      // addr 4 1 1
                        /*case 14:*/
                        r1 = r4 + r1 + 1; break;

                    // seti 1 3 1
                    case 15: r1 = 1 + 1; break;

                    // mulr 1 1 1
                    case 16: r1 = r1 * r1 + 1; break;

                    // addi 2 2 2
                    case 17:
                        r2 += 2; r1++; //goto case 18;
                                           // mulr 2 2 2
                        /*case 18:*/
                        r2 *= r2; r1++; //goto case 19;
                                            // mulr 1 2 2
                        /*case 19:*/
                        r2 = r1 * r2; r1++; //goto case 20;
                                            // muli 2 11 2
                        /*case 20:*/
                        r2 *= 11; r1++; //goto case 21;
                                            // addi 4 7 4
                        /*case 21:*/
                        r4 += 7; r1++; //goto case 22;
                                           // mulr 4 1 4
                        /*case 22:*/
                        r4 *= r1; r1++; //goto case 23;
                                            // addi 4 13 4
                        /*case 23:*/
                        r4 += 13; r1++; //goto case 24;
                                            // addr 2 4 2
                        /*case 24:*/
                        r2 += r4; r1++; //goto case 25;
                                            // addr 1 0 1
                        /*case 25:*/
                        r1 = r1 + r0 + 1; break;

                    // seti 0 9 1
                    case 26: r1 = 0 + 1; break;

                    // setr 1 0 4
                    case 27:
                        r4 = r1; r1++; //goto case 28;
                                       // mulr 4 1 4
                        /*case 28:*/
                        r4 *= r1; r1++; //goto case 29;
                                            // addr 1 4 4
                        /*case 29:*/
                        r4 = r1 + r4; r1++; //goto case 30;
                                            // mulr 1 4 4
                        /*case 30:*/
                        r4 = r1 * r4; r1++; //goto case 31;
                                            // muli 4 14 4
                        /*case 31:*/
                        r4 *= 14; r1++; //goto case 32;
                                            // mulr 4 1 4
                        /*case 32:*/
                        r4 *= r1; r1++; //goto case 33;
                                            // addr 2 4 2
                        /*case 33:*/
                        r2 += r4; r1++; //goto case 34;
                                            // seti 0 2 0

                        return GetFactorSum(r2); // We know the source number, so short circuit the rest;

                    // /*case 34:*/
                    // r0 = 0; r1++; //goto case 35;
                    //               // seti 0 0 1
                    // /*case 35:*/
                    // r1 = 1; break;

                    ///////

                    default:
                        Console.WriteLine($"***** MISSING LABEL {r1} *****");
                        break;
                }
            }
            return r0;
#endif
        }


        // public int _Part2(string input)
        // {
        //     ChronMatic.ChronCPU cpu = new ChronMatic.ChronCPU(input);
        //     cpu.Set(0, 1);
        //     cpu.PeekTime = 100000000;
        //     cpu.Run();
        //     Console.WriteLine(cpu.Speed());

        //     return cpu.Get(0);
        // }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
