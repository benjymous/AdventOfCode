namespace AoC.Advent2016;
public class Day10 : IPuzzle
{
    class Entity(string id)
    {
        public readonly string Id = id;
        public Entity High, Low;
        public List<int> parts = [];

        public void Take(int num) => parts.Add(num);
        public int Value => parts.First();

        public bool CanPass => parts.Count == 2;

        public (int, int) Pass()
        {
            var lowval = parts.Min();
            var highval = parts.Max();
            Low.Take(lowval);
            High.Take(highval);
            parts.Clear();
            return (lowval, highval);
        }
    }

    class Factory
    {
        public Dictionary<string, Entity> Entities = [];

        [Regex(@"(.+) gives low to (.+) and high to (.+)")]
        public void ProcessLine(string sender, string lowDest, string highDest)
        {
            var senderBot = GetEntity(sender);
            senderBot.Low = GetEntity(lowDest);
            senderBot.High = GetEntity(highDest);
        }

        [Regex(@"value (.+) goes to (.+)")]
        public void ProcessLine(int value, string dest) => GetEntity(dest).Take(value);

        public Entity GetEntity(string id) => Entities.GetOrCalculate(id, _ => new Entity(id));

        public string Run(bool peek = false)
        {
            bool cont;
            do
            {
                cont = false;
                foreach (var bot in Entities.Values.Where(b => b.CanPass))
                {
                    if (bot.Pass() == (17, 61) && peek) return bot.Id;
                    cont = true;
                }
            } while (cont);
            return default;
        }
    }

    public static string Part1(string input)
    {
        var factory = Util.RegexFactory<Factory>(input);
        return factory.Run(true);
    }

    public static long Part2(string input)
    {
        var factory = Util.RegexFactory<Factory>(input);
        factory.Run();
        return factory.GetEntity("output 0").Value * factory.GetEntity("output 1").Value * factory.GetEntity("output 2").Value;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}