using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day04 : IPuzzle
    {
        public string Name { get { return "2021-04"; } }

        class Board
        {
            public Board(string input)
            {
                Util.Split(input)
                    .WithIndex()
                    .ForEach(line => 
                        Util.Parse32(line.Value, ' ')
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

            Dictionary<int, (int x, int y)> index = new Dictionary<int, (int x, int y)>();
            int[,] numbers = new int[5, 5];

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

        static int RunGame(string input, Util.QuestionPart part)
        {
            var chunks = input.Split("\n\n");
            var rnd = Util.Parse32(chunks[0]);

            var boards = Util.Parse<Board>(chunks.Skip(1));

            foreach (var num in rnd)
            {
                foreach (var board in boards.Where(b => !b.Complete))
                {
                    if (board.PlayNumber(num))
                    {
                        if (part == Util.QuestionPart.Part1 || 
                            boards.Count(board => board.Complete) == boards.Count())
                            return board.CalcScore(num);
                    }
                }
            }
            return 0;
        }

        public static int Part1(string input)
        {
            return RunGame(input, Util.QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return RunGame(input, Util.QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}