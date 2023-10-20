using AoC.Utils;
using System.Collections.Generic;

namespace AoC.Advent2019
{
    public class Day13 : IPuzzle
    {
        public string Name => "2019-13";

        public class NPVGS : NPSA.ICPUInterrupt
        {
            readonly NPSA.IntCPU cpu;
            readonly HashSet<long> blocks = new();

            enum Tile
            {
                Empty = 0,
                Wall = 1,
                Block = 2,
                Paddle = 3,
                Ball = 4
            }

            long ballPos, paddlePos, score;

            public NPVGS(string program)
            {
                cpu = new NPSA.IntCPU(program, 3200) { Interrupt = this };
            }

            public long Run(QuestionPart part)
            {
                cpu.Run();
                return part.One() ? blocks.Count : score;
            }

            public void InsertCoin() => cpu.Poke(0, 2);

            public void RequestInput() => cpu.AddInput(ballPos - paddlePos);

            public void OutputReady()
            {
                if (cpu.Output.Count == 3)
                {
                    var (xPos, yPos, tile) = cpu.Output.TakeThree();

                    if (xPos == -1 && yPos == 0) score = tile;
                    else if (tile == (long)Tile.Paddle) paddlePos = xPos;
                    else if (tile == (long)Tile.Ball) ballPos = xPos;
                    else if (tile == (long)Tile.Block) blocks.Add(xPos + (yPos<<32));
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
}