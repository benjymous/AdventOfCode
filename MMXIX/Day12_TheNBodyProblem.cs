using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Advent.Utils;
using Advent.Utils.Vectors;

namespace Advent.MMXIX
{
    public class Day12 : IPuzzle
    {
        public string Name { get { return "2019-12";} }


        public class Body
        {
            public ManhattanVector3 position;
            public ManhattanVector3 velocity = new ManhattanVector3(0,0,0);
            public Body(string input)
            {
                position = new ManhattanVector3(input);
            }

            public void ApplyGravity(Body other, int component)
            {
                velocity.Component[component] += Math.Sign(other.position.Component[component]-position.Component[component]);
            }

            public void ApplyGravity(Body other)
            {
                for (int i=0; i<position.ComponentCount; ++i)
                {
                    ApplyGravity(other, i);
                }
            }

            public void ApplyVelocity(int component)
            {
                position.Component[component] += velocity.Component[component];
            }

            public void ApplyVelocity()
            {
                position += velocity;
            }

            public override string ToString()
            {
                return $"pos=<{position}> vel=<{velocity}>";
            }

            public int Energy()
            {
                var pot = position.Component.Select(n => Math.Abs(n)).Sum();
                var kin = velocity.Component.Select(n => Math.Abs(n)).Sum();
                return pot*kin;                     
            }
        }

        public class System
        {
            List<Body> bodies;
            public System(string input)
            {
                bodies = Util.Parse<Body>(input);
            }

            public void Step(int component)
            {
                UpdateGravity(component);
                UpdatePositions(component);
            }

            public void Step()
            {
                UpdateGravity();
                UpdatePositions();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var b in bodies)
                {
                    sb.Append(b);
                    sb.Append('\n');
                }
                return sb.ToString();
            }

            public string GetStateString(int component)
            {
                var sb = new StringBuilder();
                foreach (var b in bodies)
                {
                    sb.Append(b.position.Component[component]);
                    sb.Append(':');
                    sb.Append(b.velocity.Component[component]);
                    sb.Append(':');
                }
                return sb.ToString();
            }

            public string HashString()
            {
                return ToString().GetMD5String();
            }

            public int Energy()
            {
                return bodies.Select(b => b.Energy()).Sum();
            }

            void UpdateGravity(int component)
            {
                foreach (var b1 in bodies)
                {
                    foreach (var b2 in bodies)
                    {
                        b1.ApplyGravity(b2, component);
                    }
                }
            }

            void UpdateGravity()
            {
                foreach (var b1 in bodies)
                {
                    foreach (var b2 in bodies)
                    {
                        b1.ApplyGravity(b2);
                    }
                }
            }

            void UpdatePositions(int component)
            {
                foreach (var b in bodies)
                {
                    b.ApplyVelocity(component);
                }
            }

            void UpdatePositions()
            {
                foreach (var b in bodies)
                {
                    b.ApplyVelocity();
                }
            }
        }
 
        public static int Part1(string input)
        {
            var system = new System(input);

            for (int i=0; i<1000; ++i)
            {
                //Console.WriteLine($"After {i} steps");
                //system.Print();

                system.Step();
            }

            return system.Energy();
        }

        public static int FindCycle(string input, int component)
        {
            var system = new System(input);

            int step = 0;

            var starthash = system.GetStateString(component);

            while (true)
            {
                system.Step(component);
                step++;
                var hash = system.GetStateString(component);
                if (hash == starthash) return step;
            }
        }

        static Int64 CalcLCM(Int64 a, Int64 b)
        {
            Int64 num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (Int64 i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num1 * num2;
        }

        public static Int64 Part2(string input)
        {           
            var results = Enumerable.Range(0,3).AsParallel().Select(i => FindCycle(input, i));

            Int64 result = 1;
            foreach (var r in results)
            {
                result = CalcLCM(result, r);
            }

            return result;
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine(Part2("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}