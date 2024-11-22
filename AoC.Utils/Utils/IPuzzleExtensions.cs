
namespace AoC.Utils
{
    public static class IPuzzleExtensions
    {
        public static string Name(this IPuzzle puzzle) => $"{puzzle.GetYear()}-{puzzle.GetDay()}";

        public static string GetYear(this IPuzzle puzzle) => PuzzleHelpers.GetYear(puzzle.GetType());

        public static string GetDay(this IPuzzle puzzle) => PuzzleHelpers.GetDay(puzzle.GetType());

        public static long TimeRun(this IPuzzle puzzle, ILogger logger)
        {
            var input = Util.GetInput(puzzle);

            var watch = new System.Diagnostics.Stopwatch();
            var thread = Thread.CurrentThread;
            thread.Priority = ThreadPriority.AboveNormal;
            watch.Start();

            logger.WriteLine(puzzle.Name());
            puzzle.Run(input, logger);
            thread.Priority = ThreadPriority.BelowNormal;
            return watch.ElapsedMilliseconds;
        }
    }

    public static class PuzzleHelpers
    {
        public static string[] Args { get; set; }

        public static bool ShouldRun(Type puzzleType)
        {
            var yearStr = GetYear(puzzleType);
            var dayStr = GetDay(puzzleType);
            var year = int.Parse(yearStr);
            var day = int.Parse(dayStr);

            if (year > DateTime.Now.Year || (year == DateTime.Now.Year && DateTime.Now.Month < 12) || (year == DateTime.Now.Year && DateTime.Now.Month == 12 && DateTime.Now.Day < day))
                return false;

            if (Args.Length == 0) return true;

            var nameA = $"{yearStr}-{dayStr}";
            var nameB = $"Advent{yearStr}-Day{dayStr}";

            foreach (var line in Args.Select(l => l.Trim()))
            {
                if (nameA.Contains(line)) return true;
                if (line.StartsWith(nameB)) return true;
            }

            return false;
        }

        public static string GetYear(Type puzzleType) => puzzleType.Namespace[^4..];
        public static string GetDay(Type puzzleType) => puzzleType.Name[^2..];

        public static string DataURL(Type puzzleType) => $"https://adventofcode.com/{GetYear(puzzleType)}/day/{GetDay(puzzleType).TrimStart('0')}/input";

        static readonly Lock DataLock = new();
        public static string GetInput(Type puzzleType)
        {
            lock (DataLock)
            {
                var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AoCData", GetYear(puzzleType));
                if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
                var expectedFile = Path.Combine(dataPath, $"{GetYear(puzzleType)}-{GetDay(puzzleType)}.txt");

                if (!File.Exists(expectedFile))
                {
                    string puzzleData = Util.Download(DataURL(puzzleType)).Replace("\r", "");
                    File.WriteAllText(expectedFile, puzzleData);
                    return puzzleData;
                }

                return File.ReadAllText(expectedFile).Replace("\r", "");
            }
        }
    }
}