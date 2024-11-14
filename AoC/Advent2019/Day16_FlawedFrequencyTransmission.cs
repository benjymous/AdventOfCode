namespace AoC.Advent2019;
public class Day16 : IPuzzle
{
    static readonly int[] initialPattern = [0, 1, 0, -1];

    public static int SequenceAt(int iter, int pos) => initialPattern[(pos + 1) / (iter + 1) % 4];

    static IEnumerable<int> ApplyFFT(int[] input)
    {
        for (int i = 0; i < input.Length; ++i)
        {
            int outVal = 0;
            for (int j = i; j < input.Length; ++j)
            {
                outVal += input[j] * SequenceAt(i, j);
            }

            yield return Math.Abs(outVal) % 10;
        }
    }

    public static string ApplyRepeatedFFT(string initial, int repeats)
    {
        var current = initial.Trim().Select(ch => ch.AsDigit()).ToArray();
        for (var i = 0; i < repeats; ++i)
        {
            current = ApplyFFT(current).ToArray();
        }
        return current.Select(c => (char)('0' + c)).AsString();
    }

    // Thanks to FirescuOvidiu
    // All 1s in second half, past leading zeroes
    // based on algorithm here:
    // https://github.com/FirescuOvidiu/Advent-of-Code-2019/blob/master/Day%2016/day16-part2/day16-part2.cpp
    static int[] ProcessSignal(int[] sequence)
    {
        var newSequence = new int[sequence.Length];
        int sizeSequence = sequence.Length;
        int phase = 0;

        while (phase < 100)
        {
            int sum = 0;
            for (int position = sizeSequence - 1; position >= sizeSequence / 2; position--)
            {
                sum += sequence[position];
                newSequence[position] = sum % 10;
            }
            sequence = newSequence;
            phase++;
        }

        return sequence;
    }

    public static string Part1(string input) => ApplyRepeatedFFT(input, 100)[..8];

    public static string Part2(string input)
    {
        var initial = string.Join("", Enumerable.Repeat(input.Trim(), 10000)).Select(c => (int)c);

        var signal = ProcessSignal(initial.ToArray());

        int messageOffset = int.Parse(input[..7]);

        var outStr = signal.Skip(messageOffset).Take(8).Select(c => (char)('0' + c)).AsString();

        return outStr;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}