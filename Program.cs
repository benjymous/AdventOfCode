using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Advent
{
    class Program
    {
        static IEnumerable<IPuzzle> GetPuzzles()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IPuzzle).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (IPuzzle)Activator.CreateInstance(x))
                .Where(p => p.ShouldRun())
                .OrderBy(x => x.Name);
        }

        static void Main(string[] args)
        {
            Extensions.args = args;

            var puzzles = GetPuzzles();
            var timings = new ConcurrentDictionary<string, long>();

            Mutex mut = new Mutex();

            int total = puzzles.Count();
            int finished = 0;

            if (puzzles.Count()==1)
            {
                var timing = puzzles.First().TimeRun(Console.Out);
                Console.WriteLine();
                Console.WriteLine($"{timing} ms");
            }
            else
            {
                Parallel.ForEach(puzzles, new ParallelOptions(){MaxDegreeOfParallelism=1}, (puzzle) =>
                {
                    TextBuffer buffer = new TextBuffer();
                    timings[puzzle.Name] = puzzle.TimeRun(buffer);

                    mut.WaitOne();
                    Console.WriteLine(buffer);
                    ++finished;
                    Console.WriteLine($"[{finished}/{total} {((finished)*100/total)}%]");
                    Console.WriteLine();
                    mut.ReleaseMutex();
                });
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
