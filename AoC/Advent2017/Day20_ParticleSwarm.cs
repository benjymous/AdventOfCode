namespace AoC.Advent2017;
public class Day20 : IPuzzle
{
    [method: Regex(@"[p]=<(-*\d+,-*\d+,-*\d+)>, [v]=<(-*\d+,-*\d+,-*\d+)>, [a]=<(-*\d+,-*\d+,-*\d+)>")]
    class Particle(ManhattanVector3 p, ManhattanVector3 v, ManhattanVector3 a)
    {
        public ManhattanVector3 pos = p, vel = v, acc = a;

        public int Distance => pos.Length;

        public Particle Step()
        {
            vel += acc;
            pos += vel;
            return this;
        }
    }

    public static int Part1(string input)
    {
        var particles = Util.RegexParse<Particle>(input).ToArray();

        var slowest = particles.OrderBy(p => p.acc.Length).First();
        return Array.IndexOf(particles, slowest);
    }

    static IEnumerable<Particle> FilterCollisions(IEnumerable<Particle> particles) => particles.GroupBy(p => p.pos).Where(g => g.Count() == 1).SelectMany(x => x);

    public static int Part2(string input)
    {
        Particle[] particles = Util.RegexParse<Particle>(input).ToArray();

        int lastCol = 0;

        while (true)
        {
            int lastCount = particles.Length;
            particles = FilterCollisions(particles.Select(p => p.Step())).ToArray();
            if (particles.Length < lastCount) lastCol = 0;

            if (++lastCol > 10) return particles.Length;
        }
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}