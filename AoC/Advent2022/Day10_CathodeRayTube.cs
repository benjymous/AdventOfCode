namespace AoC.Advent2022;
public class Day10 : IPuzzle
{
    class Factory
    {
        [Regex("noop")] public static int Noop() => 0;
        [Regex("addx (.+)")] public static int Addx(int v) => v;
    }

    private static IEnumerable<(int cycle, int val)> SimulateCPU(Util.AutoParse<int, Factory> input)
    {
        int cycle = 1, x = 1;
        foreach (var instr in input)
        {
            for (int i = 0; i < (instr == 0 ? 1 : 2); ++i) yield return (cycle++, x);
            x += instr;
        }
    }

    public static IEnumerable<char> SimulateCRT(IEnumerable<(int cycle, int x)> watch)
        => watch.Select(v => (beamPos: v.cycle % 40, v.x))
                .Select(v => v.beamPos == 0 ? '\n' : (v.beamPos >= v.x && v.beamPos <= v.x + 2) ? '▊' : ' ');

    public static int Part1(string input)
    {
        HashSet<int> watchCycles = [20, 60, 100, 140, 180, 220];
        return SimulateCPU(input).Where(slice => watchCycles.Contains(slice.cycle))
                                 .Sum(slice => slice.val * slice.cycle);
    }

    public static string Part2(string input, ILogger logger)
    {
        var crt = SimulateCRT(SimulateCPU(input)).AsString();

        logger.WriteLine(Environment.NewLine + crt);

        return crt.GetMD5String();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, logger));
    }
}