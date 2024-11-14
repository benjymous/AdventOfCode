namespace AoC.Advent2016;
public class Day10 : IPuzzle
{
    public class Entity(string id)
    {
        public readonly string Id = id;
        public Entity High, Low;
        public List<int> parts = [];

        public void Take(int num) => parts.Add(num);
        public int Value => parts.First();

        public bool CanPass => parts.Count == 2;

        public (int, int) Pass()
        {
            var (lowval, highval) = parts.MinMax();
            Low.Take(lowval);
            High.Take(highval);
            parts.Clear();
            return (lowval, highval);
        }
    }

    public class Factory
    {
        public Dictionary<string, Entity> Entities = [];

        [Regex(@"(.+) gives low to (.+) and high to (.+)")]
        public void ProcessLine(string sender, string lowDest, string highDest)
        {
            var senderBot = this[sender];
            senderBot.Low = this[lowDest];
            senderBot.High = this[highDest];
        }

        [Regex(@"value (.+) goes to (.+)")]
        public void ProcessLine(int value, string dest) => this[dest].Take(value);

        public Entity this[string id] => Entities.GetOrCalculate(id, _ => new Entity(id));

        public string Run(bool peek = false)
        {
            bool havePassed;
            do
            {
                havePassed = false;
                foreach (var bot in Entities.Values.Where(b => b.CanPass))
                {
                    if (bot.Pass() == (17, 61) && peek) return bot.Id;
                    havePassed = true;
                }
            } while (havePassed);
            return default;
        }

        public static implicit operator Factory(string data) => Util.RegexFactory<Factory>(data);
    }

    public static string Part1(Factory factory) => factory.Run(true);

    public static long Part2(Factory factory)
    {
        factory.Run();
        return factory["output 0"].Value * factory["output 1"].Value * factory["output 2"].Value;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}