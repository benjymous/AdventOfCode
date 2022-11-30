using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day13 : IPuzzle
    {
        public string Name => "2018-13";

        public class Train
        {
            public ManhattanVector2 position = new(0, 0);
            public Direction2 direction = new(0, 0);
            public int turn;
            public bool crash;
        }

        public class TrainSim
        {     
            List<Train> trains = new();
            Dictionary<(int x, int y), char> map;

            public bool Debug { get; set; } = false;

            public bool StopOnCrash { get; set; } = true;

            void AddTrain(int x, int y, int dx, int dy)
            {
                trains.Add(new Train { position = new ManhattanVector2(x, y), direction = new Direction2(dx, dy), turn = 0, crash = false });
            }

            public TrainSim(string data) : this(Util.Split(data)) { }

            public TrainSim(string[] tracks)
            {
                List<string> rawMap = new();

                foreach (var line in tracks)
                {
                    string outLine = "";

                    for (var i = 0; i < line.Length; ++i)
                    {
                        var c = line[i];
                        switch (c)
                        {
                            case '>':
                                AddTrain(i, rawMap.Count, 1, 0);
                                c = '-';
                                break;
                            case '<':
                                AddTrain(i, rawMap.Count, -1, 0);
                                c = '-';
                                break;
                            case '^':
                                AddTrain(i, rawMap.Count, 0, -1);
                                c = '|';
                                break;
                            case 'v':
                                AddTrain(i, rawMap.Count, 0, 1);
                                c = '|';
                                break;
                        }

                        outLine += c;
                    }

                    rawMap.Add(outLine);
                }

                map = Util.ParseSparseMatrix<char>(rawMap);
            }

            public string Run()
            {
                bool running = true;
                string result = null;
               
                while (running)
                {
                    foreach (var t in trains)
                    {
                        t.position.Offset(t.direction);

                        var newTrack = map[t.position];

                        switch (newTrack)
                        {
                            case '\\':
                                t.direction.SetDirection(t.direction.DY, t.direction.DX);
                                break;
                            case '/':
                                t.direction.SetDirection(-t.direction.DY, -t.direction.DX);
                                break;

                            case '+':
                                switch (t.turn)
                                {
                                    case 0: // left
                                        t.direction.TurnLeft();
                                        t.turn = 1;
                                        break;

                                    case 1: // straight;
                                        t.turn = 2;
                                        break;

                                    case 2: // right
                                        t.direction.TurnRight();
                                        t.turn = 0;
                                        break;
                                }
                                break;
                        }

                        foreach (var other in trains.Where(o => o != t && o.position == t.position))
                        {
                            if (StopOnCrash)
                            {
                                running = false;
                            }
                            t.crash = true;
                            other.crash = true;

                            result = $"Crash at {t.position}";
                            if (Debug) Console.WriteLine(result);
                        }
                    }

                    if (running)
                    {
                        // remove crashed trains, and sort top to bottom
                        trains = trains.Where(t => !t.crash).OrderBy(t => (t.position.Y,t.position.X)).ToList();

                        if (trains.Count < 2)
                        {
                            result = $"Last train at {trains.First().position}";
                            running = false;
                        }
                    }

                }
                return result;
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
}
