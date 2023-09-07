using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;

namespace AoC.Advent2019
{
    public class Day19 : IPuzzle
    {
        public string Name => "2019-19";

        public class TestDrone
        {
            readonly NPSA.IntCPU cpu;

            public int Scans { get; private set; } = 0;

            readonly Dictionary<(int,int), long> Cache = new();

            public TestDrone(string program)
            {
                cpu = new NPSA.IntCPU(program);
                cpu.Reserve(1000);
            }

            public long Visit(int scanX, int scanY) => Cache.GetOrCalculate((scanX, scanY), _ =>
            {
                Scans++;
                cpu.Input.Enqueue(scanX);
                cpu.Input.Enqueue(scanY);
                cpu.Run();
                long res = cpu.Output.Dequeue();
                cpu.Reset();
                cpu.Reserve(1000);
                return res;
            });

            public void DrawDroneView(int xc, int yc, int size = 5, int boxSize = 1)
            {
                for (var y = yc - size; y <= yc + size; ++y)
                {
                    for (var x = xc - size; x <= xc + size; ++x)
                    {
                        var res = Visit(x, y);

                        if (res > 1) throw new Exception("Unexpected output");

                        if (boxSize > 1 && x >= xc && x < xc + boxSize && y >= yc && y < yc + boxSize)
                        {
                            Console.Write(res > 0 ? "[]" : "!!");
                        }
                        else
                        {
                            if (xc == x && yc == y)
                            {
                                Console.Write(res > 0 ? ">#" : "> ");
                            }
                            else
                            {
                                Console.Write(res > 0 ? "##" : "  ");
                            }
                        }

                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public static long Part1(string input)
        {

            long result = 0;
            const int scanSize = 50;

            var drone = new TestDrone(input);

            for (int y = 0; y < scanSize; ++y)
            {
                for (int x = 0; x < scanSize; ++x)
                {
                    var res = drone.Visit(x, y);

                    result += res;

                    if (res > 1) throw new Exception("Unexpected output");

                    //Console.Write( res> 0 ? "##" : "  ");
                }
                //Console.WriteLine();
            }

            return result;
        }

        static bool BoxFit(TestDrone drone, int x, int y, int boxSize)
        {
            int size = boxSize - 1;

            return drone.Visit(x + size, y) +
                   drone.Visit(x, y + size) == 2;
        }

        public static int Part2(string input)
        {
            const int boxSize = 100;

            ManhattanVector2 topPos = new(0, 0);
            ManhattanVector2 bottomPos = new(0, 0);

            var drone = new TestDrone(input);


            // start out a bit, since the intial beam is gappy
            int x = boxSize; int y = 0;
            while (drone.Visit(x, y) == 0)
            {
                y++;
            }
            topPos.Set(x, y);
            bottomPos.Set(x, y);

            while (true)
            {
                // track the top of the beam
                while (true)
                {
                    if (drone.Visit(topPos.X + 1, topPos.Y) > 0)
                    {
                        topPos.X++;
                    }
                    else if (drone.Visit(topPos.X, topPos.Y - 1) > 0)
                    {
                        topPos.Y--;
                    }
                    else
                    {
                        break;
                    }
                }

                // track the bottom of the beam
                bottomPos.X = topPos.X;
                while (drone.Visit(bottomPos.X, bottomPos.Y + 1) == 1)
                {
                    bottomPos.Y++;
                }

                var verticalBeamSize = bottomPos.Y - topPos.Y + 1;

                //Console.WriteLine($"Vertical Beam size at {searchPos} is {verticalBeamSize}");
                //drone.DrawDroneView(searchPos.X, searchPos.Y);

                if (verticalBeamSize >= boxSize)
                {
                    if (BoxFit(drone, topPos.X, bottomPos.Y + 1 - boxSize, boxSize))
                    {
                        //drone.DrawDroneView(searchPos.X, beamY-boxSize, boxSize+5, boxSize);

                        Console.WriteLine($"scanned {drone.Scans} locations");

                        return (topPos.X * 10000) + (bottomPos.Y + 1 - boxSize);
                    }
                }

                topPos.Y++;
            }
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}