namespace AoC.Utils
{
    public static class IPuzzleExtensions
    {
        public static string[] args;

        public static string DataURL(this IPuzzle puzzle)
        {
            var bits = puzzle.Name.Split('-');
            return $"https://adventofcode.com/{bits[0]}/day/{bits[1].TrimStart('0')}/input";
        }

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

            var watch = new System.Diagnostics.Stopwatch();
            var thread = System.Threading.Thread.CurrentThread;
            thread.Priority = System.Threading.ThreadPriority.AboveNormal;
            watch.Start();

            logger.WriteLine(puzzle.Name);
            puzzle.Run(input, logger);
            thread.Priority = System.Threading.ThreadPriority.BelowNormal;
            return watch.ElapsedMilliseconds;
        }

    }


}