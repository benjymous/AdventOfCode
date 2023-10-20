using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day07 : IPuzzle
    {
        public string Name => "2018-07";

        private class Factory
        {
            public Factory(string input)
            {
                Util.Split(input).ForEach(line => AddDependency(line.Split(" ").Where((v, idx) => idx == 1 || idx == 7).Select(str => str[0]).ToArray().Decompose2()));
                steps = dependencies.Keys.Concat(dependencies.SelectMany(kvp => kvp.Value)).ToHashSet();
            }

            void AddDependency((char dependency, char task) entry) => dependencies.GetOrCalculate(entry.task, _ => new()).Add(entry.dependency);

            readonly HashSet<char> steps;
            readonly Dictionary<char, HashSet<char>> dependencies = new();
            readonly HashSet<char> ready = new();

            public bool WorkToDo => steps.Count != 0;
            public bool WorkReady => ready.Count != 0;

            public char GetNext()
            {
                var next = ready.Min();
                steps.Remove(next);
                ready.Remove(next);

                return next;
            }

            public void UpdateDependencies() => ready.UnionWith(steps.Where(step => !dependencies.ContainsKey(step)));
            public void CompleteTask(char task) => dependencies.Where(kvp => kvp.Value.Remove(task) && kvp.Value.Count == 0).ForEach(kvp => dependencies.Remove(kvp.Key));
        }

        class Worker
        {
            char task;
            int timeRemaining = 0;
            public bool Busy => timeRemaining > 0;

            public void DoWork(Factory factory)
            {
                if (timeRemaining == 0 && factory.WorkReady)
                {
                    task = factory.GetNext();
                    timeRemaining = task - 4; // A = 61, B = 62, etc
                }
                if (timeRemaining > 0 && --timeRemaining == 0) factory.CompleteTask(task);
            }
        }

        public static string Part1(string input)
        {
            var factory = new Factory(input);

            List<char> result = new();
            while (factory.WorkToDo)
            {
                factory.UpdateDependencies();
                var next = factory.GetNext();
                factory.CompleteTask(next);
                result.Add(next);
            }

            return result.AsString();
        }

        public static int Part2(string input)
        {
            var factory = new Factory(input);
            List<Worker> workers = Util.CreateMultiple<Worker>(5);

            int time = 0;

            while (factory.WorkToDo || workers.Any(w => w.Busy))
            {
                factory.UpdateDependencies();
                workers.ForEach(w => w.DoWork(factory));
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