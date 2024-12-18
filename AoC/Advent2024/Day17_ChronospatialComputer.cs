namespace AoC.Advent2024;
public class Day17 : IPuzzle
{
    public class Cpu
    {
        public long[] Registers = new long[3];
        int[] Program;

        enum OpCode
        {
            adv = 0,
            bxl = 1,
            bst = 2,
            jsz = 3,
            bxc = 4,
            @out = 5,
            bdv = 6,
            cdv = 7,
        }

        const int RegA = 0, RegB = 1, RegC = 2;

        public static Cpu Create(string input) => Parser.Factory<Cpu>(input);

        [Regex(@"Register (.): (-?\d+)")]
        public void ParseReg(char reg, long val) => Registers[reg - 'A'] = val;

        [Regex(@"Program: (.+)")]
        public void ParseProg([Split(",")] int[] vals) => Program = vals;

        public IEnumerable<int> Run()
        {
            int PC = 0;
            while (PC < Program.Length)
            {
                var operand = Program[PC + 1];
                switch ((OpCode)Program[PC])
                {
                    case OpCode.adv:
                        Registers[RegA] = Registers[RegA] / Util.TwoPowi((int)Combo(operand));
                        break;

                    case OpCode.bxl:
                        Registers[RegB] = Registers[RegB] ^ operand;
                        break;

                    case OpCode.bst:
                        Registers[RegB] = Combo(operand) % 8;
                        break;

                    case OpCode.jsz:
                        if (Registers[RegA] != 0) PC = operand - 2;
                        break;

                    case OpCode.bxc:
                        Registers[RegB] = Registers[RegB] ^ Registers[RegC];
                        break;

                    case OpCode.@out:
                        yield return (int)(Combo(operand) % 8);
                        break;

                    case OpCode.bdv:
                        Registers[RegB] = Registers[RegA] / Util.TwoPowi((int)Combo(operand));
                        break;

                    case OpCode.cdv:
                        Registers[RegC] = Registers[RegA] / Util.TwoPowi((int)Combo(operand));
                        break;
                }

                PC += 2;
            }
        }

        public void List()
        {
            for (int PC = 0; PC < Program.Length; PC += 2)
            {
                Console.Write($"PC{PC}: ");

                var operand = Program[PC + 1];
                switch ((OpCode)Program[PC])
                {
                    case OpCode.adv:
                        Console.Write($"A /= {TwoPow2(Combo2(operand))};");
                        break;

                    case OpCode.bxl:
                        Console.Write($"B = B ^ {operand};");
                        break;

                    case OpCode.bst:
                        Console.Write($"B = {Combo2(operand)} % 8;");
                        break;

                    case OpCode.jsz:
                        Console.Write($"if (A!=0) goto PC{operand};");
                        break;

                    case OpCode.bxc:
                        Console.Write("B ^= C;");
                        break;

                    case OpCode.@out:
                        Console.Write($"Out.Add({Combo2(operand)} % 8);");
                        break;

                    case OpCode.bdv:
                        Console.Write($"B = A / {TwoPow2(Combo2(operand))};");
                        break;

                    case OpCode.cdv:
                        Console.Write($"C = A / {TwoPow2(Combo2(operand))};");
                        break;
                }

                Console.WriteLine($" // {(OpCode)Program[PC]} {operand}");
            }

            static string Combo2(int i) => i switch
            {
                4 => "A",
                5 => "B",
                6 => "C",
                _ => i.ToString()
            };

            static string TwoPow2(string i)
                => i is "A" or "B" or "C" ? $"TwoPow((int){i})" : Util.TwoPowi(int.Parse(i)).ToString();
        }

        long Combo(int operand)
            => operand is > 3 and < 7 ? Registers[operand - 4] : operand;
    }

    private static long SolvePart2(string input)
    {
        // Get our desired output
        var test = Util.Split(Util.Split(input, "\n").Last()[9..], ",").Select(int.Parse).ToArray();

        // Find where the output stops being the right length
        long regA = Util.BinarySearch(0, long.MaxValue, (i) => TestCPU(i).Count() > test.Length);

        long lastResult = 0;

        while (regA > 0)
        {
            var res = TestCPU(regA).ToArray();

            var match = Match(res, test);
            if (match == res.Length)
                lastResult = regA;

            // step based on how close we are and how big the set is
            regA -= Math.Max(1, (long)Math.Pow(10, 13 - match));
        }

        return lastResult;
    }

    static int Match(int[] v, int[] v2)
    {
        if (v.Length != v2.Length) return 0;

        int count = 0;
        for (int i = v.Length - 1; i >= 0; i--)
        {
            if (v[i] == v2[i]) count++;
            else break;
        }
        return count;
    }

    private static IEnumerable<int> TestCPU(long initA)
    {
        long a = initA, b = 0, c = 0;

        ///// Modified from Pasted code from cpu.List()!
        do
        {
            b = a % 8; // bst 4
            b ^= 7; // bxl 7
            c = a / Util.TwoPowi((int)b); // cdv 5
            a /= 8; // adv 3
            b ^= 7; // bxl 7
            b ^= c; // bxc 1
            yield return (int)(b % 8); // out 5
        } while (a != 0); // jsz 0
        ///// end of paste!
    }

    public static string Part1(string input) => string.Join(",", Cpu.Create(input).Run());
    public static long Part2(string input) => SolvePart2(input);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}