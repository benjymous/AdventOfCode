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


    public class ManhattanVector2 : IVec
    {    
        public ManhattanVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public ManhattanVector2(string val)
        {
            var bits = val.Trim().Split(",");
            X = int.Parse(bits[0]);
            Y = int.Parse(bits[1]);
        }

        public int X {get;set;}
        public int Y {get;set;}

        public override string ToString()
        {
            return X+","+Y;
        }

        public void Offset(int dx, int dy)
        {
            X+=dx;
            Y+=dy;
        }

        public static ManhattanVector2 operator+ (ManhattanVector2 a, ManhattanVector2 b)
        {
            return new ManhattanVector2(a.X+b.X, a.Y+b.Y);   
        }

        public static ManhattanVector2 operator- (ManhattanVector2 a, ManhattanVector2 b)
        {
            return new ManhattanVector2(a.X-b.X, a.Y-b.Y);   
        }

        public int Distance(IVec other)
        {
            if (!(other is ManhattanVector2)) return Int32.MaxValue;

            var man2 = other as ManhattanVector2;
            return Distance(man2.X, man2.Y);
        }

        public int Distance(int x, int y)
        {
            return Math.Abs(X-x)+Math.Abs(Y-y);
        }

        public static bool operator== (ManhattanVector2 v1, ManhattanVector2 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator!= (ManhattanVector2 v1, ManhattanVector2 v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object other)
        {
            if (!(other is ManhattanVector2)) return false;
            return Distance(other as ManhattanVector2) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + X.GetHashCode();
                hash = hash * 31 + Y.GetHashCode();
                return hash;
            }
        }

        public static ManhattanVector2 Zero = new ManhattanVector2(0,0);
    }


    public class ManhattanVector3 : IVec
    {
        public ManhattanVector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X {get;set;}
        public int Y {get;set;}
        public int Z {get;set;}

        public override string ToString()
        {
            return X+","+Y+","+Z;
        }

        public void Offset(int dx, int dy, int dz)
        {
            X+=dx;
            Y+=dy;
            Z+=dz;
        }

        public static ManhattanVector3 operator+ (ManhattanVector3 a, ManhattanVector3 b)
        {
            return new ManhattanVector3(a.X+b.X, a.Y+b.Y, a.Z+b.Z);   
        }

        public static ManhattanVector3 operator- (ManhattanVector3 a, ManhattanVector3 b)
        {
            return new ManhattanVector3(a.X-b.X, a.Y-b.Y, a.Z-b.Z);   
        }

        public int Distance(IVec other)
        {
            if (!(other is ManhattanVector3)) return Int32.MaxValue;

            var man2 = other as ManhattanVector3;
            return Math.Abs(X-man2.X) + Math.Abs(Y-man2.Y) + Math.Abs(Z-man2.Z);
        }

        public static bool operator== (ManhattanVector3 v1, ManhattanVector3 v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator!= (ManhattanVector3 v1, ManhattanVector3 v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object other)
        {
            if (!(other is ManhattanVector3)) return false;
            return Distance(other as ManhattanVector3) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + X.GetHashCode();
                hash = hash * 31 + Y.GetHashCode();
                hash = hash * 31 + Z.GetHashCode();
                return hash;
            }
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
