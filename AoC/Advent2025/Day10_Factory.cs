using Microsoft.Z3;

namespace AoC.Advent2025;

public class Day10 : IPuzzle
{
    public class Machine
    {
        bool[] TargetLights = [];
        int[] TargetJolts = [];
        readonly List<int[]> ButtonActions = [];

        [Regex("(.+)")] public Machine(string input) => Parser.Factory(input, this, " ");

        public int Solve(QuestionPart part)
        {
            using var ctx = new Context();
            var opt = ctx.MkOptimize();

            int numButtons = ButtonActions.Count;

            // Each button can be pressed 0 or more times, must be whole number
            var numPresses = new IntExpr[numButtons];
            for (int j = 0; j < numButtons; j++)
            {
                numPresses[j] = ctx.MkIntConst(ctx.MkSymbol(j));
                opt.Assert(ctx.MkGe(numPresses[j], ctx.MkInt(0))); // numPresses[j] >= 0
            }

            // Define how button presses effect the target values
            // for each target, its value is the sum of all the button presses
            // that effect that target
            if (part.One)
            {
                // lights are toggled by presses
                foreach (var light in TargetLights.Index())
                {
                    var termsSum = SumOfPressesForTarget(ctx, numPresses, light.Index);

                    var lhs = ctx.MkMod((IntExpr)termsSum, ctx.MkInt(2)); // check odd (on) or even (off)
                    var rhs = ctx.MkInt(light.Item ? 1 : 0);

                    opt.Assert(ctx.MkEq(lhs, rhs));
                }
            }
            else
            {
                // jolts are incremented by presses
                foreach (var target in TargetJolts.Index())
                {
                    var lhs = SumOfPressesForTarget(ctx, numPresses, target.Index);
                    var rhs = ctx.MkInt(target.Item);

                    opt.Assert(ctx.MkEq(lhs, rhs));
                }
            }

            // Objective: minimize sum of press values  (total presses)
            var sumPresses = ctx.MkAdd(numPresses);
            opt.MkMinimize(sumPresses);

            if (opt.Check() != Status.SATISFIABLE) return 0; // No solution

            // result 
            return ((IntNum)opt.Model.Eval(sumPresses)).Int;
        }

        private ArithExpr SumOfPressesForTarget(Context ctx, IntExpr[] numPresses, int targetIndex)
        {
            return ctx.MkAdd(ButtonActions.Index()
                .Where(button => button.Item.Contains(targetIndex))
                .Select(button => numPresses[button.Index]));
        }

        [Regex(@"\[(.+)\]")] public void ParseLights(string l) => TargetLights = [.. l.Select(c => c == '#')];
        [Regex(@"\((.+)\)")] public void ParseButton(int[] actions) => ButtonActions.Add(actions);
        [Regex(@"\{(.+)\}")] public void ParseJolts(int[] jolts) => TargetJolts = jolts;
    }

    static int Solve(Parser.AutoArray<Machine> machines, QuestionPart part) => machines.AsParallel().Sum(m => m.Solve(part));

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);
    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}