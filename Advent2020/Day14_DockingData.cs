using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day14 : IPuzzle
    {
        public string Name => "2020-14";

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
                    Mask = Mask(bits[1]);
                }
                else
                {
                    type = StatementType.mem;
                    Address = Int64.Parse(bits[0].Replace("mem", "").Replace("[", "").Replace("]", ""));
                    Value = Int64.Parse(bits[1]);
                }
            }

            public StatementType type;
            public Int64 Address = 0;
            public Int64 Value = 0;
            public (Int64 Value, Int64 QuantumBits) Mask;
        }

        public static (Int64 Value, Int64 QuantumBits) Mask(string input)
        {
            Int64 v = 0, q = 0;
            int j = 0;
            for (var i = input.Length - 1; i >= 0; --i, ++j)
            {
                switch (input[i])
                {
                    case '1':
                        Util.SetBit(ref v, j);
                        break;

                    case 'X':
                        Util.SetBit(ref q, j);
                        break;
                }
            }
            return (v, q);
        }

        public static Int64 ApplyMaskV1(Int64 value, (Int64 Value, Int64 QuantumBits) mask)
        {
            Int64 result = value;

            for (var i = 0; i < 36; ++i)
            {
                var b = 1L << i;

                if ((mask.QuantumBits & b) == 0)
                {
                    if ((mask.Value & b) == 0)
                    {
                        Util.ClearBit(ref result, i);
                    }
                    else
                    {
                        Util.SetBit(ref result, i);
                    }
                }
            }

            return result;
        }

        public static (Int64 Value, Int64 QuantumBits) ApplyMaskV2(Int64 value, (Int64 Value, Int64 QuantumBits) mask)
        {
            Int64 resval = value;
            Int64 resqua = 0;

            for (var i = 0; i < 36; ++i)
            {
                var b = 1L << i;

                if ((mask.Value & b) != 0)
                {
                    Util.SetBit(ref resval, i);
                }
                else if ((mask.QuantumBits & b) != 0)
                {
                    Util.SetBit(ref resqua, i);
                }

            }

            return (resval, resqua);
        }

        class MemoryContainer<T>
        {
            readonly Queue<T> items = new();
            readonly HashSet<T> seen = new();
            public void Push(T v)
            {
                if (!seen.Contains(v))
                {
                    items.Enqueue(v);
                    seen.Add(v);
                }
            }

            public T Take() => items.Dequeue();
            public bool Any => items.Count > 0;
        }

        public static IEnumerable<Int64> Combinations((Int64 Value, Int64 QuantumBits) input)
        {
            var inputs = new MemoryContainer<(Int64 Value, Int64 QuantumBits)>();
            inputs.Push(input);

            while (inputs.Any)
            {
                var (Value, QuantumBits) = inputs.Take();

                if (QuantumBits == 0)
                {
                    yield return Value;
                }
                else
                {
                    foreach (var b in QuantumBits.BitSequence())
                    {
                        Int64 newq = QuantumBits & ~b;
                        inputs.Push((Value & ~(b), newq));
                        inputs.Push((Value | b, newq));
                    }
                }
            }
        }

        public static Int64 Part1(string input)
        {
            var statements = Util.Parse<Statement>(input);

            (Int64 Value, Int64 QuantumBits) mask = (0, 0);
            var memory = new Dictionary<Int64, Int64>();
            foreach (var statement in statements)
            {
                if (statement.type == StatementType.mask)
                {
                    mask = statement.Mask;
                }
                else
                {
                    memory[statement.Address] = ApplyMaskV1(statement.Value, mask);
                }
            }

            return memory.Values.Sum();
        }

        public static Int64 Part2(string input)
        {
            var statements = Util.Parse<Statement>(input);

            (Int64 Value, Int64 QuantumBits) mask = (0, 0);
            var memory = new Dictionary<Int64, Int64>();
            foreach (var statement in statements)
            {
                if (statement.type == StatementType.mask)
                {
                    mask = statement.Mask;
                }
                else
                {
                    var addressMask = ApplyMaskV2(statement.Address, mask);
                    var addresses = Combinations(addressMask);
                    foreach (var addr in addresses)
                    {
                        memory[addr] = statement.Value;
                    }
                }
            }

            Console.WriteLine(memory.Count);
            return memory.Values.Sum();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}