using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime;

namespace Advent
{
    class Program
    {
        static string[] _args;

        static bool ShouldRun(IPuzzle puzzle)
        {
            if (_args.Length == 0) return true;

            foreach (var line in _args)
            {
                if (line.Contains(puzzle.Name)) return true;
            }

            return false;
        }

        static List<IPuzzle> GetPuzzles()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IPuzzle).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (IPuzzle)Activator.CreateInstance(x)).OrderBy(x => x.Name).ToList();
        }

        static void Main(string[] args)
        {
            _args = args;

            var puzzles = GetPuzzles();

            foreach (var puzzle in puzzles)
            {
                if (ShouldRun(puzzle))
                {
                    string input = System.IO.File.ReadAllText(System.IO.Path.Combine("Data",puzzle.Name+".txt"));
                    puzzle.Run(input);
                }
            }
        }
    }
}
