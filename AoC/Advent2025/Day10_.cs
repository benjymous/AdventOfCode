using System.Data;

namespace AoC.Advent2025;

public class Day10 : IPuzzle
{
    public class Machine
    {
        string key;
        bool[] TargetLights = [];
        int[] TargetJolts = [];
        readonly List<int[]> ButtonCombo = [];

        public Machine(string input)
        {
            key = input;
            Parser.Factory(input, this, " ");
        }

        static bool[] PressButtonsPart1(bool[] lights, int[] buttons)
        {
            var l = lights.ToArray();
            foreach (var b in buttons)
            {
                l[b] = !l[b];
            }
            return l;
        }

        static int[] PressButtonsPart2(int[] jolts, int[] buttons)
        {
            var j = jolts.ToArray();
            foreach (var b in buttons)
            {
                j[b]++;
            }
            return j;
        }

        public int SolvePart1()
        {
            bool[] initialLights = TargetLights.Select(v => false).ToArray();
            return Solver<(bool[] lights, int presses)>.Solve((initialLights, 0), (state, solver) =>
            {
                if (state.lights.SequenceEqual(TargetLights)) return state.presses;

                if (solver.CurrentBest != null && state.presses >= solver.CurrentBest) return default;

                if (solver.IsBetterThanSeen(state.lights, state.presses))
                {
                    foreach (var combo in ButtonCombo)
                    {
                        solver.Enqueue((PressButtonsPart1(state.lights, combo), state.presses + 1), state.presses + 1);
                    }
                }

                return default;

            }, Math.Min);
        }


        object Key(int[] jolts) => jolts.GetCombinedHashCode();


        public int FindPresses(int[] remainingjolts) => Memoize(Key(remainingjolts), _ =>
        {
            if (remainingjolts.Sum() == 0)
            {
                return 0;
            }

            int min = int.MaxValue;

            foreach (var rule in ButtonCombo.OrderBy(v => Random.Shared.Next()))
            {

                bool skip = false;
                int maxSteps = remainingjolts.Max();
                for (int i = 0; i < rule.Length; ++i)
                {
                    maxSteps = Math.Min(maxSteps, remainingjolts[rule[i]]);
                }

                if (maxSteps == 0) continue;

                

                int[] next = remainingjolts.ToArray();

                for (int i = 0; i < rule.Length; ++i)
                {
                    next[rule[i]] -= 1;
                    if (next[rule[i]] < 0)
                    {
                        skip = true;
                        break;
                    }
                }

                if (!skip)
                {
                    if (next.Sum() == 0) return 1;

                    int v = FindPresses(next);
                    if (v == int.MaxValue) continue;

                    min = Math.Min(min, v + 1);
                }
                

            }

            return min;
        }, "202510", key);


        public int SolvePart2() => FindPresses(TargetJolts);


        [Regex(@"\[(.+)\]")]
        public void ParseLights(string l)
        {
            TargetLights = l.Select(c => c == '#').ToArray();
        }

        [Regex(@"\((.+)\)")]
        public void ParseButtons(int[] buttons)
        {
            ButtonCombo.Add(buttons);
        }

        [Regex(@"\{(.+)\}")]
        public void ParseJolts(int[] jolts)
        {
            TargetJolts = jolts;
        }
    }


    public static int Part1(string input)
    {
        var rows = Util.Split(input, "\n").Select(r => new Machine(r)).ToArray();

        return rows.Sum(r => r.SolvePart1());
    }

    

    public static int Part2(string input, ILogger logger = null)
    {
        try
        {
            var rows = Util.Split(input, "\n").Select(r => new Machine(r)).ToArray();

            //int i = 0;
            //int sum = 0;



            //foreach (var r in rows)
            //{
            //    if (logger != null) logger.WriteLine($"{i++} / {rows.Length}");
            //    sum += r.SolvePart2();
            //}

            //return sum;


            var previous = "1 = 157\r\n11 = 55\r\n12 = 43\r\n16 = 152\r\n17 = 38\r\n23 = 35\r\n26 = 33\r\n35 = 153\r\n36 = 46\r\n39 = 34\r\n40 = 26\r\n5 = 141\r\n54 = 23\r\n58 = 27\r\n6 = 37\r\n60 = 56\r\n62 = 217\r\n64 = 41\r\n7 = 30\r\n70 = 134\r\n72 = 7\r\n73 = 50\r\n79 = 61\r\n80 = 45\r\n83 = 29\r\n86 = 29\r\n87 = 32\r\n88 = 34\r\n89 = 35";

            var cache = new Dictionary<int, int>();
            foreach (var line in previous.Split("\r\n"))
            {
                var parts = line.Split(" = ");
                cache[int.Parse(parts[0])] = int.Parse(parts[1]);
            }

            object locky = new();
            int num = 0;
            int count = rows.Count();

            int DoSolve(Machine m, int index)
            {
                if (cache.ContainsKey(index))
                {
                    lock (locky)
                    {
                        num++;
                    }
                    return cache[index];
                }

                int v = m.SolvePart2();

                Console.WriteLine($"{index} = {v}");
                lock(locky)
                {
                    Console.WriteLine($"[{num++} / {count}]");
                }

                return v;
            }


            //return rows.Index()
            //    .AsParallel()
            //    .WithDegreeOfParallelism(64)
            //    .Select((r) => r.Item.SolvePart2(r.Index)).Sum();

            return rows.Index()
                .AsParallel()
                .WithDegreeOfParallelism(64)
                .Select((r) => DoSolve(r.Item, r.Index)).Sum();

            //return rows.Select((r) => r.SolvePart2(0)).Sum();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    public void Run(string input, ILogger logger)
    {
        //logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input, logger));

        //Console.WriteLine(Part2("[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}"));

        //Console.WriteLine(Part2("[..#..] (1,4) (0,3) (2) {4,11,19,4,11}"));
    }
}