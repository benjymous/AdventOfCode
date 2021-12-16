//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace AoC.Advent2018
//{
//    public class Day21 : IPuzzle
//    {
//        public string Name => "2018-21";

//        public static int Part1(string input)
//        {

//#if false
//            ChronMatic.ChronCPU cpu = new ChronMatic.ChronCPU(input);
//            var program = cpu.Dump();

//            foreach (var line in program)
//            {
//                Console.WriteLine(line);
//            }
//#else


//            HashSet<int> jumps = new HashSet<int>();

//            int r0 = 0, r1 = 0, r2 = 0, r3 = 0, r4 = 0, r5 = 0;
//            Int64 cycle = 0;
//            while (true)
//            {
//                cycle++;

//                if (cycle % 100000000 == 0)
//                {
//                    Console.WriteLine($"[{cycle}] {r0} {r1} {r2} {r3} {r4} {r5}");
//                }

//                //Console.WriteLine($"{cycle} - {r1} - {r0} {r1} {r2} {r3} {r4} {r5}");

//                //jumps.Add(r1);

//                // if (r1 == 7)
//                // {
//                //     r0 = GetFactorSum(r2);
//                //     r1 = 8;
//                //     r3 = r2;
//                // }

//                switch (r3)
//                {
//                    ///////
//                    /// Paste generated block here!
//                    ///////
//                    // seti 123 0 1
//                    case 0: r1 = 123 + 1; break;

//                    // bani 1 456 1
//                    case 1: r1 = r1 & 456 + 1; break;

//                    // eqri 1 72 1
//                    case 2: r1 = (r1 == 72) ? 1 : 0 + 1; break;

//                    // addr 1 3 3
//                    case 3: r3 = r1 + r3; r1++; goto case 4;
//                    // seti 0 0 3
//                    case 4: r3 = 0; r1++; goto case 5;
//                    // seti 0 7 1
//                    case 5: r1 = 0 + 1; break;

//                    // bori 1 65536 4
//                    case 6: r4 = r1 | 65536; r1++; goto case 7;
//                    // seti 3798839 3 1
//                    case 7: r1 = 3798839 + 1; break;

//                    // bani 4 255 5
//                    case 8: r5 = r4 & 255; r1++; goto case 9;
//                    // addr 1 5 1
//                    case 9: r1 = r1 + r5 + 1; break;

//                    // bani 1 16777215 1
//                    case 10: r1 = r1 & 16777215 + 1; break;

//                    // muli 1 65899 1
//                    case 11: r1 = r1 * 65899 + 1; break;

//                    // bani 1 16777215 1
//                    case 12: r1 = r1 & 16777215 + 1; break;

//                    // gtir 256 4 5
//                    case 13: r5 = (256 > r4) ? 1 : 0; r1++; goto case 14;
//                    // addr 5 3 3
//                    case 14: r3 = r5 + r3; r1++; goto case 15;
//                    // addi 3 1 3
//                    case 15: r3 = r3 + 1; r1++; goto case 16;
//                    // seti 27 6 3
//                    case 16: r3 = 27; r1++; goto case 17;
//                    // seti 0 2 5
//                    case 17: r5 = 0; r1++; goto case 18;
//                    // addi 5 1 2
//                    case 18: r2 = r5 + 1; r1++; goto case 19;
//                    // muli 2 256 2
//                    case 19: r2 = r2 * 256; r1++; goto case 20;
//                    // gtrr 2 4 2
//                    case 20: r2 = (r2 > r4) ? 1 : 0; r1++; goto case 21;
//                    // addr 2 3 3
//                    case 21: r3 = r2 + r3; r1++; goto case 22;
//                    // addi 3 1 3
//                    case 22: r3 = r3 + 1; r1++; goto case 23;
//                    // seti 25 3 3
//                    case 23: r3 = 25; r1++; goto case 24;
//                    // addi 5 1 5
//                    case 24: r5 = r5 + 1; r1++; goto case 25;
//                    // seti 17 1 3
//                    case 25: r3 = 17; r1++; goto case 26;
//                    // setr 5 6 4
//                    case 26: r4 = r5; r1++; goto case 27;
//                    // seti 7 8 3
//                    case 27: r3 = 7; r1++; goto case 28;
//                    // eqrr 1 0 5
//                    case 28: r5 = (r1 == r0) ? 1 : 0; r1++; goto case 29;
//                    // addr 5 3 3
//                    case 29: r3 = r5 + r3; r1++; goto case 30;
//                    // seti 5 6 3
//                    case 30: r3 = 5; r1++; goto case 31;

//                    case 31:
//                        return 0;
//                }
//            }
//#endif


//            return 0;
//        }

//        public static int Part2(string input)
//        {
//            return 0;
//        }

//        public void Run(string input, ILogger logger)
//        {
//            logger.WriteLine("- Pt1 - " + Part1(input));
//            logger.WriteLine("- Pt2 - " + Part2(input));
//        }
//    }
//}
