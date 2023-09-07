using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day13 : IPuzzle
    {
        public string Name => "2019-13";

        public class NPVGS : NPSA.ICPUInterrupt
        {
            readonly NPSA.IntCPU cpu;
            readonly Dictionary<(int x, int y), int> screen = new();

            // 0 is an empty tile. No game object appears in this tile.
            // 1 is a wall tile. Walls are indestructible barriers.
            // 2 is a block tile. Blocks can be broken by the ball.
            // 3 is a horizontal paddle tile. The paddle is indestructible.
            // 4 is a ball tile. The ball moves diagonally and bounces off objects.

            //const string tiles = " █■━●";

            int ballPos, paddlePos, score;
            readonly QuestionPart part;

            public NPVGS(string program, QuestionPart p)
            {
                cpu = new NPSA.IntCPU(program);
                cpu.Reserve(3200);
                cpu.Interrupt = this;
                part = p;
            }

            public int Run()
            {
                cpu.Run();

                return part.One() ? screen.Count(kvp => kvp.Value == 2) : score;
            }

            public void InsertCoin() => cpu.Poke(0, 2);

            public void RequestInput() => cpu.Input.Enqueue(Math.Sign(ballPos - paddlePos));

            public void OutputReady()
            {
                if (cpu.Output.Count == 3)
                {
                    int xPos = (int)cpu.Output.Dequeue();
                    int yPos = (int)cpu.Output.Dequeue();
                    int tile = (int)cpu.Output.Dequeue();

                    if (xPos == -1 && yPos == 0) score = tile;
                    if (tile == 3) paddlePos = xPos;
                    if (tile == 4) ballPos = xPos;
                    
                    if (part.One()) screen[(xPos,yPos)] = tile;
                }
            }
        }

        public static int Part1(string input)
        {
            var game = new NPVGS(input, QuestionPart.Part1);

            return game.Run();
        }

        public static int Part2(string input)
        {
            var game = new NPVGS(input, QuestionPart.Part2);
            game.InsertCoin();
            return game.Run();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}