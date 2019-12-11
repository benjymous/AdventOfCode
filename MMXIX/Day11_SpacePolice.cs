using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day11 : IPuzzle
    {
        public string Name { get { return "2019-11";} }
 
        public class EmergencyHullPainterRobot : NPSA.ICPUInterrupt
        {
            NPSA.IntCPU cpu;

            ManhattanVector2 position = new ManhattanVector2(0,0);

            Dictionary<string, int> hullColours = new Dictionary<string, int>();

            int dx = 0;
            int dy = -1;

            int minx = 0;
            int miny = 0;

            int maxx = 0;
            int maxy = 0;

            int readState = 0;

            public EmergencyHullPainterRobot(string program)
            {
                cpu = new NPSA.IntCPU(program);
                cpu.Interrupt = this;
            }

            void SetDirection(int dirx, int diry)
            {
                dx = dirx;
                dy = diry;
            }

            void TurnLeft()
            {
                // up :  0,-1 ->  -1,0;
                // left: -1,0 -> 0,1
                // down: 0,1 -> 1,0
                // right: 1,0 -> 0,-1

                if (dx==0 && dy == -1) SetDirection(-1,0);
                else if (dx==-1 && dy==0) SetDirection(0,1);
                else if (dx==0 && dy==1) SetDirection(1,0);
                else if (dx==1 && dy==0) SetDirection(0,-1);

                else throw new Exception("Unrecognised train direction: "+dx+","+dy);
            }

            void TurnRight()
            {
                // up :  0,-1 ->  1,0;
                // right: 1,0 -> 0,1
                // down: 0,1 -> -1,0
                // left: -1,0 -> 0,-1

                if (dx==0 && dy == -1) SetDirection(1,0);
                else if (dx==1 && dy==0) SetDirection(0,1);
                else if (dx==0 && dy==1) SetDirection(-1,0);
                else if (dx==-1 && dy==0) SetDirection(0,-1);

                else throw new Exception("Unrecognised train direction :"+dx+","+dy);
            }

            void Forwards()
            {
                position.Offset(dx, dy);

                minx = Math.Min(minx, position.X);
                miny = Math.Min(miny, position.Y);

                maxx = Math.Max(maxx, position.X);
                maxy = Math.Max(maxy, position.Y);
            }
          
            public void PaintHull(int colour)
            {
                if (colour < 0 || colour > 1) throw new Exception("Unexpected hull colour!");
                hullColours[position.ToString()] = colour;
            }
            int ReadCamera()
            {
                if (!hullColours.ContainsKey(position.ToString())) return 0;
                return hullColours[position.ToString()];
            }

            public int GetPaintedTileCount()
            {
                return hullColours.Count();
            }


            public void Run()
            {
                cpu.Run();
            }

            public void WillReadInput()
            {
                cpu.Input.Enqueue(ReadCamera());
            }

            public void HasPutOutput()
            {
                var data = cpu.Output.Dequeue();

                if (readState == 0)
                {
                    PaintHull((int)data);
                }
                else if (readState == 1)
                {
                    if (data == 0) TurnLeft();
                    else if (data == 1) TurnRight();
                    else {throw new Exception("Unexpected turn direction!");}
                    Forwards();
                }
                else
                {
                    throw new Exception("robot in wrong state!");
                }

                readState = (readState+1)%2;
            }

            public string GetDrawnPattern()
            {
                var outStr = "";

                for (var y=miny; y<=maxy; ++y)
                {
                    for (var x=minx; x<=maxx; ++x)
                    {
                        int val = 0;
                        var key = $"{x},{y}";
                        if (hullColours.ContainsKey(key))
                        {
                            val = hullColours[key];
                        }

                        if (val == 0) outStr +=" ";
                        else outStr +="#";

                    }
                    outStr+="\n";
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

        public static string Part2(string input)
        {
            var robot = new EmergencyHullPainterRobot(input);
            robot.PaintHull(1);
            robot.Run();
            return robot.GetDrawnPattern();
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2:\n"+Part2(input));
        }
    }
}