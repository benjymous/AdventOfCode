namespace AoC.Advent2019;
public class Day08 : IPuzzle
{
    readonly static int Width = 25, Height = 6;

    class Image(string data)
    {
        readonly int[][] groups = data.Trim().Select(ch => ch.AsDigit()).Chunk(Width * Height).ToArray();

        public int GetChecksum()
        {
            var lowestZeroes = groups.MinBy(g => g.Count(i => i == 0));
            return lowestZeroes.Count(i => i == 1) * lowestZeroes.Count(i => i == 2);
        }

        public override string ToString()
        {
            var pixelData = new List<int>();
            foreach (var layer in groups.Reverse())
                if (pixelData.Count == 0) pixelData.AddRange(layer);
                else
                    for (var i = 0; i < layer.Length; ++i)
                        if (layer[i] != 2)
                            pixelData[i] = layer[i];

            return string.Join('\n', pixelData.SelectMany(c => c == 1 ? "##" : "  ").Chunk(Width * 2).Select(line => line.AsString()));
        }
    }

    public static int Part1(string input) => new Image(input).GetChecksum();

    public static string Part2(string input, ILogger logger)
    {
        var image = new Image(input).ToString();
        logger.WriteLine("\n" + image);
        return Utils.OCR.TextReader.Read(image);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, logger));
    }
}