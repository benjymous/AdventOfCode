using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day08 : IPuzzle
    {
        public string Name => "2019-08";

        class Image
        {
            public Image(string data, int width, int height)
            {
                (Width, Height) = (width, height);

                groups = data.Trim().Select(ch => ch - '0').ToArray().Chunk(width * height).ToArray();
            }

            private void Decode()
            {
                pixelData = new();
                foreach (var layer in groups.Reverse())
                {
                    if (pixelData.Count == 0)
                    {
                        pixelData.AddRange(layer);
                    }
                    else
                    {
                        for (var i = 0; i < layer.Length; ++i)
                        {
                            if (layer[i] != 2)
                            {
                                pixelData[i] = layer[i];
                            }
                        }
                    }
                }
            }

            public readonly int Width, Height;

            readonly int[][] groups;
            List<int> pixelData = null;

            public int GetChecksum()
            {
                int leastZeros = int.MaxValue;
                int onesByTwos = 0;

                foreach (var g in groups)
                {
                    var zeroes = g.Count(i => i == 0);

                    if (zeroes < leastZeros)
                    {
                        leastZeros = zeroes;
                        onesByTwos = g.Count(i => i == 1) * g.Count(i => i == 2);
                    }
                }

                return onesByTwos;
            }

            public override string ToString()
            {
                if (pixelData == null) Decode();

                return string.Join('\n',pixelData.SelectMany(c => c == 1 ? "##" : "  ").Chunk(Width*2).Select(line => line.AsString()));
            }
        }

        public static int Part1(string input)
        {
            return new Image(input, 25, 6).GetChecksum();
        }

        public static string Part2(string input, ILogger logger)
        {
            var image = new Image(input, 25, 6).ToString();
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