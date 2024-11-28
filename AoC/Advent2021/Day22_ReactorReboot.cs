namespace AoC.Advent2021;
using Box = (int MinX, int MaxX, int MinY, int MaxY, int MinZ, int MaxZ);
public class Day22 : IPuzzle
{
    public enum Toggle
    {
        on,
        off,
    }

    [method: Regex(@"(.+) x=(-?\d+)..(-?\d+),y=(-?\d+)..(-?\d+),z=(-?\d+)..(-?\d+)")]
    public class Instruction(Toggle action, int x1, int x2, int y1, int y2, int z1, int z2)
    {
        public Toggle Action = action;
        public Box Box = (x1, x2 + 1, y1, y2 + 1, z1, z2 + 1);
    }

    static int[] GetDivisions(params int[] values) => [.. values.Distinct().Order()];

    static IEnumerable<Box> Subtract(Box b1, Box b2)
    {
        var divX = GetDivisions(b1.MinX, b1.MaxX, b2.MinX, b2.MaxX);
        var divY = GetDivisions(b1.MinY, b1.MaxY, b2.MinY, b2.MaxY);
        var divZ = GetDivisions(b1.MinZ, b1.MaxZ, b2.MinZ, b2.MaxZ);

        for (int i = 0; i < divX.Length - 1; ++i)
            for (int j = 0; j < divY.Length - 1; ++j)
                for (int k = 0; k < divZ.Length - 1; ++k)
                {
                    var newBox = (divX[i], divX[i + 1], divY[j], divY[j + 1], divZ[k], divZ[k + 1]);
                    if (Overlaps(newBox, b1) && !Overlaps(newBox, b2)) yield return newBox;
                }
    }

    static bool Overlaps(Box box1, Box box2) => (box1.MinX < box2.MaxX) && (box1.MaxX > box2.MinX) && (box1.MinY < box2.MaxY) && (box1.MaxY > box2.MinY) && (box1.MinZ < box2.MaxZ) && (box1.MaxZ > box2.MinZ);

    static long Volume(Box box) => (box.MaxX - (long)box.MinX) * (box.MaxY - (long)box.MinY) * (box.MaxZ - (long)box.MinZ);

    public static long RunOperation(IEnumerable<Instruction> instructions)
    {
        var data = new List<Box>();

        foreach (var instr in instructions)
        {
            var newBoxes = new List<Box>(data.Count + 100);
            if (instr.Action == Toggle.on) newBoxes.Add(instr.Box);
            foreach (var box in data)
            {
                if (Overlaps(box, instr.Box)) newBoxes.AddRange(Subtract(box, instr.Box));
                else newBoxes.Add(box);
            }
            data = newBoxes;
        }
        return data.Sum(Volume);
    }

    public static long Part1(Parser.AutoArray<Instruction> lines) => RunOperation(lines.Take(20));

    public static long Part2(Parser.AutoArray<Instruction> lines) => RunOperation(lines);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}