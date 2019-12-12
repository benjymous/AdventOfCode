using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day12 : IPuzzle
    {
        public string Name { get { return "2019-12";} }


        public class Body
        {
            ManhattanVector3 position;
            ManhattanVector3 velocity = new ManhattanVector3(0,0,0);
            public Body(string input)
            {
                position = new ManhattanVector3(input);
            }

            public void ApplyGravity(Body other)
            {
                for (int i=0; i<position.ComponentCount; ++i)
                {
                    velocity.Component[i] += Math.Sign(other.position.Component[i]-position.Component[i]);
                }
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

            public string HashString()
            {
                return ToString().GetMD5String();
            }

            public int Energy()
            {
                return bodies.Select(b => b.Energy()).Sum();
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

        public static int Part2(string input)
        {
            var system = new System(input);

            var seen = new HashSet<string>();

            int step = 0;

            while (true)
            {
                var hash = string.Join("",system.ToString().GetMD5().Select(i=>(char)i));
                if (seen.Contains(hash)) return step;
                seen.Add(hash);
                system.Step();
                step++;
            }
        }

        public void Run(string input, TextWriter console)
        {
            console.WriteLine(Part2("<x=-1, y=0, z=2>\n<x=2, y=-10, z=-7>\n<x=4, y=-8, z=8>\n<x=3, y=5, z=-1>"));

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}