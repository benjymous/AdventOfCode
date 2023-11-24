using CritterPos = AoC.Utils.Vectors.PackedVect2<int, AoC.Advent2021.Day23.PackCritterPos>;
namespace AoC.Advent2021;
public class Day23 : IPuzzle
{
    static readonly (CritterPos Pos, char Value)[] destinations = [((3, 2), 'A'), ((5, 2), 'B'), ((7, 2), 'C'), ((9, 2), 'D'), ((3, 3), 'A'), ((5, 3), 'B'), ((7, 3), 'C'), ((9, 3), 'D'), ((3, 4), 'A'), ((5, 4), 'B'), ((7, 4), 'C'), ((9, 4), 'D'), ((3, 5), 'A'), ((5, 5), 'B'), ((7, 5), 'C'), ((9, 5), 'D')];
    static readonly int[] destinationCol = [3, 5, 7, 9], moveCosts = [1, 10, 100, 1000];

    public class PackCritterPos : ICoordinatePacker2<int>
    {
        public static int GetX(int v) => (v % 11) + 1;
        public static int GetY(int v) => (v / 11) + 1;
        public static int Set((int x, int y) v) => v.x - 1 + ((v.y - 1) * 11);
    }

    static bool IsClear(int x1, int x2, State state) => !Util.RangeInclusive(Math.Min(x1, x2), Math.Max(x1, x2)).Any(x => !state.CellOpen(x - 1));

    readonly record struct State(ulong OpenCells, Dictionary<CritterPos, char> Critters, int Score, int _)
    {
        public State(ulong openCells, Dictionary<CritterPos, char> critters, int score) : this(openCells, critters, score, 0) => Util.RepeatWhile(() => destinations.Where(kvp => critters.TryGetValue(kvp.Pos, out var other) && other == kvp.Value && ((1UL << (kvp.Pos + 11)) & openCells) == 0 && !critters.ContainsKey(kvp.Pos + 11)).Select(kvp => kvp.Pos).ForEach(key => critters.Remove(key)));

        public readonly bool CellOpen(int cell) => ((1UL << cell) & (OpenCells)) != 0;

        public override readonly int GetHashCode() => (Critters.Keys.Select(k => 1UL << k).Sum(), Critters.OrderBy(kvp => kvp.Key.V).Select(kvp => (uint)(kvp.Value - 'A')).Aggregate(0U, (p, v) => (p << 2) + v)).GetHashCode();
    }

    static IEnumerable<(CritterPos origin, CritterPos destination, char value, int spaces)> CritterMoves(State state, KeyValuePair<CritterPos, char> critter)
    {
        if (critter.Key.Y == 1 || state.CellOpen(critter.Key - 11)) // can this critter reach its home room?
        {
            int destX = destinationCol[critter.Value - 'A'];
            if (IsClear(critter.Key.X + Math.Sign(destX - critter.Key.X), destX, state))
            {
                for (var destY = 5; destY >= 2; --destY)
                {
                    CritterPos dest = (destX, destY);
                    if (state.CellOpen(dest) && !state.CellOpen(dest + 11) && !state.Critters.ContainsKey(dest + 11))
                    {
                        yield return (critter.Key, dest, critter.Value, Math.Abs(critter.Key.X - destX) + (destY - 1) + (critter.Key.Y - 1));
                        yield break; // if the critter can get home, no point trying anywhere else
                    }
                }
            }
        }

        if (critter.Key.Y >= 2 && state.CellOpen(critter.Key - 11)) // critter in room, and cell above is empty
        {
            for (int dir = -1; dir <= 1; dir += 2)
                for (int i = 1; i < 9; i++)
                {
                    int x = critter.Key.X + (i * dir);
                    if (x > 0 && x < 12 && !destinationCol.Contains(x))
                        if (state.CellOpen(x - 1)) yield return (critter.Key, x - 1, critter.Value, critter.Key.Y - 1 + i);
                        else break; // if it can't move to i, it won't be able to move to i+1 either
                }
        }
    }

    public static int ShrimpStacker(IEnumerable<string> input)
    {
        var map = Util.ParseSparseMatrix<CritterPos, char>(input, CritterPos.Convert, new Util.Convertomatic.SkipChars('#'));
        var initial = new State(map.Where(kvp => kvp.Value == '.').Select(kvp => 1UL << kvp.Key).Sum(), map.Where(kvp => kvp.Value is >= 'A' and <= 'D').ToDictionary(), 0);

        return Solver<State, int>.Solve(initial, (state, solver) =>
        {
            foreach (var newState in state.Critters.SelectMany(critter => CritterMoves(state, critter)).Select(move => new State(state.OpenCells - (1UL << move.destination) + (1UL << move.origin), new Dictionary<CritterPos, char>(state.Critters).Move(move.origin, move.destination, move.value), state.Score + (move.spaces * moveCosts[move.value - 'A']))))
            {
                var estimatedScore = newState.Score + newState.Critters.Sum(v => (v.Key.Y - 1 + Math.Abs(v.Key.X - destinationCol[v.Value - 'A'])) * moveCosts[v.Value - 'A']);
                if (solver.IsBetterThanSeen(newState, estimatedScore))
                    if (newState.Critters.Count != 0) solver.Enqueue(newState, estimatedScore);
                    else return newState.Score;
            }
            return default;
        }, Math.Min);
    }

    public static int Part1(string input) => ShrimpStacker(input.Split('\n'));

    public static int Part2(string input)
    {
        string[] elements = ["  #D#C#B#A#", "  #D#B#A#C#"];
        return ShrimpStacker(input.Split('\n').InsertRangeAt(elements, 3));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}