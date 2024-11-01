using State = (int x, int y, int dir, int dirSteps, int heatLoss);

namespace AoC.Advent2023;
public class Day17 : IPuzzle
{
    static readonly Direction2[] Directions = [Direction2.North, Direction2.East, Direction2.South, Direction2.West];

    private static int Solve(string input, QuestionPart part)
    {
        var grid = Util.ParseMatrix<byte>(input);

        (int x, int y) target = (grid.Width() - 1, grid.Height() - 1);

        int maxStraight = part.One() ? 3 : 10, minStop = part.One() ? 1 : 4;

        return Solver<State, int>.Solve((0, 0, 1, 0, 0), (state, solver) =>
        {
            if (solver.IsBetterThanSeen((state.x, state.y, state.dir, state.dirSteps), state.heatLoss))
            {
                if (((state.x, state.y) == target) && state.dirSteps >= minStop)
                {
                    return state.heatLoss;
                }

                if (state.dirSteps < maxStraight)
                {
                    AddNext(solver, grid, state, state.dir, target);
                }

                if (part.One() || state.dirSteps is >= 4 or 0)
                {
                    AddNext(solver, grid, state, state.dir - 1, target);
                    AddNext(solver, grid, state, state.dir + 1, target);
                }
            }

            return default;
        }, Math.Min);
    }

    private static void AddNext(Solver<State, int> solver, byte[,] grid, State current, int nextDir, (int x, int y) target)
    {
        nextDir = (nextDir + 4) % 4;
        var nextPos = (current.x, current.y).OffsetBy(Directions[nextDir]);
        if (grid.Contains(nextPos))
        {
            var nextLoss = current.heatLoss + grid[nextPos.x, nextPos.y];
            solver.Enqueue((nextPos.x, nextPos.y, nextDir, nextDir == current.dir ? current.dirSteps + 1 : 1, nextLoss), target.Distance(nextPos) + nextLoss);
        }
    }

    public static int Part1(string input) => Solve(input, QuestionPart.Part1);

    public static int Part2(string input) => Solve(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}