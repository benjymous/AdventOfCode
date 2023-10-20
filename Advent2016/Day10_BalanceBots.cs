using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day10 : IPuzzle
    {
        public string Name => "2016-10";

        class Entity
        {
            public string id;
            public Entity High, Low;
            public List<int> parts = new();

            public void Take(int num) => parts.Add(num);
            public int Value => parts.First();

            public bool CanPass() => parts.Count == 2;

            public (int, int) Pass()
            {
                var lowval = parts.Min();
                var highval = parts.Max();
                Low.Take(lowval);
                High.Take(highval);
                parts.Clear();
                return (lowval, highval);
            }

            public override string ToString() => $"{id} : {Low.id} < [{string.Join(", ", parts)}] > {High.id}";
        }

        class Factory
        {
            public Dictionary<string, Entity> Entities = new();
            public Dictionary<(int, int), string> Log = new();

            public Factory(string input)
            {
                foreach (var instr in Util.Split(input))
                {
                    var bits = instr.Split(" ");
                    if (bits[0] == "bot")
                    {
                        var bot = GetEntity($"{bits[0]} {bits[1]}");
                        bot.Low = GetEntity($"{bits[5]} {bits[6]}");
                        bot.High = GetEntity($"{bits[10]} {bits[11]}");
                    }
                    else if (bits[0] == "value")
                    {
                        GetEntity($"{bits[4]} {bits[5]}").Take(int.Parse(bits[1]));
                    }
                }
            }

            public Entity GetEntity(string id) => Entities.GetOrCalculate(id, _ => new Entity() { id = id });

            public void Run()
            {
                bool cont;
                do
                {
                    cont = false;
                    foreach (var bot in Entities.Values.Where(b => b.CanPass()))
                    {
                        Log[bot.Pass()] = bot.id;
                        cont = true;
                    }
                } while (cont);
            }
        }

        public static string Part1(string input)
        {
            var factory = new Factory(input);
            factory.Run();
            return factory.Log[(17, 61)];
        }

        public static int Part2(string input)
        {
            var factory = new Factory(input);
            factory.Run();
            return factory.GetEntity("output 0").Value * factory.GetEntity("output 1").Value * factory.GetEntity("output 2").Value;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}