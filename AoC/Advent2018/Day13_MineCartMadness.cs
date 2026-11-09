namespace AoC.Advent2018;

public class Day13 : IPuzzle
{
    public class Train()
    {
        public Direction2 direction = new(0, 0);
        public int turn = 0;
    }

    public static string TrainSim(string data, bool StopOnCrash = true)
    {
        var map = Util.ParseSparseMatrix<char>(data, new Util.Convertomatic.SkipChars(' '));
        var trainChars = "^v<>".ToHashSet();
        var trains = map.Where(kvp => trainChars.Contains(kvp.Value)).Select(kvp => (kvp.Key, new Train { direction = new Direction2(kvp.Value) })).ToDictionary();

        while (true)
        {
            foreach (var currentPos in trains.Keys.OrderBy(pos => (pos.y, pos.x)).ToList())
            {
                if (!trains.TryGetValue(currentPos, out var t)) continue;

                trains.Remove(currentPos);

                var newPos = currentPos.OffsetBy(t.direction);

                if (trains.Remove(newPos))
                {
                    if (StopOnCrash) return $"Crash at {newPos}";
                }
                else
                {
                    trains[newPos] = t;
                    var m = map[newPos];

                    if (m is '\\' or '/')
                    {
                        t.direction.Mirror(m);
                    }
                    else if (m is '+')
                    {
                        t.direction.TurnRightBySteps(t.turn - 1);
                        t.turn = (t.turn + 1) % 3;
                    }
                }
            }

            if (trains.Count < 2) return $"Last train at {trains.First().Key}";
        }
    }

    public static string Part1(string input) => TrainSim(input);

    public static string Part2(string input) => TrainSim(input, false);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}