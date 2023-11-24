namespace AoC.Advent2020;
public class Day11 : IPuzzle
{
    public class State
    {
        public State(string input, QuestionPart p)
        {
            part = p;
            var data = Util.ParseSparseMatrix<PackedPos32, char>(input, new Util.Convertomatic.SkipChars('.'));
            (Width, Height) = (data.Keys.Max(pos => pos.X), data.Keys.Max(pos => pos.Y));
            Seats = [.. data.Keys];
            Occupied = data.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();
        }

        public State(State other) => (Width, Height, part, Seats) = (other.Width, other.Height, other.part, other.Seats);

        readonly int Height, Width;
        readonly QuestionPart part;
        public readonly HashSet<PackedPos32> Occupied = [], Seats = [];

        int MaxOccupancy => part.One() ? 4 : 5;

        public int Neighbours(PackedPos32 pos) => directions.Count(d => CheckDirection(pos, d) == true);
        bool Inside(PackedPos32 pos) => (pos.X >= 0) && (pos.X <= Width) && (pos.Y >= 0) && (pos.Y <= Height);

        public bool CheckDirection(PackedPos32 pos, int dir)
        {
            if (part.One()) return Occupied.Contains(pos + dir);
            else while (Inside(pos += dir)) if (Seats.Contains(pos)) return Occupied.Contains(pos);
            return false;
        }

        static readonly PackedPos32[] directions = [(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1)];

        public bool Tick(State oldState, PackedPos32 pos)
        {
            int neighbours = oldState.Neighbours(pos);

            if (oldState.Occupied.Contains(pos))
            {
                if (neighbours >= MaxOccupancy) return true;
                Occupied.Add(pos);
            }
            else if (neighbours == 0)
            {
                Occupied.Add(pos);
                return true;
            }

            return false;
        }
    }

    public static bool Tick(State oldState, State newState)
    {
        newState.Occupied.Clear();
        return oldState.Seats.Where(key => newState.Tick(oldState, key)).ToArray().Length != 0;
    }

    public static int Run(string input, QuestionPart part)
    {
        State s1 = new(input, part), s2 = new(s1);
        do (s1, s2) = (s2, s1); while (Tick(s1, s2));
        return s1.Occupied.Count;
    }

    public static int Part1(string input) => Run(input, QuestionPart.Part1);

    public static int Part2(string input) => Run(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}