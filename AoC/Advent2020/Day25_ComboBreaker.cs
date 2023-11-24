namespace AoC.Advent2020;
public class Day25 : IPuzzle
{
    const int MagicNumber = 20201227;

    public static IEnumerable<(int loop, long val)> LoopVals(int subject)
    {
        yield return (0, 0); // Keep things zero indexed

        int loop = 1;
        long val = 1;

        while (true) yield return (loop++, val = val * subject % MagicNumber);
    }

    private static int CalculatePrivateKey(int publicKey)
        => LoopVals(7)
           .First(v => v.val == publicKey)
           .loop;

    private static int CalculateEncryptionKey(int publicKey, int privateKey)
        => (int)LoopVals(publicKey)
           .ElementAt(privateKey)
           .val;

    public static int Part1(string input)
    {
        var (doorPublicKey, cardPublicKey) = Util.ParseNumbers<int>(input).Decompose2();

        var privateKey = CalculatePrivateKey(doorPublicKey);

        return CalculateEncryptionKey(cardPublicKey, privateKey);
    }

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}