namespace AoC.Advent2023;
public class Day06 : IPuzzle
{
    static IEnumerable<(T time, T recordDistance)> Parse<T>(string input) where T : IBinaryInteger<T>
        => Util.Split(input).Select(Util.ExtractNumbers<T>).ZipTwo();

    public static bool WillBeat<T>(T charge, (T maxTime, T record) entry) where T : IBinaryInteger<T>
        => (entry.maxTime - charge) * charge > entry.record;

    public static T Solve<T>((T maxTime, T recordDistance) input) where T : IBinaryInteger<T>
    {
        var midPoint = input.maxTime >> 1; // divide by 2

        var lowerBound = Util.BinarySearch(T.Zero, midPoint, charge => WillBeat(charge, input));
        var upperBound = Util.BinarySearch(midPoint, input.maxTime, charge => !WillBeat(charge, input));

        return upperBound - lowerBound;
    }

    public static long Part1(string input) => Parse<int>(input).Select(Solve).Product();

    public static long Part2(string input) => Solve(Parse<long>(input.Replace(" ", "")).Single());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}