using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Utils.Vectors;

namespace Advent.MMXVIII
{
    public class Day13 : IPuzzle
    {
        public string Name { get { return "2018-13";} }
 
        public class Train
        {
            public ManhattanVector2 position = new ManhattanVector2(0,0);
            public Direction2 direction = new Direction2(0,0);
            public int turn;
            public bool crash;


            public char GetTrainCh() 
            {
                if (crash) return 'X';
                
                return direction.AsChar();
            }
        }

        public class TrainSim
        {
            List<string> map = new List<string>();

            List<Train> trains = new List<Train>();

            public bool Debug {get;set;} = false;

            public bool StopOnCrash {get;set;} = true;

            void AddTrain(int x, int y, int dx, int dy)
            {
                trains.Add(new Train{position = new ManhattanVector2(x,y), direction = new Direction2(dx,dy),turn=0,crash=false});
            }

            public TrainSim(string data) : this(Util.Split(data)) {}
            
            public TrainSim(string[] tracks)
            {
                foreach (var line in tracks)
                {
                    string outLine = "";

                    for (var i=0; i<line.Length; ++i) {
                        var c = line[i];
                        switch(c) {
                            case '>':
                                AddTrain(i, map.Count, 1, 0);
                                c = '-';
                                break;
                            case '<':
                                AddTrain(i, map.Count, -1, 0);
                                c = '-';
                                break;
                            case '^':
                                AddTrain(i, map.Count, 0, -1);
                                c = '|';
                                break;
                            case 'v':
                                AddTrain(i, map.Count, 0, 1);
                                c = '|';
                                break;
                        }

                        outLine += c;
                    }

                    map.Add(outLine);
                }
            }

            public string Run()
            {
                bool running = true;
                string result = null;

                while (running)
                {

                    var blank = map.Select(x => x.ToCharArray().ToList()).ToList();
                    var turn = map.Select(x => x.ToCharArray().ToList()).ToList();

                    foreach (var t in trains)
                    {
                        turn[t.position.Y][t.position.X] = t.GetTrainCh();
                    }

                    for(var i=0; i<trains.Count; ++i)
                    {
                        var t = trains[i];

                        turn[t.position.Y][t.position.X] = blank[t.position.Y][t.position.X];

                        t.position.Offset(t.direction);

                        var newTrack = turn[t.position.Y][t.position.X];

                        switch (newTrack) 
                        {
                            case '\\':
                                t.direction.SetDirection(t.direction.DY, t.direction.DX);
                                break;
                            case '/':  
                                t.direction.SetDirection(-t.direction.DY, -t.direction.DX);        
                                break;

                            case '+':
                                switch (t.turn) {
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
                        turn[t.position.Y][t.position.X] = t.GetTrainCh();

                        foreach (var other in trains)
                        {
                            if (other != t)
                            {
                                if (other.position == t.position) 
                                {                          
                                    if (StopOnCrash) 
                                    {                      
                                        running = false;
                                    } 
                                    t.crash = true;
                                    other.crash = true;
                               
                                    result = "Crash at "+t.position.X+","+ t.position.Y;
                                    if (Debug) Console.WriteLine(result);                          
                                }
                            }
                        }
                    }

                    // sort trains from top to bottom
                    trains.Sort((a, b) => 
                    {
                        if (a.position.Y != b.position.Y) return a.position.Y-b.position.Y;
                        return a.position.X-b.position.X;
                    });



                    // if (Debug)
                    // {
                    //     Console.WriteLine(trains.Where(t => t.crash).Count()+ " crashed");

                    //     foreach (var line in turn)
                    //     {
                    //         Console.WriteLine(string.Join("", line));
                    //     }
                    //     Console.WriteLine();
                    // }

                        // remove crashed trains
                    
                        trains = trains.Where(t => !t.crash).ToList();

                    if (running) 
                    {
                        if (trains.Count < 2)
                        {
                            result = "Last train at "+trains.First().position;
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
            var t2 = new TrainSim(input);
            t2.StopOnCrash=false;
            return t2.Run();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
