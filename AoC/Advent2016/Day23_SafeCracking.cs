namespace AoC.Advent2016;
public class Day23 : IPuzzle
{
    public static int Crack(string program, int input)
    {
        var cpu = new BunniTek.BunnyCPU(Optimise(program));
        cpu.Set(BunniTek.RegisterId.a, input);
        cpu.Run();
        Console.WriteLine(cpu.Speed());
        return cpu.Get(BunniTek.RegisterId.a);
    }

    private static string Optimise(string program)
    {
        // replace the nested add loop with a multiply
        // brute force replace - assume this will work for any input!

        var lines = program.Split("\n");

        for (int i = 2; i <= 8; ++i) lines[i] = "nop";
        lines[2] = "jnz 1 7";
        lines[9] = "mul b a";

        return string.Join('\n', lines);
    }

    public static int Part1(string input) => Crack(input, 7);

    public static int Part2(string input) => Crack(input, 12);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}