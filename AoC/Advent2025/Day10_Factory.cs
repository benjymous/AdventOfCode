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

        public int Solve(QuestionPart part)
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

            if (part.One)
            {
                // Define how button presses effect the lights
                foreach (var light in TargetLights.Index())
                {
                    var termsSum = EnumerateButtons(ctx, numPresses, light.Index);

                    var lhs = ctx.MkMod((IntExpr)termsSum, ctx.MkInt(2));
                    var rhs = ctx.MkInt(light.Item ? 1 : 0);

                    opt.Assert(ctx.MkEq(lhs, rhs));
                }
            }
            else
            {
                // Define how button presses effect target jolts
                // for each target, its value is the sum of all the button presses
                // that effect that target
                foreach (var target in TargetJolts.Index())
                {
                    var lhs = EnumerateButtons(ctx, numPresses, target.Index);

                    var rhs = ctx.MkInt(target.Item);

                    opt.Assert(ctx.MkEq(lhs, rhs));
                }
            }

            // Objective: minimize sum of press values  (total presses)
            var sumPresses = ctx.MkAdd(numPresses);
            opt.MkMinimize(sumPresses);

            if (opt.Check() != Status.SATISFIABLE) return 0; // No solution

            // result 
            var model = opt.Model;
            return numPresses.Sum(press => ((IntNum)model.Eval(press, true)).Int);
        }

        private ArithExpr EnumerateButtons(Context ctx, IntExpr[] numPresses, int targetIndex)
        {
            var terms = ButtonActions.Index()
                .Where(button => button.Item.Contains(targetIndex))
                .Select(button => numPresses[button.Index]).ToList();

            return terms.Count == 0 ? ctx.MkInt(0) : ctx.MkAdd(terms);
        }

        [Regex(@"\[(.+)\]")] public void ParseLights(string l) => TargetLights = [.. l.Select(c => c == '#')];
        [Regex(@"\((.+)\)")] public void ParseButton(int[] actions) => ButtonActions.Add([.. actions]);
        [Regex(@"\{(.+)\}")] public void ParseJolts(int[] jolts) => TargetJolts = jolts;
    }

    static IEnumerable<Machine> Parse(string input) => Util.Split(input, "\n").Select(r => new Machine(r));
    static int Solve(IEnumerable<Machine> machines, QuestionPart part) => machines.AsParallel().Sum(m => m.Solve(part));

    public static int Part1(string input) => Solve(Parse(input), QuestionPart.Part1);

    public static int Part2(string input) => Solve(Parse(input), QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}