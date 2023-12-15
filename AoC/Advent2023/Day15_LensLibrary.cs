namespace AoC.Advent2023;
public class Day15 : IPuzzle
{
    public static int CalculateHASH(IEnumerable<char> collection) => collection.Aggregate(0, (hash, v) => (hash + v) * 17 % 256);

    public record struct Instruction(string Label, bool Put, int FocalLength = 0)
    {
        [Regex(@"([a-z]+)=(\d+)")]
        public Instruction(string label, int focalLength) : this(label, true, focalLength) { }

        [Regex(@"([a-z]+)-")]
        public Instruction(string label) : this(label, false) { }

        public readonly int BoxNumber = CalculateHASH(Label);
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
    {
        var boxes = Enumerable.Range(0, 256).Select(i => new LensBox(i)).ToArray();

        foreach (var instr in Util.RegexParse<Instruction>(Util.Split(input)))
        {
            if (instr.Put)
                boxes[instr.BoxNumber].AddLens((instr.Label, instr.FocalLength));
            else
                boxes[instr.BoxNumber].RemoveLens(instr.Label);
        }

        return boxes.Sum(b => b.GetPower());
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}