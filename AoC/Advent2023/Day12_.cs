namespace AoC.Advent2023;
public class Day12 : IPuzzle
{
    public static IEnumerable<int> CountSprings(bool[] input)
    {
        int i = 0;
        for (int n = 0; n < input.Length; n++)
        {
            if (input[n]) { i++; }
            else
            {
                if (i > 0) yield return i;
                i = 0;
            }
        }
        if (i > 0) yield return i;
    }

    [method: Regex(@"(.+) (.+)")]
    record class Row(string Input, int[] Lengths);

    static int[] Unfold(int[] rowLengths)
    {
        List<int> lengths = [];
        for (int i = 0; i < 5; ++i)
        {
            lengths.AddRange(rowLengths);
        }
        return [.. lengths];
    }

    static char[] ReplaceChar(char[] input, int index, char ch)
    {
        var ret = input.ToArray();
        ret[index] = ch;
        return ret;
    }

    public static List<char[]> GetPermutations(char[] input)
    {
        var blankIdx = input.IndexOf('?');
        if (blankIdx == -1) return [input];
        else
        {
            var results = new List<char[]>();
            results.AddRange(GetPermutations(ReplaceChar(input, blankIdx, '#')));
            results.AddRange(GetPermutations(ReplaceChar(input, blankIdx, '.')));

            return results;
        }
    }

    private static int Solve1(Row[] data)
    {
        int count = 0;

        foreach (var row in data)
        {
            count += Solve1Single(row);
        }

        return count;
    }

    private static int Solve1Single(Row row)
    {
        int count = 0;
        var perms = GetPermutations(row.Input.ToArray()).Select(p => p.Select(c => c == '#').ToArray());

        foreach (var p in perms)
        {
            var springCount = CountSprings(p);
            if (springCount.SequenceEqual(row.Lengths)) count++;
        }

        return count;
    }

    static object l = new();
    static int rowCount = 0;

    private static int Solve2Single(Row row, ILogger logger)
    {
        var perms = GetPermutations([.. row.Input]).Select(p => p.Select(c => c == '#').ToArray()).ToList();

        int[] lengths = Unfold(row.Lengths);

        int res = FindPermPerms([], perms, lengths);

        if (logger != null)
            lock (l)
            {
                logger.WriteLine($"{rowCount++} {row.Input} : {string.Join(", ", row.Lengths)} => {res} ({perms.Count})");
            }
        return res;
    }

    private static bool Equal(int[] v1, int[] v2)
    {
        if (v1.Length != v2.Length) return false;

        for (int i = 0; i < v1.Length; ++i)
            if (v1[i] != v2[i]) return false;

        return true;
    }

    private static int FindPermPerms(bool[] input, List<bool[]> perms, int[] lengths, int level = 0)
    {
        int result = 0;
        for (int i = 0; i < perms.Count; ++i)
        {
            bool[] current = [.. input, .. perms[i]];

            var springCount = CountSprings(current).ToArray();
            if (springCount.Length <= lengths.Length)
            {
                if (level == 4)
                {
                    if (lengths.SequenceEqual(springCount))
                    //if (Equal(springCount, lengths))
                    {
                        result++;
                    }
                }
                else
                {
                    bool possible = true;
                    for (int l = 0; l < springCount.Length; ++l)
                    {
                        if (springCount[l] != lengths[l])
                        {
                            possible = false;
                            break;
                        }
                    }
                    if (possible)
                    {
                        var trimmedInput = current.Skip(current.FindNthIndex(i => i, lengths.Take(springCount.Length - 1).Sum()) + 1);
                        var subLengths = lengths.Skip(springCount.Length - 1).ToArray();

                        result += FindPermPerms([.. trimmedInput, true], perms, subLengths, level + 1);
                        result += FindPermPerms([.. trimmedInput, false], perms, subLengths, level + 1);
                    }
                }
            }
        }
        return result;
    }

    public static int Part1(string input)
    {
        var data = Util.RegexParse<Row>(input).ToArray();

        return Solve1(data);
    }

    public static int Part2(string input, ILogger logger = null)
    {
        var data = Util.RegexParse<Row>(input).ToArray();

        Console.WriteLine($"Solving {data.Length}");
        return data.AsParallel().Sum(r => Solve2Single(r, logger));
    }

    public void Run(string input, ILogger logger)
    {

        //        // string test = @"???.### 1,1,3";

        //        string test = @"???.### 1,1,3
        //.??..??...?##. 1,1,3
        //?#?#?#?#?#?#?#? 1,3,1,6
        //????.#...#... 4,1,1
        //????.######..#####. 1,6,5
        //?###???????? 3,2,1";

        //        logger.WriteLine("- Pt2 - " + Part2(test));

        //logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, logger));
    }
}
