using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day12 : IPuzzle
    {
        public string Name => "2019-12";

        public class Body
        {
            [Regex("<x=(.+), y=(.+), z=(.+)>")]
            public Body(int x, int y, int z) => position = new int[] { x, y, z };

            public int[] position, velocity = new int[] { 0, 0, 0 };

            public void ApplyGravity(Body other, int component) => velocity[component] += Math.Sign(other.position[component] - position[component]);

            public void ApplyVelocity(int component) => position[component] += velocity[component];
        }

        public class System
        {
            readonly Body[] bodies;
            public System(string input) => bodies = Util.RegexParse<Body>(input).ToArray();

            public void Step(int component)
            {
                for (int i = 0; i < bodies.Length; ++i)
                {
                    var b1 = bodies[i];
                    for (int j = i + 1; j < bodies.Length; ++j)
                    {
                        var b2 = bodies[j];
                        b1.ApplyGravity(b2, component);
                        b2.ApplyGravity(b1, component);
                    }
                    b1.ApplyVelocity(component);
                }
            }

            public void Step()
            {
                for (int i = 0; i < 3; ++i) Step(i);
            }

            public int GetState(int component)
            {
                if (bodies[0].velocity[component] != 0) return 0;
                unchecked
                {
                    int hash = 17;
                    foreach (var b in bodies) hash = hash * 31 + b.position[component];
                    return hash;
                }
            }

            public int Energy() => bodies.Sum(b => (b.position.Sum(Math.Abs) * b.velocity.Sum(Math.Abs)));
        }

        public static int Part1(string input)
        {
            var system = new System(input);

            for (int i = 0; i < 1000; ++i) system.Step();

            return system.Energy();
        }

        public static int FindCycle(string input, int component)
        {
            var system = new System(input);

            var starthash = system.GetState(component);

            for (int step = 1; true; ++step)
            {
                system.Step(component);
                if (system.GetState(component) == starthash) return step;
            }
        }

        public static long Part2(string input)
        {
            return Enumerable.Range(0, 3)
                             .AsParallel()
                             .Select(i => FindCycle(input, i))
                             .Aggregate(1L, (curr, next) => Util.LCM(curr, next));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}