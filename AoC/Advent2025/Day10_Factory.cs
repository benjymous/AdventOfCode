using Microsoft.Z3;

namespace AoC.Advent2025;

public class Day10 : IPuzzle
{
    public class Machine
    {
        bool[] TargetLights = [];
        int[] TargetJolts = [];
        readonly List<HashSet<int>> ButtonActions = [];

        public Machine(string input) => Parser.Factory(input, this, " ");

        public int SolvePart1()
        {
            int target = TargetLights.Reverse().Aggregate(0, (v, b) => (v << 1) + (b ? 1 : 0));
            var buttons = ButtonActions.Select(a => a.Sum(v => 1 << v)).ToArray();
            
            return Solver<(int lights, int presses)>.Solve((0, 0), (state, solver) =>
            {
                if (state.lights == target) return state.presses;

                if ((solver.CurrentBest == null || state.presses < solver.CurrentBest) && solver.IsBetterThanSeen(state.lights, state.presses))
                {
                    solver.EnqueueRange(buttons.Select(c => ((state.lights ^ c, state.presses + 1), state.presses + 1)));
                }

                return default;
            }, Math.Min);
        }

        public int SolvePart2()
        {
            using var ctx = new Context();
            var opt = ctx.MkOptimize();

            int numTargets = TargetJolts.Length;
            int numButtons = ButtonActions.Count;

            // Each button can be pressed 0 or more times
            var numPresses = new IntExpr[numButtons];
            for (int j = 0; j < numButtons; j++)
            {
                numPresses[j] = ctx.MkIntConst($"x_{j}");
                opt.Assert(ctx.MkGe(numPresses[j], ctx.MkInt(0))); // numPresses[j] >= 0
            }

            // Define how button presses effect target jolts
            // for each target, its value is the sum of all the button presses
            // that effect that target
            foreach (var target in TargetJolts.Index())
            {
                var terms = ButtonActions.Index()
                    .Where(button => button.Item.Contains(target.Index))
                    .Select(button => numPresses[button.Index]).ToList();

                var lhs = terms.Count == 0 ? ctx.MkInt(0) : ctx.MkAdd(terms);
                var rhs = ctx.MkInt(target.Item);

                opt.Assert(ctx.MkEq(lhs, rhs));
            }

            // Objective: minimize sum of press values  (total presses)
            var sumPresses = ctx.MkAdd(numPresses);
            opt.MkMinimize(sumPresses);

            if (opt.Check() != Status.SATISFIABLE) return 0; // No solution

            // result 
            var model = opt.Model;
            return numPresses.Sum(press => ((IntNum)model.Eval(press, true)).Int);
        }

        [Regex(@"\[(.+)\]")] public void ParseLights(string l) => TargetLights = [.. l.Select(c => c == '#')];
        [Regex(@"\((.+)\)")] public void ParseButton(int[] actions) => ButtonActions.Add([.. actions]);
        [Regex(@"\{(.+)\}")] public void ParseJolts(int[] jolts) => TargetJolts = jolts;
    }

    static IEnumerable<Machine> Parse(string input) => Util.Split(input, "\n").Select(r => new Machine(r));

    public static int Part1(string input) => Parse(input).Sum(r => r.SolvePart1());

    public static int Part2(string input, ILogger logger = null)
        => Parse(input)
          .AsParallel()
          .Select((r) => r.SolvePart2()).Sum();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, logger));
    }
}