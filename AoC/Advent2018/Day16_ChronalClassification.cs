using AoC.Advent2018.ChronMatic;

namespace AoC.Advent2018;
public class Day16 : IPuzzle
{
    [Regex(@"Before: \[(.+)\]\n(\d+) (.+)\nAfter:  \[(.+)]")]
    public record class Test(long[] Before, int Instr, [Split(" ")] int[] Args, long[] After)
    {
        public bool DoTest(IInstr instr)
        {
            var data = Before.ToArray();
            instr.Do(Args, ref data);
            return data.SequenceEqual(After);
        }
    }

    public static (Dictionary<Test, HashSet<IInstr>> matches, string program) RunTests(string input)
    {
        var data = input.Split("\n\n\n");
        var testLines = data[0].SplitSections();

        var tests = Util.RegexParse<Test>(testLines);

        var instrs = ChronCPU.GetInstructions();

        Dictionary<Test, HashSet<IInstr>> testMatches = tests.Select(t => (t, instrs.Where(t.DoTest).ToHashSet())).ToDictionary();

        return (testMatches, data[1]);
    }

    public static int Part1(string input)
    {
        var (testMatches, _) = RunTests(input);
        return testMatches.Count(kvp => kvp.Value.Count >= 3);
    }

    public static long Part2(string input)
    {
        var (testMatches, program) = RunTests(input);

        var mapping = new Dictionary<int, IInstr>();
        while (mapping.Count < 16)
        {
            foreach (var match in testMatches.Where(kvp => kvp.Value.Count == 1))
            {
                var matched = match.Value.First();
                mapping[match.Key.Instr] = matched;
                testMatches.Remove(match.Key);
                testMatches.ForEach(other => other.Value.Remove(matched));
            }
        }

        var cpu = new ChronCPU(program, mapping);
        cpu.Run();

        return cpu.Get(0);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}