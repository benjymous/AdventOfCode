using AoC.Utils;
using AoC.Utils.Vectors;
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
            NPSA.IntCPU cpu;

            Dictionary<string, int> screen = new Dictionary<string, int>();

            // 0 is an empty tile. No game object appears in this tile.
            // 1 is a wall tile. Walls are indestructible barriers.
            // 2 is a block tile. Blocks can be broken by the ball.
            // 3 is a horizontal paddle tile. The paddle is indestructible.
            // 4 is a ball tile. The ball moves diagonally and bounces off objects.

            const string tiles = " █■━●";

            public NPVGS(string program)
            {
                cpu = new NPSA.IntCPU(program);
                cpu.Reserve(3200);
                cpu.Interrupt = this;
            }

            public int Run()
            {
                cpu.Run();

                return screen.Count(kvp => kvp.Value == 2);
            }

            public void InsertCoin()
            {
                cpu.Poke(0, 2);
            }

            public int Score()
            {
                return screen.GetStrKey("-1,0");
            }

            ManhattanVector2 FindCell(int tile)
            {
                return new ManhattanVector2(screen.Where(kvp => kvp.Value == tile).First().Key);
            }

            public void WillReadInput()
            {

                var ball = FindCell(4);
                var paddle = FindCell(3);

                int joystick = Math.Sign(ball.X - paddle.X);

                cpu.Input.Enqueue(joystick);

                //Draw();
            }

            public void HasPutOutput()
            {
                if (cpu.Output.Count == 3)
                {
                    int xPos = (int)cpu.Output.Dequeue();
                    int yPos = (int)cpu.Output.Dequeue();
                    int tile = (int)cpu.Output.Dequeue();

                    screen[$"{xPos},{yPos}"] = tile;

                    // if (tile == 4)
                    // {
                    //     Draw();
                    // }
                }
            }

            public void Draw()
            {
                Console.WriteLine();
                Console.WriteLine();
                for (int y = 0; y < 25; ++y)
                {
                    var str = "";
                    for (int x = 0; x <= 40; ++x)
                    {
                        str += tiles[screen.GetStrKey($"{x},{y}")] + " ";
                    }
                    Console.WriteLine(str);
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        public static int Part1(string input)
        {
            var game = new NPVGS(input);

            return game.Run();
        }

        public static int Part2(string input)
        {
            var game = new NPVGS(input);
            game.InsertCoin();
            game.Run();
            return game.Score();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}