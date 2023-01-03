using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day17 : IPuzzle
    {
        public string Name => "2022-17";

        readonly static (int x, int y)[][] Shapes = Util.Values("####",".#.,###,.#.","###,..#,..#","#,#,#,#","##,##").Select(part => Util.ParseSparseMatrix<bool>(part).Keys.ToArray()).ToArray();

        class State
        {
            public State(string input) => windData = input.Trim().Select(c => c == '<' ? -1:1).ToArray();

            readonly List<byte> map = new() { 255 };
            readonly int[] windData;
            public int windIdx = 0;

            public int MaxHeight => map.Count-1;

            public int WindDirection()
            {
                int res = windData[windIdx];
                windIdx = (windIdx + 1) % windData.Length;
                return res;
            }

            bool Blocked((int x, int y) pos) => pos.y <= 0 || pos.x <= 0 || pos.x >= 8 || map.Count>pos.y && ((map[pos.y] & 1 << pos.x) != 0);
            public bool CheckBlocked(IEnumerable<(int x, int y)> shape, int dx, int dy) => shape.Any((pos) => Blocked((pos.x + dx, pos.y + dy)));

            public void FinishBlock(IEnumerable<(int x, int y)> shape, (int x, int y) pos)
            {
                foreach (var (x, y) in shape.Select(p => (p.x + pos.x, p.y + pos.y)))
                {
                    while (map.Count < y + 1) map.Add(0);
                    map[y] |= (byte)(1 << x);
                }
            }
        }

        private static ulong RunRocktris(string input, ulong rounds)
        {
            var state = new State(input);
            HashSet<int> seen = new();

            int currentShape = 0, findWindIndex = -1;
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

                if (currentShape == 0)
                {
                    if (findWindIndex == -1)
                    {
                        if (seen.Contains(state.windIdx))
                        {
                            findWindIndex = state.windIdx;
                            benchmarkHeight = (uint)state.MaxHeight;
                            firstRepeatIndex = i;
                        }
                        else seen.Add(state.windIdx);
                    }
                    else if (state.windIdx == findWindIndex)
                    {
                        repeatHeight = (ulong)state.MaxHeight - benchmarkHeight;
                        secondRepeatIndex = i - firstRepeatIndex;
                        targetRound = i + (rounds - firstRepeatIndex) % secondRepeatIndex;
                    }
                }
            }

            return secondRepeatIndex == 0
                ? (ulong)state.MaxHeight
                : benchmarkHeight + ((rounds - firstRepeatIndex) / secondRepeatIndex * repeatHeight) + ((ulong)state.MaxHeight - benchmarkHeight - repeatHeight);
        }

        public static int Part1(string input) => (int)RunRocktris(input, 2022);

        public static ulong Part2(string input) => RunRocktris(input, 1000000000000);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}