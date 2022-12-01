using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day04 : IPuzzle
    {
        public string Name => "2021-04";

        class Board
        {
            public Board(string input)
            {
                Util.Split(input)
                    .WithIndex()
                    .ForEach(line =>
                        Util.ParseNumbers<int>(line.Value, ' ')
                             .WithIndex()
                             .ForEach(row =>
                                Put(row.Index, line.Index, row.Value)
                              )
                     );
            }

            void Put(int x, int y, int num)
            {
                numbers[y, x] = num;
                index[num] = (x, y);
            }

            readonly Dictionary<int, (int x, int y)> index = new();
            readonly int[,] numbers = new int[5, 5];

            public bool PlayNumber(int num)
            {
                if (index.TryGetValue(num, out var pos))
                {
                    numbers[pos.y, pos.x] = -1;

                    Complete = CheckRow(pos.y) || CheckCol(pos.x);
                }

                return Complete;
            }

            bool CheckRow(int y) => numbers.Row(y).All(i => i == -1);

            bool CheckCol(int x) => numbers.Column(x).All(i => i == -1);

            public int CalcScore(int turn) => numbers.Values().Where(i => i > -1).Sum() * turn;

            public bool Complete { get; private set; } = false;
        }

        static int RunGame(string input, QuestionPart part)
        {
            var chunks = input.Split("\n\n");
            var rnd = Util.ParseNumbers<int>(chunks[0]);

            var boards = Util.Parse<Board>(chunks.Skip(1));

            foreach (var num in rnd)
            {
                foreach (var board in boards.Where(b => !b.Complete))
                {
                    if (board.PlayNumber(num))
                    {
                        if (part.One() ||
                            boards.Count(board => board.Complete) == boards.Count)
                            return board.CalcScore(num);
                    }
                }
            }
            return 0;
        }

        public static int Part1(string input)
        {
            return RunGame(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return RunGame(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}