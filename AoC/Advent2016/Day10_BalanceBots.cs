namespace AoC.Advent2016;
public class Day10 : IPuzzle
{
    public record class Entity(string Id)
    {
        Entity High, Low;
        readonly List<int> parts = [];

        public void Take(int num) => parts.Add(num);
        public int Value => parts.First();
        public void SetChildren(Entity low, Entity high) => (High, Low) = (high, low);

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
        public void ProcessLine(string sender, string lowDest, string highDest) => this[sender].SetChildren(this[lowDest], this[highDest]);

        [Regex(@"value (.+) goes to (.+)")]
        public void ProcessLine(int value, string dest) => this[dest].Take(value);

        public Entity this[string id] => Entities.GetOrCalculate(id, _ => new Entity(id));

        public string Run(bool peek = false)
        {
            var passers = Entities.Values.Where(b => b.CanPass);
            while (passers.Any())
            {
                foreach (var bot in passers)
                {
                    if (bot.Pass() == (17, 61) && peek) return bot.Id;
                }
            }
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