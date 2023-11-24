namespace AoC.Advent2015;
public class Day23 : IPuzzle
{
    public class CPU()
    {
        int programCounter = 0;
        Dictionary<char, int> registers = [];

        [Regex(@"jmp ([\+|\-]\d+)")] public Action InstrJmp(int jump) => () => programCounter += jump - 1;
        [Regex(@"jio (.), \+?(\-?\d+)")] public Action InstrJio(char reg, int jump) => () => programCounter += (registers[reg] == 1) ? jump - 1 : 0;
        [Regex(@"jie (.), ([\+|\-]\d+)")] public Action InstrJie(char reg, int jump) => () => programCounter += ((registers[reg] % 2) == 0) ? jump - 1 : 0;

        [Regex(@"inc (.)")] public Action InstrInc(char reg) => () => registers[reg]++;
        [Regex(@"tpl (.)")] public Action InstrTpl(char reg) => () => registers[reg] *= 3;
        [Regex(@"hlf (.)")] public Action InstrHlf(char reg) => () => registers[reg] /= 2;

        public int Run(string input, Dictionary<char, int> initial)
        {
            registers = initial;
            var program = Util.RegexFactory<Action, CPU>(input, this).ToArray();

            do program[programCounter++]();
            while (programCounter < program.Length);

            return registers['b'];
        }
    }

    public static int Part1(string input)
    {
        var cpu = new CPU();
        return cpu.Run(input, new Dictionary<char, int>() { { 'a', 0 }, { 'b', 0 } });
    }

    public static int Part2(string input)
    {
        var cpu = new CPU();
        return cpu.Run(input, new Dictionary<char, int>() { { 'a', 1 }, { 'b', 0 } });
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}