namespace AoC.Advent2019;
public class Day02 : IPuzzle
{
    public static long RunProgram(string input, int noun, int verb)
    {
        var cpu = new NPSA.IntCPU(input);
        cpu.Poke(1, noun);
        cpu.Poke(2, verb);
        cpu.Run();
        return cpu.Peek(0);
    }

    public static long Part1(string input) => RunProgram(input, 12, 2);

    const int Part2_Target = 19690720;

    public static int Part2(string input)
    {
        var initial = RunProgram(input, 0, 0);
        long aim = initial + (Part2_Target % 100) - (initial % 100) + (((initial % 100) > (Part2_Target % 100)) ? 100 : 0);

        var y = Util.BinarySearch(0, 100, y => (RunProgram(input, 0, y) >= aim, y)).result;
        var x = Util.BinarySearch(0, 100, x => (RunProgram(input, x, y) >= Part2_Target, x)).result;

        return (x * 100) + y;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}