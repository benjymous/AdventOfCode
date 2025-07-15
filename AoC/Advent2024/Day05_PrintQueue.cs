namespace AoC.Advent2024;
public class Day05 : IPuzzle
{
    public class UpdatePackage() : IComparer<int>
    {
        private readonly HashSet<(int Before, int After)> Rules = [];
        public readonly List<List<int>> ValidSets = [], InvalidSets = [];

        [Regex(@"(\d+)\|(\d+)")] public void Rule(int before, int after) => Rules.Add((before, after));
        [Regex(@"(.+)")] public void PageSet(List<int> pages) => (IsValid(pages) ? ValidSets : InvalidSets).Add(pages);

        public int Compare(int x, int y) => Rules.Contains((x, y)) ? -1 : 1;

        private bool IsValid(List<int> pages)
            => pages.OverlappingPairs().All(p => Compare(p.first, p.second) == -1);

        public static implicit operator UpdatePackage(string data) => Parser.Factory<UpdatePackage>(data);
    }

    public static int Part1(UpdatePackage package)
        => package.ValidSets.Sum(u => u[u.Count / 2]);

    public static int Part2(UpdatePackage package)
        => package.InvalidSets.Sum(u => { u.Sort(package); return u[u.Count / 2]; });

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}