﻿namespace AoC.Advent2015;
public class Day14 : IPuzzle
{
    [Regex(@".+ can fly (.+) km\/s for (.+) seconds, but then must rest for (.+) seconds\.")]
    public record struct Reindeer(int Speed, int Sprint, int Rest)
    {
        public readonly IEnumerable<int> Distance()
        {
            int total = 0;
            yield return total;
            while (true)
            {
                for (int fly = 0; fly < Sprint; ++fly)
                {
                    total += Speed;
                    yield return total;
                }
                for (int wait = 0; wait < Rest; ++wait)
                {
                    yield return total;
                }
            }
        }
    }

    static int MaxDistanceAfterTime(IEnumerable<Reindeer> deer, int seconds) => deer.Select(d => d.Distance().Skip(seconds).First()).Max();

    public static int MaxScoreAfterTime(IEnumerable<Reindeer> deer, int seconds)
    {
        var distances = deer.Select(d => d.Distance().Take(seconds).ToArray()).ToArray();

        Dictionary<int, int> scores = [];

        for (int timeIdx = 1; timeIdx < seconds; ++timeIdx)
        {
            int maxDistanceAtTime = distances.Max(v => v[timeIdx]);

            for (int deerIdx = 0; deerIdx < distances.Length; ++deerIdx)
            {
                if (distances[deerIdx][timeIdx] == maxDistanceAtTime)
                {
                    scores.IncrementAtIndex(deerIdx);
                }
            }
        }

        return scores.Values.Max();
    }

    public static int Part1(Parser.AutoArray<Reindeer> deer) => MaxDistanceAfterTime(deer, 2503);

    public static int Part2(Parser.AutoArray<Reindeer> deer) => MaxScoreAfterTime(deer, 2503);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}