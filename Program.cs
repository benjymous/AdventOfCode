using Advent.Utils;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            if (puzzles.Count() == 1)
            {
                var timing = puzzles.First().TimeRun(new ConsoleOut());
                Console.WriteLine();
                Console.WriteLine($"{timing} ms");
            }
            else
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                ConcurrentDictionary<string, bool> running = new ConcurrentDictionary<string, bool>();

                Parallel.ForEach(puzzles, (puzzle) =>
                     {
                         running[puzzle.Name] = true;

                         mut.WaitOne();
                         Console.WriteLine($"{puzzle.Name} starting");
                         Console.WriteLine($"Running: [{string.Join(", ", running.Keys)}]");
                         mut.ReleaseMutex();

                         TextBuffer buffer = new TextBuffer();
                         timings[puzzle.Name] = puzzle.TimeRun(new TimeLogger(buffer));

                         
                         bool ignore;
                         running.TryRemove(puzzle.Name, out ignore);

                         mut.WaitOne();
                         Console.WriteLine();
                         Console.WriteLine(buffer);
                         ++finished;
                         Console.WriteLine();
                         Console.WriteLine($"Running: [{string.Join(", ", running.Keys)}]");
                         Console.WriteLine($"[{finished}/{total} {((finished) * 100 / total)}%]");
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
