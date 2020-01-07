using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day10 : IPuzzle
    {
        public string Name { get { return "2016-10";} }

        class Entity
        {
            public string id;
            public void Take(int num)
            {
                parts.Add(num);
            }

            public List<int> parts = new List<int>();

            public int Value()
            {
                if (parts.Count!=1) throw new Exception($"Too many values at {id}");

                return parts.First();
            }

            public override string ToString() => $"{id} : [{string.Join(", ", parts)}]";
        }

        class Bot : Entity
        {
            public Entity High;
            public Entity Low;

            public bool CanPass() => (parts.Count == 2);

            public string Pass()
            {         
                var lowval = parts.Min();
                var highval = parts.Max();
                //Console.WriteLine($"{id}: sends {lowval} to {Low.id} and {highval} to {High.id}");
                Low.Take(lowval);
                High.Take(highval);
                parts.Clear();
                return $"{lowval},{highval}";
            }

            public override string ToString() => $"{id} : {Low.id} < [{string.Join(", ", parts)}] > {High.id}";
        }

        class Factory
        {
            public Dictionary<string,Entity> Entities = new Dictionary<string, Entity>();
            public Dictionary<string,string> Log = new Dictionary<string, string>();

            public Factory(string input)
            {
                var instructions = Util.Split(input);

                foreach (var instr in instructions)
                {
                    //var numbers = Util.ExtractNumbers(instr);
                    var bits = instr.Split(" ");
                    if (bits[0] == "bot")
                    {                      
                        var bot = GetEntity($"{bits[0]} {bits[1]}") as Bot;
                        var low = GetEntity($"{bits[5]} {bits[6]}");
                        var high = GetEntity($"{bits[10]} {bits[11]}");

                        bot.Low = low;
                        bot.High = high;
                    }
                    else if (bits[0] == "value")
                    {
                        var bot = GetEntity($"{bits[4]} {bits[5]}");
                        bot.Take(int.Parse(bits[1]));
                    }
                }
            }

            public Entity GetEntity(string id)
            {
                if (Entities.TryGetValue(id, out var entity))
                {
                    return entity;
                }
                else
                {
                    Entity h;
                    if (id.StartsWith("bot"))
                    {
                        h = new Bot(){id=id};
                    }
                    else
                    {
                        h = new Entity(){id=id};
                    }
                    Entities[id] = h;
                    return h;
                }
            }

            public void Run()
            {
                bool cont;
                do
                {
                    cont = false;
                    foreach (var bot in Entities.Values.OfType<Bot>())
                    {
                        if (bot.CanPass())
                        {                        
                            var msg = bot.Pass();
                            Log[msg] = bot.id;
                            cont = true;
                        }
                    }
                } while (cont);
            }
        }
 
        public static string Part1(string input)
        {
            var factory = new Factory(input);
            factory.Run();
            return factory.Log["17,61"];
        }

        public static int Part2(string input)
        {
            var factory = new Factory(input);
            factory.Run();
            return factory.GetEntity("output 0").Value() * factory.GetEntity("output 1").Value() * factory.GetEntity("output 2").Value();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}