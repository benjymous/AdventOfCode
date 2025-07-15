namespace AoC.Advent2024;
public class Day09 : IPuzzle
{
    private class Filesystem
    {
        public Filesystem(string input)
        {
            Queue<int> buffer = [.. input.Replace("\n", "").Select(c => c.AsDigit())];

            int blockCount = 0;

            while (buffer.TryDequeue(out int numBlocks))
            {
                Files.Add([.. Enumerable.Range(blockCount, numBlocks)]);
                blockCount += numBlocks;

                if (buffer.TryDequeue(out int numSpaces))
                {
                    FreeBlocks.AddRange(Enumerable.Range(blockCount, numSpaces));
                    blockCount += numSpaces;
                }
            }
        }

        private int GetContiguousSpace(int space)
        {
            for (int i = 0; i < FreeBlocks.Count - space; ++i)
            {
                if (FreeBlocks[i + space - 1] == FreeBlocks[i] + space - 1)
                {
                    int block = FreeBlocks[i];
                    FreeBlocks.RemoveRange(i, space);
                    return block;
                }
            }
            return int.MaxValue;
        }

        private bool MoveFile(List<int> file)
        {
            foreach (var from in file.ToArray().Reverse())
            {
                var to = FreeBlocks.First();
                if (to >= from) return false;
                FreeBlocks.Remove(to);
                file.Remove(from);
                file.Add(to);
            }
            return true;
        }

        private bool MoveFileContiguous(List<int> file)
        {
            int sizeNeeded = file.Count;
            var destination = GetContiguousSpace(sizeNeeded);
            if (destination < file[0])
            {
                file.Clear();
                file.AddRange(Enumerable.Range(destination, sizeNeeded));
            }
            return true;
        }

        public long Defrag(QuestionPart part)
        {
            foreach (var blocks in ((IEnumerable<List<int>>)Files).Reverse())
            {
                if (!(part.One ? MoveFile(blocks) : MoveFileContiguous(blocks))) break;
            }
            return Files.Index().Sum(f => ((long)f.Index) * f.Item.Sum());
        }

        private readonly List<List<int>> Files = [];
        private readonly List<int> FreeBlocks = [];
    }

    public static long PerformDefrag(string input, QuestionPart part) => new Filesystem(input).Defrag(part);

    public static long Part1(string input) => PerformDefrag(input, QuestionPart.Part1);
    public static long Part2(string input) => PerformDefrag(input, QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}