namespace AoC.Advent2023;
public class Day15 : IPuzzle
{
    public static int CalculateHASH(string collection) => Memoize(collection, _ => collection.Aggregate(0, (hash, v) => (hash + v) * 17 % 256));

    public class Factory
    {
        public readonly LensBox[] Boxes = Enumerable.Range(0, 256).Select(i => new LensBox(i)).ToArray();
        LensBox Box(string label) => Boxes[CalculateHASH(label)];

        [Regex(@"([a-z]+)=(\d+)")]
        public void AddLens(string label, int focalLength) => Box(label).AddLens((label, focalLength));

        [Regex(@"([a-z]+)-")]
        public void RemoveLens(string label) => Box(label).RemoveLens(label);
    }

    public record class LensBox(int BoxIndex)
    {
        public readonly List<(string Label, int FocalLength)> Lenses = [];

        public void AddLens((string Label, int FocalLength) lens)
        {
            var existing = Lenses.FirstOrDefault(l => l.Label == lens.Label);

            if (existing != default) Lenses[Lenses.IndexOf(existing)] = lens;
            else Lenses.Add(lens);
        }

        public void RemoveLens(string label) => Lenses.Remove(Lenses.FirstOrDefault(l => l.Label == label));

        public int GetPower() => Lenses.WithIndex().Sum(entry => (BoxIndex + 1) * (entry.Index + 1) * entry.Value.FocalLength);
    }

    public static int Part1(string input) => Util.Split(input).Sum(CalculateHASH);

    public static int Part2(string input)
        => Util.RegexFactory<Factory>(Util.Split(input)).Boxes.Sum(b => b.GetPower());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}