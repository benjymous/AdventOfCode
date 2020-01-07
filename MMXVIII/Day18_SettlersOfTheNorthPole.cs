using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Advent.MMXVIII
{
    public class Day18 : IPuzzle
    {
        public string Name { get { return "2018-18";} }
 
        const char OPEN = '.';
        const char TREES = '|';
        const char LUMBERYARD = '#';

        public static char Step(char current, string neighbours)
        {
            switch(current)
            {
                case OPEN:
                    // An open acre will become filled with trees if three or more adjacent acres
                    // contained trees. 
                    // Otherwise, nothing happens.
                    if (neighbours.Where(n => n==TREES).Count() >=3) return TREES;
                    break;

                case TREES:
                    // An acre filled with trees will become a lumberyard if three or more adjacent
                    // acres were lumberyards. 
                    // Otherwise, nothing happens.
                    if (neighbours.Where(n => n==LUMBERYARD).Count() >=3) return LUMBERYARD;
                    break;

                case LUMBERYARD:
                    // An acre containing a lumberyard will remain a lumberyard if it was adjacent
                    // to at least one other lumberyard and at least one acre containing trees.
                    // Otherwise, it becomes open.
                    if (neighbours.Where(n => n==LUMBERYARD).Count() >=1 &&
                        neighbours.Where(n => n==TREES).Count() >=1) return LUMBERYARD;
                    else return OPEN;     
            }

            return current;
        }

        static int Count(char type, string[] state)
        {
            var all = String.Join("", state);
            return all.Where(c => c==type).Count();
        }

        static char GetAt(string[] input, int x, int y)
        {
            if (y<0 || y >= input.Length) return '-';
            if (x<0 || x >= input[y].Length) return '-';
            return input[y][x];
        }

        static void Display(string[] state)
        {
            foreach (var line in state) Console.WriteLine(line);
            Console.WriteLine();
        }

        public static int Run(string input, int iterations)
        {
            var currentState = Util.Split(input);
            var newState = (string[])currentState.Clone();

            var previous = new Queue<string>();
            int targetStep = -1;

            //Display(currentState);

            //var LOOKFOR = 190512;
            

            for (var i=0; i<iterations; ++i) 
            {
                var score = Count(TREES, currentState) * Count(LUMBERYARD, currentState); 

                // if (score == LOOKFOR)
                // {
                //     Console.WriteLine($"Answer at {i}");
                // }

                if (targetStep == i)
                {
                    return Count(TREES, currentState) * Count(LUMBERYARD, currentState); 
                } 

                var hash = String.Join("",currentState).GetSHA256String();
                
                var matchIdx = i-previous.Count;
                foreach (var prev in previous)
                {
                    matchIdx++;
                    if (prev == hash)
                    {
                        //Console.WriteLine($"Cycle detected between {matchIdx} and {i}");

                        var cycleLength = i-matchIdx+1;
                        //Console.WriteLine($"Cycle length = {cycleLength}");

                        //Console.WriteLine($"{i},{Count(TREES, currentState) * Count(LUMBERYARD, currentState)}");

                        if (targetStep == -1)
                        {
                            if (cycleLength == 1)
                            {
                                targetStep = i+1;
                            }
                            else
                            {
                                int cycleOffset = (i%cycleLength);
                                targetStep = i+((iterations-cycleOffset)%cycleLength);   
                            }                      
                        }
                        

                    }
                }

                previous.Enqueue(hash);
                if (previous.Count>50) previous.Dequeue();

                for (var y=0; y<currentState.Length; ++y)
                {
                    var line = "";
                    for (var x=0; x<currentState[y].Length; ++x)
                    {
                        var cell = GetAt(currentState, x, y);
                        string neighbours = "";

                        for (var y1=y-1; y1 <= y+1; ++y1)
                        {
                            for (var x1=x-1; x1 <= x+1; ++x1)
                            {
                                if (x!=x1 || y!=y1)
                                {
                                    neighbours += GetAt(currentState, x1, y1);
                                }
                            }
                        }

                        line += Step(cell, neighbours);
                    }
                    newState[y]=line;
                }
                currentState = (string[])newState.Clone();

                //Display(currentState);
                //if (i%1000 == 0) Console.WriteLine($"{i}/{iterations}");
            }

            return Count(TREES, currentState) * Count(LUMBERYARD, currentState);
        }

        public static int Part1(string input)
        {
            return Run(input, 10);
        }

        public static int Part2(string input)
        {
            return Run(input, 1000000000);
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(Step('|', "...|||.."));

            //Console.WriteLine(Run("......,......,..#|..,..#|..,..||..,......", 1000000));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
