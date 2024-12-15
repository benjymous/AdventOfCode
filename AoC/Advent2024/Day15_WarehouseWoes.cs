namespace AoC.Advent2024;
public class Day15 : IPuzzle
{
    class Warehouse
    {
        public Warehouse(string input)
        {
            var parts = input.SplitSections();

            var grid = Util.ParseSparseMatrix<char>(parts[0], new Util.Convertomatic.SkipChars('.'));
            Instructions = parts[1].Replace("\n", "").Select(c => new Direction2(c)).ToList();

            Bot = grid.KeysWithValue('@').Single();
            Boxes = grid.KeysWithValue('O').ToHashSet();
            WideBoxes = grid.KeysWithValue('[').ToHashSet();
            Walls = grid.KeysWithValue('#').ToHashSet();
        }

        public int PerformMoves()
        {
            Instructions.ForEach(d => { var np = Bot.OffsetBy(d); if (TryMove(np, d)) Bot = np; });
            return Boxes.Sum(p => (p.y * 100) + p.x) + WideBoxes.Sum(p => (p.y * 100) + p.x);
        }

        static bool DoMove(HashSet<(int, int)> set, (int, int) oldPos, (int, int) newPos)
            => set.Remove(oldPos) && set.Add(newPos);

        bool TryMove((int x, int y) pos, Direction2 dir)
        {
            var isBox = Boxes.Contains(pos);
            var isWideBox = WideBoxes.Contains(pos);
            var isWideBoxSide = WideBoxes.Contains((pos.x - 1, pos.y));
            var isWall = Walls.Contains(pos);
            var nextPos = pos.OffsetBy(dir);
            var nextPosRight = (pos.x + 1, pos.y).OffsetBy(dir);
            return (!isBox && !isWall && !isWideBox && !isWideBoxSide) || 
                   (!isWall && (isBox ? TryMove(nextPos, dir) &&
                DoMove(Boxes, pos, nextPos) : isWideBox ? dir.DY == 0 ? TryMove(nextPos, dir) &&
                DoMove(WideBoxes, pos, nextPos) : TestMove(nextPos, dir) && TestMove(nextPosRight, dir) &&
                TryMove(nextPos, dir) && TryMove(nextPosRight, dir) && DoMove(WideBoxes, pos, nextPos) :
                isWideBoxSide && (dir.DY == 0 ? (dir.DX == -1 ? TryMove((pos.x - 1, pos.y), dir) :
                TryMove((pos.x + 1, pos.y), dir)) : TryMove((pos.x - 1, pos.y), dir))));
        }

        bool TestMove((int x, int y) pos, Direction2 dir)
        {
            var isWideBox = WideBoxes.Contains(pos);
            var isWideBoxSide = WideBoxes.Contains((pos.x - 1, pos.y));
            var isWall = Walls.Contains(pos);
            return (!isWall && !isWideBox && !isWideBoxSide) || !isWall && (isWideBox ? dir.DY == 0 ?
                TestMove(pos.OffsetBy(dir), dir) : TestMove(pos.OffsetBy(dir), dir) &&
                TestMove((pos.x + 1, pos.y).OffsetBy(dir), dir) : isWideBoxSide && TestMove((pos.x - 1, pos.y), dir));
        }

        (int x, int y) Bot;
        readonly HashSet<(int x, int y)> Boxes, WideBoxes, Walls;

        readonly List<Direction2> Instructions;
    }

    static string DoubleMap(string input)
        => input.Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");

    public static int Part1(string input) => new Warehouse(input).PerformMoves();
    public static int Part2(string input) => new Warehouse(DoubleMap(input)).PerformMoves();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}