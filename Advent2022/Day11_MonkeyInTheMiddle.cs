using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Advent2022
{
    public class Day11 : IPuzzle
    {
        public string Name => "2022-11";

        public class Monkey
        {
            [Regex(@"Monkey (.+): Starting items: (.+) Operation: new = old (.) (.+) Test: divisible by (.+) If true: throw to monkey (.) If false: throw to monkey (.)")]
            public Monkey(int id, string items, char op, string opBy, int test, int ifTrue, int ifFalse)
            {
                Id = id;
                Items = Util.ParseNumbers<long>(items).ToList();
                if (op == '+')
                {
                    Operation = old => old + int.Parse(opBy);
                }
                else if (op == '*')
                {
                    Operation = old => old * (opBy == "old" ? old : int.Parse(opBy));
                }
                Divisor = test;
                IfTrue = ifTrue;
                IfFalse = ifFalse;
            }

            readonly public int Id, Divisor;
            readonly List<long> Items;
            readonly Func<long, long> Operation;
            readonly int IfTrue, IfFalse;

            public IEnumerable<(long worry, int target)> DoRound(bool lessWorry)
            { 
                foreach (var i in Items)
                {
                    var item = Operation(i) / (lessWorry ? 3 : 1);
                    yield return item % Divisor == 0 ? (item, IfTrue) : (item, IfFalse);
                }
                Items.Clear();
            }

            public void AddItem(long item) => Items.Add(item);
        }

        private static long RunRounds(string input, int numRounds, bool lessWorry)
        {
            var data = Util.RegexParse<Monkey>(input.Split("\n\n").Select(line => line.Replace("\n", "").WithoutMultipleSpaces())).ToArray();

            var filter = data.Select(m => m.Divisor).Product();

            Dictionary<int, int> monkeyScores = new();
            for (int round = 0; round < numRounds; ++round)
            {
                foreach (var monkey in data)
                {
                    var actions = monkey.DoRound(lessWorry).ToArray();
                    monkeyScores.IncrementAtIndex(monkey.Id, actions.Length);
                    foreach (var (worry, target) in actions)
                    {
                        data[target].AddItem(worry % filter);
                    }
                }
            }

            return monkeyScores.Values.OrderDescending().Take(2).Product();
        }

        public static long Part1(string input)
        {
            return RunRounds(input, 20, true);
        }

        public static long Part2(string input)
        {
            return RunRounds(input, 10000, false);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}