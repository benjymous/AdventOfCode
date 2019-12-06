using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;


namespace Advent
{
    public class Util
    {
        public static string[] Split(string input)
        {
            int commaCount = input.Count( c => c == ',');
            int linefeedCount = input.Count( c => c == '\n');
            if (linefeedCount > commaCount)
            {
                return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            else
            {
                return input.Split(",").Select(e => e.Replace("\n","")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }

        public static List<T> Parse<T>(string input)
        {
            return input.Split("\n")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(line => (T)Activator.CreateInstance(typeof(T), new object[] {line}))
                        .ToList(); 
        }

        public static int[] Parse(string input) => Parse(Split(input));

        public static int[] Parse(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();

        public static string GetInput(IPuzzle puzzle) => System.IO.File.ReadAllText(System.IO.Path.Combine("Data",puzzle.Name+".txt")).Replace("\r","");   
    
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }

    public interface IVec
    {
        int Distance(IVec other);
    }


    public class ManhattanVectorN : IVec
    {    
        public ManhattanVectorN(int[] components)
        {
            Component = components;
        }

        public ManhattanVectorN(string val)
        {
            var bits = val.Trim().Split(",");
            Component = bits.Select(s => int.Parse(s)).ToArray();
        }

        public int[] Component = null;

        public int ComponentCount { get { return Component.Length; } }

        public override string ToString()
        {
            return string.Join(",", Component);
        }

        protected void Set(int [] newVal)
        {
            Component = newVal;
        }

        protected void Offset(int [] offset)
        {
            for (int i=0; i<offset.Length; ++i)
            {
                Component[i] += offset[i];
            }
        }

        protected void Offset(ManhattanVectorN offset)
        {
            if (offset.ComponentCount != ComponentCount) throw new System.Exception("Incompatible vectors");
            Offset(offset.Component);
        }

        public static ManhattanVectorN operator+ (ManhattanVectorN a, ManhattanVectorN b)
        {
            if (a.ComponentCount != b.ComponentCount) throw new System.Exception("Incompatible vectors");
            int [] newVal = (int[]) (a.Component.Clone());
            for (int i=0; i<a.ComponentCount; ++i)
            {
                newVal[i]+=b.Component[i];
            }
            return new ManhattanVectorN(newVal);   
        }

        public static ManhattanVectorN operator- (ManhattanVectorN a, ManhattanVectorN b)
        {
            if (a.ComponentCount != b.ComponentCount) throw new System.Exception("Incompatible vectors");
            int [] newVal = (int[]) (a.Component.Clone());
            for (int i=0; i<a.ComponentCount; ++i)
            {
                newVal[i]-=b.Component[i];
            }  
            return new ManhattanVectorN(newVal);  
        }

        public int Distance(IVec other)
        {
            if (!(other is ManhattanVectorN)) return Int32.MaxValue;
            
            var man2 = other as ManhattanVectorN;

            return Distance(man2.Component);
        }

        public int Distance(int[] other)
        {
            if (other.Length != ComponentCount) return Int32.MaxValue;

            int distance = 0;
            for (var i=0; i<ComponentCount; ++i)
            {
                distance += Math.Abs(Component[i]-other[i]);
            }
            return distance;
        }


        public static bool operator== (ManhattanVectorN v1, ManhattanVectorN v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator!= (ManhattanVectorN v1, ManhattanVectorN v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object other)
        {
            if (!(other is ManhattanVectorN)) return false;
            return Distance(other as ManhattanVectorN) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                for (int i=0; i<ComponentCount; ++i)
                {
                    hash = hash * 31 + Component[i];
                }
                return hash;
            }
        }
    }

    public class ManhattanVector2 : ManhattanVectorN
    {
        public ManhattanVector2(int x, int y)
            : base(new int[]{x,y})
        {
        }

        public ManhattanVector2(string val)
            : base(val)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2");   
        }

        public ManhattanVector2(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2"); 
        }

        public int X { get{ return Component[0]; } set { Component[0] = value;} }
        public int Y { get{ return Component[1]; } set { Component[1] = value;} }

        public void Offset(int dx, int dy)
        {
            base.Offset(new int[]{dx,dy});
        }

        public int Distance(int x, int y)
        {
            return base.Distance(new int[]{x,y});
        }

        public static ManhattanVector2 operator+ (ManhattanVector2 a, ManhattanVector2 b)
        {
            return new ManhattanVector2((ManhattanVectorN)a + (ManhattanVectorN)b);
        }

        public static ManhattanVector2 operator- (ManhattanVector2 a, ManhattanVector2 b)
        {
            return new ManhattanVector2((ManhattanVectorN)a - (ManhattanVectorN)b);
        }

        public static ManhattanVector2 Zero = new ManhattanVector2(0,0);
    }

        public class ManhattanVector3 : ManhattanVectorN
    {
        public ManhattanVector3(int x, int y, int z)
            : base(new int[]{x,y,z})
        {
        }

        public ManhattanVector3(string val)
            : base(val)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector2");   
        }

        public ManhattanVector3(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector2"); 
        }

        public int X { get{ return Component[0]; } set { Component[0] = value;} }
        public int Y { get{ return Component[1]; } set { Component[1] = value;} }
        public int Z { get{ return Component[2]; } set { Component[2] = value;} }

        public void Offset(int dx, int dy, int dz)
        {
            base.Offset(new int[]{dx,dy,dz});
        }

        public int Distance(int x, int y, int z)
        {
            return base.Distance(new int[]{x,y,z});
        }

        public static ManhattanVector3 operator+ (ManhattanVector3 a, ManhattanVector3 b)
        {
            return new ManhattanVector3(a + (ManhattanVectorN)b);
        }

        public static ManhattanVector3 operator- (ManhattanVector3 a, ManhattanVector3 b)
        {
            return new ManhattanVector3((ManhattanVectorN)a - (ManhattanVectorN)b);
        }

        public static ManhattanVector3 Zero = new ManhattanVector3(0,0,0);
    }

    public static class Extensions
    {
        public static string[] args;

        public static bool ShouldRun(this IPuzzle puzzle)
        {
            if (args.Length == 0) return true;

            foreach (var line in args)
            {
                if (puzzle.Name.Contains(line.Trim())) return true;
            }

            return false;
        }

        public static long TimeRun(this IPuzzle puzzle)
        {
            var watch = new System.Diagnostics.Stopwatch();        
            watch.Start();
            Console.WriteLine(puzzle.Name);
            var input = Util.GetInput(puzzle);
            puzzle.Run(input);
            return watch.ElapsedMilliseconds;
        }

        public static void IncrementAtIndex<T>(this Dictionary<T,int> dict, T key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict[key] = 1;
            }
        }
    }
}
