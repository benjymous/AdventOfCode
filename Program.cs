using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;

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
            var timings = new Dictionary<string, long>();

            foreach (var puzzle in puzzles)
            {             
                timings[puzzle.Name] = puzzle.TimeRun();            
            }

            Console.WriteLine();
            foreach (var kvp in timings)
            {
                Console.WriteLine($"{kvp.Key} - {kvp.Value}ms");
            }
        }
    }
}
