﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Advent.Utils;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            IPuzzleExtensions.args = args;

            var puzzles = Util.GetPuzzles();
            var timings = new ConcurrentDictionary<string, long>();

            Mutex mut = new Mutex();

            int total = puzzles.Count();
            int finished = 0;

            if (puzzles.Count()==1)
            {
                var timing = puzzles.First().TimeRun(new ConsoleOut());
                Console.WriteLine();
                Console.WriteLine($"{timing} ms");
            }
            else
            {
                var watch = new System.Diagnostics.Stopwatch();        
                watch.Start();

                Parallel.ForEach(puzzles, new ParallelOptions(){MaxDegreeOfParallelism=8}, (puzzle) =>
                {
                    TextBuffer buffer = new TextBuffer();
                    timings[puzzle.Name] = puzzle.TimeRun(new TimeLogger(buffer));

                    mut.WaitOne();
                    Console.WriteLine(buffer);
                    ++finished;
                    Console.WriteLine($"[{finished}/{total} {((finished)*100/total)}%]");
                    Console.WriteLine();
                    mut.ReleaseMutex();
                });

                Console.WriteLine($"All completed in {watch.ElapsedMilliseconds} ms");
            }

            Console.WriteLine();
            foreach (var kvp in timings.OrderBy(kvp => kvp.Key))
            {
                Console.WriteLine($"{kvp.Key} - {kvp.Value}ms");
            }
            
            Console.WriteLine();
            foreach (var kvp in timings.OrderBy(kvp => kvp.Value))
            {
                Console.WriteLine($"{kvp.Key} - {kvp.Value}ms");
            }
        }
    }
}
