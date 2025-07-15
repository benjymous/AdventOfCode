namespace AoC.Advent2022;
public class Day17 : IPuzzle
{
    private static readonly (int x, int y)[][] Shapes = [.. Util.Values("####", ".#.,###,.#.", "###,..#,..#", "#,#,#,#", "##,##").Select(part => Util.ParseSparseMatrix<bool>(part).Keys.OrderByDescending(k => k.y).ToArray())];

    private class State(string input)
    {
        private readonly int[] map = new int[10000], windData = [.. input.Trim().Select(c => c == '<' ? -1 : 1)];
        public int WindIdx { get; private set; } = 0;
        public int MaxHeight { get; private set; } = 0;

        public int WindDirection()
        {
            int res = windData[WindIdx];
            WindIdx = (WindIdx + 1) % windData.Length;
            return res;
        }

        private bool Blocked((int x, int y) pos) => pos.y <= 0 || pos.x <= 0 || pos.x >= 8 || (MaxHeight + 1 > pos.y && ((map[pos.y] & (1 << pos.x)) != 0));
        public bool CheckBlocked((int x, int y)[] shape, int dx, int dy) => shape.Any((pos) => Blocked((pos.x + dx, pos.y + dy)));

        public void FinishBlock((int x, int y)[] shape, (int x, int y) pos)
        {
            MaxHeight = Math.Max(MaxHeight, shape[0].y + pos.y);
            for (int i = 0; i < shape.Length; ++i) map[pos.y + shape[i].y] |= 1 << (pos.x + shape[i].x);
        }
    }

    private static ulong RunRocktris(string input, ulong rounds)
    {
        var state = new State(input);
        HashSet<int> seen = [];

        int currentShape = 0, findCombo = -1;
        ulong benchmarkHeight = 0, firstRepeatIndex = 0, repeatHeight = 0, secondRepeatIndex = 0, targetRound = rounds;
        for (ulong i = 0; i < targetRound; ++i)
        {
            var shape = Shapes[currentShape];
            for ((int x, int y) rockPos = (3, 4 + state.MaxHeight); true; rockPos.y--)
            {
                var windDir = state.WindDirection();
                if (!state.CheckBlocked(shape, rockPos.x + windDir, rockPos.y)) rockPos.x += windDir;
                if (state.CheckBlocked(shape, rockPos.x, rockPos.y - 1))
                {
                    state.FinishBlock(shape, rockPos);
                    break;
                }
            }
            currentShape = (currentShape + 1) % 5;

            int checkCombo = currentShape + ((state.WindIdx) << 3);
            if (findCombo == -1)
            {
                if (!seen.Add(checkCombo))
                {
                    findCombo = checkCombo;
                    benchmarkHeight = (ulong)state.MaxHeight;
                    firstRepeatIndex = i;
                }
            }
            else if (checkCombo == findCombo)
            {
                repeatHeight = (ulong)state.MaxHeight - benchmarkHeight;
                secondRepeatIndex = i - firstRepeatIndex;
                targetRound = i + ((rounds - firstRepeatIndex) % secondRepeatIndex);
            }
        }

        return secondRepeatIndex == 0
            ? (ulong)state.MaxHeight
            : benchmarkHeight + ((rounds - firstRepeatIndex) / secondRepeatIndex * repeatHeight) + ((ulong)state.MaxHeight - benchmarkHeight - repeatHeight);
    }

    public static ulong Part1(string input) => RunRocktris(input, 2022);

    public static ulong Part2(string input) => RunRocktris(input, 1000000000000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}