using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day07 : IPuzzle
    {
        public string Name => "2018-07";


        private static void ParseInput(string input, out HashSet<string> steps, out Dictionary<string, HashSet<string>> dependencies)
        {
            var lines = Util.Split(input);
            steps = new HashSet<string>();
            dependencies = new Dictionary<string, HashSet<string>>();
            foreach (var line in lines)
            {
                var bits = line.Split(" ");

                if (!steps.Contains(bits[1])) steps.Add(bits[1]);
                if (!steps.Contains(bits[7])) steps.Add(bits[7]);

                if (!dependencies.ContainsKey(bits[7]))
                {
                    dependencies[bits[7]] = new HashSet<string>();
                }
                dependencies[bits[7]].Add(bits[1]);
            }
        }

        public static string Part1(string input)
        {
            ParseInput(input, out HashSet<string> steps, out Dictionary<string, HashSet<string>> dependencies);

            List<string> result = new();
            HashSet<string> ready = new();
            while (steps.Count > 0)
            {

                foreach (var step in steps)
                {
                    if (!dependencies.ContainsKey(step) || dependencies[step].Count == 0)
                    {
                        ready.Add(step);
                    }
                }

                var next = ready.Order().First();
                result.Add(next);
                steps.Remove(next);
                ready.Remove(next);

                foreach (var step in steps)
                {
                    if (dependencies.ContainsKey(step))
                    {
                        dependencies[step].Remove(next);
                    }
                }

            }

            return string.Join("", result);
        }

        class Worker
        {
            public string step = null;
            public int timeRemaining = 0;
        }

        public static int Part2(string input)
        {
            ParseInput(input, out HashSet<string> steps, out Dictionary<string, HashSet<string>> dependencies);

            List<string> result = new();
            HashSet<string> ready = new();

            List<Worker> workers = new();

            const int workerCount = 5;

            for (int i = 0; i < workerCount; ++i)
            {
                workers.Add(new Worker());
            }

            int time = 0;

            while (steps.Count > 0 || workers.Any(w => w.step != null))
            {
                foreach (var step in steps)
                {
                    if (!dependencies.ContainsKey(step) || dependencies[step].Count == 0)
                    {
                        ready.Add(step);
                    }
                }

                foreach (var worker in workers)
                {
                    if (worker.step == null)
                    {
                        if (ready.Count > 0)
                        {
                            var next = ready.Order().First();
                            ready.Remove(next);
                            steps.Remove(next);
                            worker.step = next;
                            worker.timeRemaining = next[0] - 4; // A = 61, B = 62, etc
                        }
                    }

                    if (worker.step != null)
                    {
                        worker.timeRemaining--;

                        if (worker.timeRemaining == 0)
                        {
                            result.Add(worker.step);

                            foreach (var step in steps)
                            {
                                if (dependencies.ContainsKey(step))
                                {
                                    dependencies[step].Remove(worker.step);
                                }
                            }

                            worker.step = null;
                        }
                    }

                }
                time++;

            }

            return time;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}