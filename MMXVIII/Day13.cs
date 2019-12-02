using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day13 : IPuzzle
    {
        public string Name { get { return "2018-13";} }
 
        public class Train
        {
            public int x;
            public int y;
            public int dx;
            public int dy;
            public int turn;
            public bool crash;

            public void SetDirection(int dirx, int diry)
            {
                dx = dirx;
                dy = diry;
            }

            public void TurnLeft()
            {
                // up :  0,-1 ->  -1,0;
                // left: -1,0 -> 0,1
                // down: 0,1 -> 1,0
                // right: 1,0 -> 0,-1

                if (dx==0 && dy == -1) SetDirection(-1,0);
                else if (dx==-1 && dy==0) SetDirection(0,1);
                else if (dx==0 && dy==1) SetDirection(1,0);
                else if (dx==1 && dy==0) SetDirection(0,-1);

                else throw new Exception("Unrecognised train direction: "+dx+","+dy);
            }

            public void TurnRight()
            {
                // up :  0,-1 ->  1,0;
                // right: 1,0 -> 0,1
                // down: 0,1 -> -1,0
                // left: -1,0 -> 0,-1

                if (dx==0 && dy == -1) SetDirection(1,0);
                else if (dx==1 && dy==0) SetDirection(0,1);
                else if (dx==0 && dy==1) SetDirection(-1,0);
                else if (dx==-1 && dy==0) SetDirection(0,-1);

                else throw new Exception("Unrecognised train direction :"+dx+","+dy);
            }

            public char GetTrainCh() 
            {
                if (crash) return 'X';
                if (dx > 0) return '>';
                if (dx < 0) return '<';
                if (dy < 0) return '^';
                if (dy > 0) return 'v';

                throw new Exception("Unknown train state");
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
                trains.Add(new Train{x=x,y=y,dx=dx,dy=dy,turn=0,crash=false});
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
                        turn[t.y][t.x] = t.GetTrainCh();
                    }

                    for(var i=0; i<trains.Count; ++i)
                    {
                        var t = trains[i];

                        turn[t.y][t.x] = blank[t.y][t.x];

                        t.x += t.dx;
                        t.y += t.dy;

                        var newTrack = turn[t.y][t.x];

                        switch (newTrack) 
                        {
                            case '\\':
                                t.SetDirection(t.dy, t.dx);
                                break;
                            case '/':  
                                t.SetDirection(-t.dy, -t.dx);        
                                break;

                            case '+':
                                switch (t.turn) {
                                    case 0: // left
                                        t.TurnLeft();
                                        t.turn = 1;
                                        break;

                                    case 1: // straight;
                                        t.turn = 2;
                                        break;

                                    case 2: // right
                                        t.TurnRight();
                                        t.turn = 0;
                                        break;
                                }
                                break;      
                        }
                        turn[t.y][t.x] = t.GetTrainCh();

                        foreach (var other in trains)
                        {
                            if (other != t)
                            {
                                if (other.x == t.x && other.y == t.y) 
                                {                          
                                    if (StopOnCrash) 
                                    {                      
                                        running = false;
                                    } 
                                    t.crash = true;
                                    other.crash = true;
                               
                                    result = "Crash at "+t.x+","+ t.y;
                                    if (Debug) Console.WriteLine(result);                          
                                }
                            }
                        }
                    }

                    // sort trains from top to bottom
                    trains.Sort((a, b) => 
                    {
                        if (a.y != b.y) return a.y-b.y;
                        return a.x-b.x;
                    });



                    if (Debug)
                    {
                        Console.WriteLine(trains.Where(t => t.crash).Count()+ " crashed");

                        foreach (var line in turn)
                        {
                            Console.WriteLine(string.Join("", line));
                        }
                        Console.WriteLine();
                    }

                        // remove crashed trains
                    
                        trains = trains.Where(t => !t.crash).ToList();

                    if (running) 
                    {
                        if (trains.Count < 2)
                        {
                            result = "Last train at "+trains.First().x+","+trains.First().y;
                            running = false;
                        }
                    }

                }
                return result;
            }
        }

        public void Run(string input)
        {
            var t = new TrainSim(input);

            //var t = new TrainSim(@"/->-\        ,|   |  /----\,| /-+--+-\  |,| | |  | v  |,\-+-/  \-+--/,  \------/   ");
            Console.WriteLine("- Pt1 - "+t.Run());

            var t2 = new TrainSim(input);
            t2.StopOnCrash=false;
            Console.WriteLine("- Pt2 - "+t2.Run());

            // var t3 = new TrainSim(@"/>-<\  ,|   |  ,| /<+-\,| | | v,\>+</ |,  |   ^,  \<->/");
            // t3.StopOnCrash=false;
            // //t3.Debug = true;
            // Console.WriteLine("- Pt2 - "+t3.Run());
        }
    }
}
