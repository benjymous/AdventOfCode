using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day14 : IPuzzle
    {
        public string Name { get { return "2020-14";} }
 
        enum StatementType
        {
            mask,
            mem,
        }

        class Statement
        {
            public Statement(string input)
            {
                var bits = input.Split(" = ");
                if (bits[0] == "mask")
                {
                    type = StatementType.mask;
                    Mask = new Mask(bits[1]);
                }
                else
                {
                    type = StatementType.mem;
                    Address = Int64.Parse(bits[0].Replace("mem","").Replace("[","").Replace("]",""));
                    Value = Int64.Parse(bits[1]);
                }
            }

            public StatementType type;
            public Int64 Address = 0;
            public Int64 Value = 0;
            public Mask Mask;

        }

        public class Mask
        {
            public Mask(string value)
            {
                Value = value;
            }
            public string Value {get; private set;}

            public char Bit(int i)
            {
                return Value[Value.Length-1-i];
            }
        }

        public static Int64 ApplyMaskV1(Int64 value, Mask mask)
        {
            //Console.WriteLine(Convert.ToString(value, 2).PadLeft(36,'0'));
            //Console.WriteLine(mask.Value);
            Int64 result = 0;

            Int64 apply = value;

            for (var i = 0 ; i <mask.Value.Length; ++i)
            {
                Int64 applyBit = 0;
                char b = mask.Bit(i);
                switch(b)
                {
                    case '0': 
                        applyBit = 0;
                        break;
                    case '1':
                        applyBit = 1;
                        break;
                    case 'X':
                        applyBit = apply & 1; 
                        break;
                }
                result |= (applyBit << i);

                //Console.WriteLine();
                //Console.WriteLine("  " + Convert.ToString(result, 2));
                //Console.WriteLine("  " + Convert.ToString(apply, 2));

                apply >>= 1;
            }

            return result;
        }

        public static string ApplyMaskV2(Int64 value, Mask mask)
        {
            //Console.WriteLine();
            //Console.WriteLine("* " + Convert.ToString(value, 2).PadLeft(36,'0'));
            //Console.WriteLine("* " + mask.Value);
            var result = Convert.ToString(value, 2).PadLeft(36,'0').ToArray();

            Int64 apply = value;

            for (var i = 0 ; i <mask.Value.Length; ++i)
            {
                char b = mask.Value[i];
                switch(b)
                {
                    case '0': 
                        break;
                    case '1':
                        result[i] = '1';
                        break;
                    case 'X':
                        result[i] = 'X';
                        break;
                }
            }

            return new string(result);
        }

        public static IEnumerable<Int64> Combinations(string input)
        {
            Queue<string> inputs = new Queue<string>();
            inputs.Enqueue(input);
            HashSet<string> seen = new HashSet<string>();

            while (inputs.Count > 0)
            {
                var next = inputs.Dequeue();
                if (seen.Contains(next)) continue;
                seen.Add(next);

                if (!next.Contains('X')) 
                {
                    //Console.WriteLine(" >> "+next);
                    yield return Convert.ToInt64(next, 2);
                }
                else
                {
                    //Console.WriteLine(" XX "+next);
                    for (var i=0; i<next.Length; ++i)
                    {
                        if (next[i]=='X')
                        {
                            var str0 = next.ToArray();
                            var str1 = next.ToArray();

                            str0[i]='0';
                            str1[i]='1';

                            //Console.WriteLine(" << "+new string(str0));
                            //Console.WriteLine(" << "+new string(str1));

                            inputs.Enqueue(new string(str0));
                            inputs.Enqueue(new string(str1));
                        }
                    }
                }
            }
        }

        public static Int64 Part1(string input)
        {
            var statements = Util.Parse<Statement>(input);

            Mask mask = null;
            var memory = new Dictionary<Int64, Int64>();
            foreach (var statement in statements)
            {
                if (statement.type == StatementType.mask)
                {
                    //Console.WriteLine("MASK:" + statement.Mask.Value );
                    mask = statement.Mask;
                }
                else
                {
                    //Console.WriteLine("MEM "+statement.Address+" = "+statement.Value);
                    memory[statement.Address] = ApplyMaskV1(statement.Value, mask);
                }
            }

            return memory.Values.Sum();
        }

        public static Int64 Part2(string input)
        {
            var statements = Util.Parse<Statement>(input);

            Mask mask = null;
            var memory = new Dictionary<Int64, Int64>();
            foreach (var statement in statements)
            {
                if (statement.type == StatementType.mask)
                {
                    //Console.WriteLine("MASK:" + statement.Mask.Value );
                    mask = statement.Mask;
                }
                else
                {
                    var addressMask = ApplyMaskV2(statement.Address, mask);
                    var addresses = Combinations(addressMask);
                    foreach (var addr in addresses)
                    {
                        //Console.WriteLine("memory["+addr+"] = "+statement.Value);
                        memory[addr] = statement.Value;
                    }
                }
            }

            return memory.Values.Sum();
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(ApplyMask(11, new Mask("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X")));  // 73
            //Console.WriteLine(ApplyMask(101, new Mask("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X")));  // 101
            //Console.WriteLine(ApplyMask(0, new Mask("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X")));  // 64

            //Console.WriteLine("  "+ApplyMaskV2(26, new Mask("00000000000000000000000000000000X0XX")));
   
            //Console.WriteLine(string.Join("\n", Combinations("00000000000000000000000000000001X0XX")));

            // Console.WriteLine(Part2("mask = 000000000000000000000000000000X1001X\n"+
            //     "mem[42] = 100\n"+
            //     "mask = 00000000000000000000000000000000X0XX\n"+
            //     "mem[26] = 1"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}