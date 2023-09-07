using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;

namespace AoC.Advent2019
{
    public class Day11 : IPuzzle
    {
        public string Name => "2019-11";

        public class EmergencyHullPainterRobot : NPSA.ICPUInterrupt
        {
            readonly NPSA.IntCPU cpu;
            readonly ManhattanVector2 position = new(0, 0);
            readonly Dictionary<(int x, int y), bool> hullColours = new();
            readonly Direction2 direction = new(0, -1);

            int readState = 0;

            public EmergencyHullPainterRobot(string program)
            {
                cpu = new NPSA.IntCPU(program);
                cpu.Reserve(1200);
                cpu.Interrupt = this;
            }

            void Forwards() => position.Offset(direction);
            public void PaintHull(bool colour) => hullColours[position] = colour;
            bool ReadCamera() => hullColours.GetOrDefault(position);
            public int GetPaintedTileCount() => hullColours.Count;

            public void Run() => cpu.Run();

            public void RequestInput() => cpu.Input.Enqueue(ReadCamera() ? 1 : 0);

            public void OutputReady()
            {
                var data = cpu.Output.Dequeue();

                if (readState == 0)
                {
                    PaintHull(data == 1);
                }
                else
                {
                    if (data == 0) direction.TurnLeft();
                    else direction.TurnRight();
                    Forwards();
                }

                readState = (readState + 1) % 2;
            }

            public string GetDrawnPattern()
            {
                var outStr = "";

                var (minx, maxx) = hullColours.Keys.MinMax(v => v.x);
                var (miny, maxy) = hullColours.Keys.MinMax(v => v.y);

                for (var y = miny; y <= maxy; ++y)
                {
                    for (var x = minx; x <= maxx; ++x)
                    {
                        outStr += hullColours.GetOrDefault((x, y)) ? "##" : "  ";
                    }
                    outStr += "\n";
                }

                return outStr;
            }
        }

        public static int Part1(string input)
        {
            var robot = new EmergencyHullPainterRobot(input);
            robot.Run();
            return robot.GetPaintedTileCount();
        }

        public static string Part2(string input, ILogger logger)
        {
            var robot = new EmergencyHullPainterRobot(input);
            robot.PaintHull(true);
            robot.Run();
            var image = robot.GetDrawnPattern();
            logger.WriteLine("\n" + image);
            return image.GetMD5String();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}