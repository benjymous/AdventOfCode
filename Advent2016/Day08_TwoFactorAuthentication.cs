using AoC.Utils;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day08 : IPuzzle
    {
        public string Name => "2016-08";

        public class Display
        {
            readonly int GridWidth, GridHeight;
            readonly bool[,] pixels;

            public Display(string input, int w, int h)
            {
                (GridWidth, GridHeight) = (w, h);

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
                            var nums = Util.ParseNumbers<int>(bits[1], 'x');
                            Set(nums[0], nums[1]);
                            break;

                        case "rotate":
                            switch (bits[1])
                            {
                                case "row":
                                    RotateRow(int.Parse(bits[2].Replace("y=", "")), int.Parse(bits[4]));
                                    break;

                                case "column":
                                    RotateCol(int.Parse(bits[2].Replace("x=", "")), int.Parse(bits[4]));
                                    break;
                            }
                            break;
                    }
                }
            }

            void Set(int width, int height)
            {
                for (int y = 0; y < height; ++y)
                    for (int x = 0; x < width; ++x)
                        pixels[x, y] = true;
            }

            void RotateRow(int row, int shift)
            {
                var oldRow = pixels.Row(row).ToArray();
                for (int x = 0; x < GridWidth; ++x)
                    pixels[x, row] = oldRow[(GridWidth + (x - shift)) % GridWidth];
            }

            void RotateCol(int col, int shift)
            {
                var oldCol = pixels.Column(col).ToArray();
                for (int y = 0; y < GridHeight; ++y)
                    pixels[col, y] = oldCol[(GridHeight + (y - shift)) % GridHeight];
            }

            public int NumPixelsOn() => pixels.Values().Count(x => x);

            public override string ToString() => pixels.AsString(v => v ? "##" : "..");
        }

        public static int Part1(string input)
        {
            return new Display(input, 50, 6).NumPixelsOn();
        }

        public static string Part2(string input, ILogger logger = null)
        {
            var display = new Display(input, 50, 6).ToString();
            logger?.WriteLine("\n\n" + display);
            return display.GetMD5String(false);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - \n" + Part2(input));
        }
    }
}