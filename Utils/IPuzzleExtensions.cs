namespace AoC.Utils
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

        public static long TimeRun(this IPuzzle puzzle, ILogger logger)
        {
            var input = Util.GetInput(puzzle);
            logger.WriteLine(puzzle.Name);
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            puzzle.Run(input, logger);
            return watch.ElapsedMilliseconds;
        }

    }


}