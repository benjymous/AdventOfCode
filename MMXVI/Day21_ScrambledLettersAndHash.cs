using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils;

namespace Advent.MMXVI
{
    public class Day21 : IPuzzle
    {
        public string Name { get { return "2016-21";} }
 
        public static char Switch(char c, char from, char to)
        {
            if (c==from)
            {
                return to;
            }
            else if (c==to)
            {
                return from;
            }
            return c;
        }



        private static string Scramble(IEnumerable<string> instructions, IEnumerable<char> input)
        {
            var length = input.Count();

            IEnumerable<char>password = input;

            foreach (var instr in instructions)
            {
                password = password.ToArray();
                //Console.WriteLine($"[{password.AsString()}] - {instr}");
                var bits = instr.Split(" ");
                if (instr.StartsWith("rotate"))
                {

                    if (bits[1] == "right")
                    {
                        var dist = int.Parse(bits[2]);
                        password = RotateRight(password, length, dist);
                    }
                    else if (bits[1] == "left")
                    {
                        var dist = int.Parse(bits[2]);
                        password = RotateLeft(password, dist);
                    }
                    else if (bits[1] == "based")
                    {
                        var letter = bits[6][0];
                        password = RotateRightBasedOnLetter(password, length, letter);
                    }
                    else
                    {
                        Console.WriteLine("Unknown rotate " + bits[1]);
                    }

                }
                else if (instr.StartsWith("swap"))
                {
                    if (bits[1] == "letter")
                    {
                        var from = bits[2][0];
                        var to = bits[5][0];
                        password = SwapLetter(password, from, to);
                    }
                    else if (bits[1] == "position")
                    {
                        var from = int.Parse(bits[2]);
                        var to = int.Parse(bits[5]);
                        password = SwapPosition(password, from, to);
                    }
                    else
                    {
                        Console.WriteLine("Unknown swap " + bits[1]);
                    }
                }
                else if (instr.StartsWith("reverse"))
                {
                    var start = int.Parse(bits[2]);
                    var end = int.Parse(bits[4]);

                    password = Reverse(password, start, end);
                }
                else if (instr.StartsWith("move"))
                {
                    var from = int.Parse(bits[2]);
                    var to = int.Parse(bits[5]);

                    password = Move(password, from, to);
                }
                else
                {

                    Console.WriteLine("Unknown instruction " + instr);
                }

            }
            
            //Console.WriteLine(password.AsString());
            //Console.WriteLine("----");
            return password.AsString();
        }


        // private static string Unscramble(IEnumerable<string> instructions, IEnumerable<char> input)
        // {
        //     instructions = instructions.Reverse();
        //     var length = input.Count();

        //     IEnumerable<char>password = input;

        //     foreach (var instr in instructions)
        //     {
        //         password = password.ToArray();
        //         Console.WriteLine($"[{password.AsString()}] - {instr}");
        //         var bits = instr.Split(" ");
        //         if (instr.StartsWith("rotate"))
        //         {

        //             if (bits[1] == "right")
        //             {
        //                 var dist = int.Parse(bits[2]);
        //                 password = RotateLeft(password, dist);
        //             }
        //             else if (bits[1] == "left")
        //             {
        //                 var dist = int.Parse(bits[2]);
        //                 password = RotateRight(password, length, dist);
        //             }
        //             else if (bits[1] == "based")
        //             {
        //                 var letter = bits[6][0];
        //                 password = RotateLeftBasedOnLetter(password, length, letter);
        //             }
        //             else
        //             {
        //                 Console.WriteLine("Unknown rotate " + bits[1]);
        //             }

        //         }
        //         else if (instr.StartsWith("swap"))
        //         {
        //             if (bits[1] == "letter")
        //             {
        //                 var from = bits[5][0];
        //                 var to = bits[2][0];
        //                 password = SwapLetter(password, from, to);
        //             }
        //             else if (bits[1] == "position")
        //             {
        //                 var from = int.Parse(bits[5]);
        //                 var to = int.Parse(bits[2]);
        //                 password = SwapPosition(password, from, to);
        //             }
        //             else
        //             {
        //                 Console.WriteLine("Unknown swap " + bits[1]);
        //             }
        //         }
        //         else if (instr.StartsWith("reverse"))
        //         {
        //             var start = int.Parse(bits[2]);
        //             var end = int.Parse(bits[4]);

        //             password = Reverse(password, start, end);
        //         }
        //         else if (instr.StartsWith("move"))
        //         {
        //             var from = int.Parse(bits[5]);
        //             var to = int.Parse(bits[2]);

        //             password = Move(password, from, to);
        //         }
        //         else
        //         {

        //             Console.WriteLine("Unknown instruction " + instr);
        //         }

