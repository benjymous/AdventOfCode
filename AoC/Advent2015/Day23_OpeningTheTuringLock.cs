namespace AoC.Advent2015;
public class Day23 : IPuzzle
{
    public record class CPU(string Input, int[] Registers)
    {
        int programCounter = 0;
        readonly Action<CPU>[] program = Parser.Factory<Action<CPU>, CPU>(Input).ToArray();

        [Regex(@"jmp ([\+|\-]\d+)")] public static Action<CPU> InstrJmp(int jump) => (CPU c) => c.programCounter += jump - 1;
        [Regex(@"jio (.), \+?(\-?\d+)")] public static Action<CPU> InstrJio(char reg, int jump) => (CPU c) => c.programCounter += (c.Registers[reg - 'a'] == 1) ? jump - 1 : 0;
        [Regex(@"jie (.), ([\+|\-]\d+)")] public static Action<CPU> InstrJie(char reg, int jump) => (CPU c) => c.programCounter += ((c.Registers[reg - 'a'] % 2) == 0) ? jump - 1 : 0;

        [Regex(@"inc (.)")] public static Action<CPU> InstrInc(char reg) => (CPU c) => c.Registers[reg - 'a']++;
        [Regex(@"tpl (.)")] public static Action<CPU> InstrTpl(char reg) => (CPU c) => c.Registers[reg - 'a'] *= 3;
        [Regex(@"hlf (.)")] public static Action<CPU> InstrHlf(char reg) => (CPU c) => c.Registers[reg - 'a'] /= 2;

        public int Run()
        {
            do program[programCounter++](this);
            while (programCounter < program.Length);

            return Registers[1];
        }
    }

    public static int Part1(string input) => new CPU(input, [0, 0]).Run();

    public static int Part2(string input) => new CPU(input, [1, 0]).Run();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}