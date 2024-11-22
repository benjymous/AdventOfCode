namespace AoC.Advent2015;
public class Day02 : IPuzzle
{
    [Regex(@"(\d+x\d+x\d+)")]
    public class Box([Split("x")] int[] sides)
    {
        public int Wrap()
        {
            var (length, width, height) = sides.Decompose3();
            var side1 = length * width;
            var side2 = width * height;
            var side3 = height * length;

            var extra = Math.Min(Math.Min(side1, side2), side3);

            return extra + (2 * side1) + (2 * side2) + (2 * side3);
        }

        public int Ribbon()
        {
            var (length, width, height) = sides.Order().Decompose3();
            return (length * 2) + (width * 2) + (length * width * height);
        }
    }

    public static int Part1(string input) => Util.RegexParse<Box>(input).Sum(b => b.Wrap());

    public static int Part2(string input) => Util.RegexParse<Box>(input).Sum(b => b.Ribbon());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}