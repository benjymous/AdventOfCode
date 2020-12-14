using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils;

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

        public class QuantumInt
        {
            public QuantumInt(Int64 initialValue)
            {                
                Value = initialValue;
            }

            public QuantumInt(QuantumInt other)
            {
                Value = other.Value;
                QuantumBits = other.QuantumBits;
            }

            public Int64 Value {get;set;} = 0;
            public Int64 QuantumBits {get;set;} = 0;
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

        public static QuantumInt ApplyMaskV2(Int64 value, Mask mask)
        {
            //Console.WriteLine();
            //Console.WriteLine("* " + Convert.ToString(value, 2).PadLeft(36,'0'));
            //Console.WriteLine("* " + mask.Value);

            QuantumInt result = new QuantumInt(value);

            Int64 apply = value;

            for (var i = 0 ; i <mask.Value.Length; ++i)
            {
                char b = mask.Value[i];
                switch(b)
                {
                    case '0': 
                        break;
                    case '1':
                        result.Value = Util.SetBit(result.Value, mask.Value.Length-i-1);
                        break;
                    case 'X':
                        //Console.WriteLine("X at "+(mask.Value.Length-i-1));
                        result.QuantumBits = Util.SetBit(result.QuantumBits, mask.Value.Length-i-1);
                        break;
                }
            }

            //Console.WriteLine("=== "+result.Value + " / "+result.QuantumBits);

            return result;
        }

        public static IEnumerable<Int64> Combinations(QuantumInt input)
        {
            var inputs = new Queue<QuantumInt>();
            inputs.Enqueue(input);
            var seen = new HashSet<Tuple<Int64, Int64>>();

            while (inputs.Count > 0)
            {
                var next = inputs.Dequeue();

                var hash = Tuple.Create(next.Value, next.QuantumBits);
                if (seen.Contains(hash)) continue;
                seen.Add(hash);

                //Console.WriteLine(" ?? "+next.Value+" / "+next.QuantumBits);

                if (next.QuantumBits == 0) 
                {
                    //Console.WriteLine(" >> "+next.Value);
                    yield return next.Value;
                }
                else
                {                    
                    foreach (var b in next.QuantumBits.BitSequence())
                    {
                        var qu0 = new QuantumInt(next);
                        var qu1 = new QuantumInt(next);

                        qu0.QuantumBits &= ~(b);
                        qu1.QuantumBits &= ~(b);

                        qu0.Value &= ~(b);
                        qu1.Value |= (b);

                        //Console.WriteLine(" << "+qu0.Value+" / "+qu0.QuantumBits);
                        //Console.WriteLine(" << "+qu0.Value+" / "+qu0.QuantumBits);

                        inputs.Enqueue(qu0);
                        inputs.Enqueue(qu1);
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
                    //Console.WriteLine("Masking "+statement.Address + " with "+mask.Value);
                    var addressMask = ApplyMaskV2(statement.Address, mask);
                    //Console.WriteLine(" - "+addressMask.Value + " / " + addressMask.QuantumBits);
                    
                    var addresses = Combinations(addressMask);
                    //Console.WriteLine("Combinations: "+addresses.Count());
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

            //var q = ApplyMaskV2(42, new Mask("000000000000000000000000000000X1001X"));
            //var c = Combinations(q);
            //Console.WriteLine(String.Join("\n", c));
            //Console.WriteLine(c.Count());

            // Console.WriteLine(Part2("mask = 000000000000000000000000000000X1001X\n"+
            //     "mem[42] = 100\n"+
            //     "mask = 00000000000000000000000000000000X0XX\n"+
            //     "mem[26] = 1"));

            // var q = new QuantumInt(0);
            // q.SetBit(0);
            // q.SetBit(2);
            // Console.WriteLine( Convert.ToString(q.Value, 2).PadLeft(36,'0'));

            //logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}