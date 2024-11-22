namespace AoC.Advent2016;
public class Day08 : IPuzzle
{
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

        [Regex(@"rect (\d+)x(\d+)")]
        public Action RectInstr(int width, int height) => () =>
        {
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x)
                    pixels[x, y] = true;
        };

        [Regex(@"rotate row y=(\d+) by (\d+)")]
        public Action RotRowInstr(int row, int shift) => () =>
        {
            var oldRow = pixels.Row(row).ToArray();
            for (int x = 0; x < GridWidth; ++x)
                pixels[x, row] = oldRow[(GridWidth + (x - shift)) % GridWidth];
        };

        [Regex(@"rotate column x=(\d+) by (\d+)")]
        public Action RotColInstr(int col, int shift) => () =>
        {
            var oldCol = pixels.Column(col).ToArray();
            for (int y = 0; y < GridHeight; ++y)
                pixels[col, y] = oldCol[(GridHeight + (y - shift)) % GridHeight];
        };

        public void Perform(string input) => Util.RegexFactory<Action, Display>(input, this).ForEach(a => a());

        public int NumPixelsOn() => pixels.Values().Count(x => x);

        public override string ToString() => pixels.AsString(v => v ? "##" : "  ");
    }

    public static int Part1(string input) => new Display(input, 50, 6).NumPixelsOn();

    public static string Part2(string input, ILogger logger = null)
    {
        var display = new Display(input, 50, 6).ToString();
        logger?.WriteLine("\n\n" + display);
        return display;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - \n" + Part2(input));
    }
}