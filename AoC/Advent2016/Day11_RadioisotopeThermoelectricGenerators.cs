namespace AoC.Advent2016;
public class Day11 : IPuzzle
{
    public readonly struct State
    {
        public State(string input, QuestionPart part)
        {
            Dictionary<string, byte> nameLookup = [];
            Floors = [.. Util.Split(input, "\n").Select(line => ((byte chips, byte generators))line.Split(' ').Select(v => v.Length > 3 ? v[..3] : v).OverlappingPairs().Select(pair => (pair.second == "mic" ? nameLookup.GetIndexBit(pair.first) : 0, pair.second == "gen" ? nameLookup.GetIndexBit(pair.first) : 0)).Aggregate(((byte)0, (byte)0), (curr, next) => ((byte)(curr.Item1 + next.Item1), (byte)(curr.Item2 + next.Item2))))];

            if (part.Two)
            {
                byte nextTwoIds = (byte)(nameLookup.Values.Last() * 6);
                Floors[0].chips += nextTwoIds; Floors[0].generators += nextTwoIds;
            }
        }

        private State(State previous, int newFloor, int moveChips, int moveGens)
        {
            Steps = previous.Steps + 1;
            CurrentFloor = newFloor;
            var oldFloor = previous.CurrentFloor;

            Floors = [.. previous.Floors];

            Floors[oldFloor].chips -= (byte)moveChips;
            Floors[newFloor].chips += (byte)moveChips;

            Floors[oldFloor].generators -= (byte)moveGens;
            Floors[newFloor].generators += (byte)moveGens;

            if (Floors.All(floor => floor.chips == 0 || floor.generators == 0 || ((floor.generators & floor.chips) == floor.chips))) Remaining = Floors.Take(3).Sum(f => byte.PopCount(f.chips) + byte.PopCount(f.generators));
        }

        public IEnumerable<State> GetMoves()
        {
            IEnumerable<byte> currentChips = Floors[CurrentFloor].chips.BitSequence(), currentGens = Floors[CurrentFloor].generators.BitSequence();

            if (CurrentFloor < 3)
            {
                foreach (var chip1 in currentChips)
                {
                    foreach (var chip2 in currentChips.Where(c => c > chip1)) yield return new State(this, CurrentFloor + 1, chip1 | chip2, default);
                    yield return new State(this, CurrentFloor + 1, chip1, default);
                    foreach (var gen in currentGens) yield return new State(this, CurrentFloor + 1, chip1, gen);
                }
            }

            foreach (var gen1 in currentGens)
            {
                if (CurrentFloor > 0) yield return new State(this, CurrentFloor - 1, default, gen1);
                if (CurrentFloor < 3)
                {
                    foreach (var gen2 in currentGens.Where(g => g > gen1)) yield return new State(this, CurrentFloor + 1, default, gen1 | gen2);
                    yield return new State(this, CurrentFloor + 1, default, gen1);
                }
            }
        }

        private readonly (byte chips, byte generators)[] Floors;
        public readonly int CurrentFloor = 0, Steps = 0, Remaining = int.MaxValue;

        public override int GetHashCode() => ((Floors.Take(3).Aggregate(0UL, (prev, curr) => (prev << 14) + (ulong)(curr.chips << 7) + curr.generators) << 4) + (ulong)CurrentFloor).GetHashCode();
    }

    private static int FindBestPath(State initialState)
    {
        int closest = int.MaxValue;
        return Solver<State>.Solve(initialState, (state, solver) =>
        {
            foreach (var move in state.GetMoves().Where(move => solver.IsBetterThanCurrentBest(move.Steps) && move.Remaining - closest <= 2 && solver.IsBetterThanSeen(move, move.Steps)))
            {
                if (move.Remaining == 0) return move.Steps;
                closest = Math.Min(move.Remaining, closest);
                solver.Enqueue(move, move.Remaining);
            }
            return default;
        }, Math.Min);
    }

    public static int Part1(string input) => FindBestPath(new State(input, QuestionPart.Part1));

    public static int Part2(string input) => FindBestPath(new State(input, QuestionPart.Part2));

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}