        //     }
        //     Console.WriteLine(password.AsString());
        //     Console.WriteLine("----");
        //     return password.AsString();
        // }


        private static IEnumerable<char> SwapLetter(IEnumerable<char> password, char from, char to)
        {
            return password.Select(c => Switch(c, from, to));
        }

        private static IEnumerable<char> SwapPosition(IEnumerable<char> password, int from, int to)
        {
            var charFrom = password.ElementAt(from);
            var charTo = password.ElementAt(to);
            return SwapLetter(password, charFrom, charTo);
        }

        private static IEnumerable<char> RotateLeft(IEnumerable<char> password, int dist)
        {
            var first = password.Take(dist);
            var rest = password.Skip(dist);
            return rest.Concat(first);
        }

        private static IEnumerable<char> RotateRight(IEnumerable<char> password, int length, int dist)
        {
            var last = password.Skip(length - dist).Take(dist);
            var rest = password.Take(length - dist);
            return last.Concat(rest);
        }

        static Dictionary<int,int> RotateLetterForward = new Dictionary<int, int>()
        {
            {0,1},
            {1,2},
            {2,3},
            {3,4},
            {4,6},
            {5,7},
            {6,8},
            {7,9},
        };

        static Dictionary<int,int> RotateLetterBackward = new Dictionary<int, int>()
        {
            {0,1},
            {1,1}, 
            {2,6}, 
            {3,2}, 
            {4,7}, 
            {5,3},      
            {6,0}, 
            {7,4},
        };

        private static IEnumerable<char> RotateRightBasedOnLetter(IEnumerable<char> password, int length, char letter)
        {
            var pos = password.AsString().IndexOf(letter);
            //var dist1 = 1 + pos + ((pos >= 4) ? 1 : 0);
            var dist = RotateLetterForward[pos];
            //Util.Test(dist, dist1);
            return RotateRight(password, length, dist);
        }

        // 

        private static IEnumerable<char> RotateLeftBasedOnLetter(IEnumerable<char> password, int length, char letter)
        {
            var pos = password.AsString().IndexOf(letter);
            var dist = (-pos / 2 + (pos % 2 == 1 || pos == 0 ? 1 : 5))+8;
            //var dist1 = RotateLetterBackward[pos];
            //Util.Test(dist, dist1);
            return RotateRight(password, length, dist);
        }

        private static IEnumerable<char> Reverse(IEnumerable<char> password, int start, int end)
        {
            return password.Take(start).Concat(password.Skip(start).Take(end - start+1).Reverse()).Concat(password.Skip(end + 1));
        }

        private static IEnumerable<char> Move(IEnumerable<char> password, int from, int to)
        {
            var fromChar = password.ElementAt(from);
            var filtered = password.Where(c => c != fromChar);
            return filtered.Take(to).Concat($"{fromChar}").Concat(filtered.Skip(to));
        }

        public static string Part1(string input)
        {
            var instructions = Util.Split(input);
            return Scramble(instructions, "abcdefgh");
        }

        public static string Part2(string input)
        {
            var instructions = Util.Split(input);

            var scrambled = "fbgdceah";

            var perms = "abcdefgh".Permutations();

            return perms.AsParallel().Where(p => Scramble(instructions, p)==scrambled).First().AsString();
        }

        // public static string _Part2(string input)
        // {
        //     var instructions = Util.Split(input);
        //     return Unscramble(instructions, "fbgdceah");
        // }

        public void Run(string input, ILogger logger)
        {
            // Util.Test(SwapPosition("abcde", 1, 3).AsString(), "adcbe");
            // Util.Test(SwapLetter("abcde", 'a', 'c').AsString(), "cbade");

            // Util.Test(RotateLeft("abcde",2).AsString(), "cdeab");
            // Util.Test(RotateRight("abcde",5, 2).AsString(), "deabc");

            //Util.Test(RotateRightBasedOnLetter("abcdefgh", 8, 'c').AsString(), "fghabcde");

            // Util.Test(Reverse("abcdefgh", 2, 5).AsString(), "abfedcgh");

            // Util.Test(Move("abcdef", 2, 4).AsString(), "abdecf");

            // var instructions = Util.Split(input);

            // var start = "abcdefgh";
            // var pwd = Scramble(instructions, start);
            // var back = Unscramble(instructions, pwd);
            // Util.Test(back, start);

            // var start = "e-------";
            // var rot = RotateRightBasedOnLetter(start, start.Length, 'e');
            // var rev = RotateLeftBasedOnLetter(rot, start.Length, 'e');

            // Util.Test(rev.AsString(), start);

            // Util.Test(Part1(input), "dbfgaehc");

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}