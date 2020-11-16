using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day08 : IPuzzle
    {
        public string Name { get { return "2016-08"; } }

        public class Display
        {
            int GridWidth;
            int GridHeight;
            bool[,] pixels;

            public Display(string input, int w, int h)
            {
                GridWidth = w;
                GridHeight = h;

                pixels = new bool[GridWidth, GridHeight];

                Perform(input);
            }

            public void Perform(string input)
            {
                var instructions = Util.Split(input);

                foreach (var line in instructions)
                {
                    var bits = line.Split(" ");
                    switch (bits[0])
                    {
                        case "rect":
                            var rect = bits[1];
                            var size = rect.Split("x");
                            var width = int.Parse(size[0]);
                            var height = int.Parse(size[1]);
                            Set(width, height);
                            break;

                        case "rotate":
                            switch (bits[1])
                            {
                                case "row":
                                    var row = int.Parse(bits[2].Replace("y=", ""));
                                    var shiftrow = int.Parse(bits[4]);
                                    RotateRow(row, shiftrow);
                                    break;

                                case "column":
                                    var col = int.Parse(bits[2].Replace("x=", ""));
                                    var shiftcol = int.Parse(bits[4]);
                                    RotateCol(col, shiftcol);
                                    break;
                            }
                            break;

                        default:
                            Console.WriteLine($"Unknown command {line}");
                            break;

                    }
                }
            }

            void Set(int width, int height)
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        pixels[x, y] = true;
                    }
                }
            }

            void RotateRow(int row, int shift)
            {
                List<bool> changedRow = new List<bool>();
                for (int x = 0; x < GridWidth; ++x)
                {
                    int nx = (GridWidth + (x - shift)) % GridWidth;
                    changedRow.Add(pixels[nx, row]);
                }

                for (int x = 0; x < GridWidth; ++x)
                {
                    pixels[x, row] = changedRow[x];
                }
            }

            void RotateCol(int col, int shift)
            {
                List<bool> changedCol = new List<bool>();
                for (int y = 0; y < GridHeight; ++y)
                {
                    int ny = (GridHeight + (y - shift)) % GridHeight;
                    changedCol.Add(pixels[col, ny]);
                }

                for (int y = 0; y < GridHeight; ++y)
                {
                    pixels[col, y] = changedCol[y];
                }
            }

            public int NumPixelsOn() => pixels.Values().Where(x => x == true).Count();

            public override string ToString()
            {
                var sb = new StringBuilder();
                for (var y = 0; y < GridHeight; ++y)
                {
                    for (var x = 0; x < GridWidth; ++x)
                    {
                        sb.Append(pixels[x, y] ? "##" : "..");
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
        }

        public static int Part1(string input)
        {
            var display = new Display(input, 50, 6);
            return display.NumPixelsOn();
        }

        public static string Part2(string input)
        {
            var display = new Display(input, 50, 6);
            return display.ToString();
        }

        public void Run(string input, ILogger logger)
        {
            // var display = new Display("", 7, 3);
            // display.Perform("rect 3x2");
            // logger.WriteLine($"{display}\n\n");
            // display.Perform("rotate column x=1 by 1");
            // logger.WriteLine($"{display}\n\n");
            // display.Perform("rotate row y=0 by 4");
            // logger.WriteLine($"{display}\n\n");
            // display.Perform("rotate column x=1 by 1");
            // logger.WriteLine($"{display}\n\n");

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - \n" + Part2(input));
        }
    }
}