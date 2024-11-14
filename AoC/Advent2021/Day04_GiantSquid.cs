namespace AoC.Advent2021;
public class Day04 : IPuzzle
{
    class Board
    {
        public Board(string input) => Util.Split(input)
                .WithIndex()
                .ForEach(line =>
                    Util.ParseNumbers<int>(line.Value, " ")
                         .WithIndex()
                         .ForEach(row =>
                            Put(row.Index, line.Index, row.Value)
                          )
                 );

        void Put(int x, int y, int num)
        {
            numbers[x, y] = num;
            index[num] = (x, y);
        }

        readonly Dictionary<int, (int x, int y)> index = [];
        readonly int[,] numbers = new int[5, 5];

        public bool PlayNumber(int num)
        {
            if (index.TryGetValue(num, out var pos))
            {
                numbers[pos.x, pos.y] = -1;
                index.Remove(num);

                return CheckRow(pos.y) || CheckCol(pos.x);
            }

            return false;
        }

        bool CheckRow(int y) => numbers.Row(y).All(i => i == -1);

        bool CheckCol(int x) => numbers.Column(x).All(i => i == -1);

        public int CalcScore(int turn) => index.Keys.Sum() * turn;
    }

    static int RunGame(string input, QuestionPart part)
    {
        var chunks = input.SplitSections();
        var rnd = Util.ParseNumbers<int>(chunks[0]);

        var boards = Util.Parse<Board>(chunks.Skip(1)).ToHashSet();

        foreach (var num in rnd)
        {
            foreach (var board in boards)
            {
                if (board.PlayNumber(num))
                {
                    if (part.One() || boards.Count == 1)
                        return board.CalcScore(num);

                    boards.Remove(board);
                }
            }
        }
        return 0;
    }

    public static int Part1(string input) => RunGame(input, QuestionPart.Part1);

    public static int Part2(string input) => RunGame(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}