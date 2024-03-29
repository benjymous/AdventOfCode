﻿namespace AoC.Advent2018;
public class Day13 : IPuzzle
{
    public class Train()
    {
        public Direction2 direction = new(0, 0);
        public int turn = 0;
    }

    public class TrainSim
    {
        readonly Dictionary<(int x, int y), Train> trains;
        readonly Dictionary<(int x, int y), char> map;

        public bool StopOnCrash { get; set; } = true;

        public TrainSim(string data)
        {
            map = Util.ParseSparseMatrix<char>(data, new Util.Convertomatic.SkipChars(' '));
            var trainChars = "^v<>".ToHashSet();
            trains = map.Where(kvp => trainChars.Contains(kvp.Value)).Select(kvp => (kvp.Key, new Train { direction = new Direction2(kvp.Value) })).ToDictionary();
        }

        public string Run()
        {
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
    }

    public static string Part1(string input)
    {
        var t = new TrainSim(input);
        return t.Run();
    }

    public static string Part2(string input)
    {
        var t2 = new TrainSim(input)
        {
            StopOnCrash = false
        };
        return t2.Run();
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
