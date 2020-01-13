using System;
using System.Linq;

namespace Advent.Utils
{
    public static class IPuzzleExtensions
    {
        public static string[] args;

        public static bool ShouldRun(this IPuzzle puzzle)
        {
            if (args.Length == 0) return true;

            foreach (var line in args)
            {
                if (puzzle.Name.Contains(line.Trim())) return true;
            }

            return false;
        }

        public static long TimeRun(this IPuzzle puzzle,  ILogger logger)
        {
            var watch = new System.Diagnostics.Stopwatch();        
            watch.Start();
            logger.WriteLine(puzzle.Name);
            var input = Util.GetInput(puzzle);
            puzzle.Run(input, logger);
            return watch.ElapsedMilliseconds;
        }

    }


}