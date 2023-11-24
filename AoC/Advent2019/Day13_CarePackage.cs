namespace AoC.Advent2019;
public class Day13 : IPuzzle
{
    public class NPVGS(string program) : NPSA.IntCPU(program, 3200), NPSA.ICPUInterrupt
    {
        readonly HashSet<long> blocks = [];

        enum Tile
        {
            Empty = 0,
            Wall = 1,
            Block = 2,
            Paddle = 3,
            Ball = 4
        }

        long ballPos, paddlePos, score;

        public long Run(QuestionPart part)
        {
            base.Run();
            return part.One() ? blocks.Count : score;
        }

        public void InsertCoin() => Poke(0, 2);

        public void RequestInput() => AddInput(ballPos - paddlePos);

        public void OutputReady()
        {
            if (Output.Count == 3)
            {
                var (xPos, yPos, tile) = Output.TakeThree();

                if (xPos == -1 && yPos == 0) score = tile;
                else if (tile == (long)Tile.Paddle) paddlePos = xPos;
                else if (tile == (long)Tile.Ball) ballPos = xPos;
                else if (tile == (long)Tile.Block) blocks.Add(xPos + (yPos << 32));
            }
        }
    }

    public static long Part1(string input)
    {
        var game = new NPVGS(input);

        return game.Run(QuestionPart.Part1);
    }

    public static long Part2(string input)
    {
        var game = new NPVGS(input);
        game.InsertCoin();
        return game.Run(QuestionPart.Part2);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}