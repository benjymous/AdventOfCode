namespace AoC.Advent2024;
public class Day13 : IPuzzle
{
    [Regex(@"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)")]
    public record class ClawMachine(long X1, long Y1, long X2, long Y2, long X3, long Y3)
    {
        public long Solve(long additional = 0)
        {
            var (a, b) = SolveLinearEquations((X1, X2, Y1, Y2), (X3 + additional, Y3 + additional));

            return !a.IsInteger() || !b.IsInteger() ? 0 : (long)((a * 3) + b);
        }
    }

    private static (double A, double B) SolveLinearEquations(
        (double x1, double x2, double y1, double y2) coefficients,
        (double x3, double y3) constants)
    {
        double det = coefficients.x1 * coefficients.y2 - coefficients.x2 * coefficients.y1;
        return (
            (constants.x3 * coefficients.y2 - constants.y3 * coefficients.x2) / det,
            (coefficients.x1 * constants.y3 - coefficients.y1 * constants.x3) / det
        );
    }

    public static long Solve(string input, long additional = 0)
        => Parser.Parse<ClawMachine>(input.SplitSections())
                 .Sum(m => m.Solve(additional));

    public static long Part1(string input) => Solve(input);
    public static long Part2(string input) => Solve(input, 10000000000000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}