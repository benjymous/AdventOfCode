namespace AoC.Advent2023;
public class Day12 : IPuzzle
{
    public static IEnumerable<int> CountSprings(bool[] input) => Memoize(input, _ =>
    {
        List<int> result = new List<int>();
        int i = 0;
        for (int n = 0; n < input.Length; n++)
        {
            if (input[n]) { i++; }
            else
            {
                if (i > 0) result.Add(i);
                i = 0;
            }
        }
        if (i > 0) result.Add(i);
        return result;
    });

    [method: Regex(@"(.+) (.+)")]
    public record class Row(string Input, int[] Lengths);

    static T[] Unfold<T>(IEnumerable<T> input)
    {
        List<T> output = [];
        for (int i = 0; i < 5; ++i)
        {
            output.AddRange(input);
        }
        return [.. output];
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

    //private static int Solve1Single(Row row)
    //{
    //    int count = 0;
    //    var perms = GetPermutations(row.Input.ToArray()).Select(p => p.Select(c => c == '#').ToArray());

    //    foreach (var p in perms)
    //    {
    //        var springCount = CountSprings(p);
    //        if (springCount.SequenceEqual(row.Lengths)) count++;
    //    }

    //    return count;
    //}

    public static int Solve1Single(Row row)
    {
        return SolveR(row.Input.ToArray(), row.Lengths);
    }

    static object l = new();
    static int rowCount = 0;

    private static char Get(char[] input, int i)
    {
        if (i < input.Length) return input[i];
        return '.';
    }

    private static int SolveR(char[] input, int[] lengths, int currentCount = 0, int recurse=0, string prev="")
    {
        Console.WriteLine("".PadLeft(recurse,'-')+ " '" + prev + "' .. " + input.AsString() + " " + string.Join(",", lengths) + " " + currentCount);

        if (input.Length==0 && lengths.Length==0)
        {
            Console.WriteLine("".PadLeft(recurse, '-') + " " + prev + " <<<");
            return 1;
        }

        int res = 0;
        int i = 0;
        int count = currentCount;

        string p = prev;

        do
        {

            if (i > input.Length || lengths.Length == 0)
            {
                if (lengths.Length == 0)
                {
                    Console.WriteLine("".PadLeft(recurse, '-') + " " + p + " <<<");
                }

                return lengths.Length == 0 ? 1 : 0;
            }

            while (Get(input, i) == '#')
            {
                p += '#';
                i++; count++;
            }

            if (count > lengths[0]) return 0;

            if (Get(input, i) == '.' && count > 0)
            {
                // end of block, compare against lengths
                if (count == lengths[0])
                {
                    lengths = lengths[1..];
                }
                else
                {
                    return 0;
                }
                count = 0;
            }

            while (i < input.Length && Get(input, i) == '.')
            {
                p += '.';
                i++;
            }

        } while (Get(input, i) != '?');

        if (Get(input, i)=='?')
        {
            res += SolveR(['#', ..input.Skip(i+1)], lengths, count, recurse+1, p);

            if (count == lengths[0])
            {
                lengths = lengths[1..];
                count = 0;
            }

            res += SolveR([.. input.Skip(i+1)], lengths, count, recurse+1, p+".");
        }

        return res;
    }

    private static int Solve2Single(Row row, ILogger logger)
    {
        //var perms = GetPermutations([.. row.Input]).Select(p => p.Select(c => c == '#').ToArray()).ToList();

        int[] lengths = Unfold(row.Lengths);
        char[] input = Unfold(row.Input);

        int res = SolveR(input, lengths);

        if (logger != null)
            lock (l)
            {
                logger.WriteLine($"{rowCount++} {row.Input} : {string.Join(", ", row.Lengths)} => {res}");
            }
        return res;
    }


    //private static int FindPermPerms(Row row, bool[] input, List<bool[]> perms, int[] lengths, int skip=0, int level = 0)      
    //{
    //    int result = 0;
    //    for (int i = 0; i < perms.Count; ++i)
    //    {

    //        result += row.Memoize((string.Join("", input), i, skip), _ =>
    //        {
    //            int subResult = 0;

    //            bool[] current = [.. input, .. perms[i]];

    //            var springCount = CountSprings(current).ToArray();
    //            if (springCount.Length <= lengths.Length)
    //            {
    //                if (level == 4)
    //                {
    //                    if (lengths.SequenceEqual(springCount))
   
    //                    {
    //                        subResult++;
    //                    }
    //                }
    //                else
    //                {
    //                    bool possible = true;
    //                    for (int l = skip; l < springCount.Length; ++l)
    //                    {
    //                        if (springCount[l] != lengths[l])
    //                        {
    //                            possible = false;
    //                            break;
    //                        }
    //                    }
    //                    if (possible)
    //                    {
    //                        //var trimmedInput = current.Skip(current.FindNthIndex(i => i, lengths.Take(springCount.Length - 1).Sum()) + 1);
    //                        //var subLengths = lengths.Skip(springCount.Length - 1).ToArray();

    //                        subResult += FindPermPerms(row, [.. current, true], perms, lengths, springCount.Length, level + 1);
    //                        subResult += FindPermPerms(row, [.. current, false], perms, lengths, springCount.Length, level + 1);
    //                    }
    //                }
    //            }
    //            //result += subResult;
    //            return subResult;
    //        });
            
    //    }
    //    return result;
    //}

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
