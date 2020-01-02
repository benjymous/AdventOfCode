using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Collections;

namespace Advent
{
    public class Util
    {
        public static string[] Split(string input, char splitChar='\0')
        {
            if (splitChar == '\0')
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
            else
            {
                return input.Split(splitChar).Select(e => e.Replace("\n","")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }

        public static List<T> Parse<T>(string input, char splitChar='\n')
        {
            return input.Split(splitChar)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(line => (T)Activator.CreateInstance(typeof(T), new object[] {line}))
                        .ToList(); 
        }

        public static int[] Parse(string input, char splitChar='\0') => Parse(Split(input, splitChar));
        public static Int64[] Parse64(string input, char splitChar='\0') => Parse64(Split(input, splitChar));

        public static int[] Parse(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();
        public static Int64[] Parse64(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int64.Parse(s)).ToArray();

        public static IEnumerable<IEnumerable<T>> Slice<T>(IEnumerable<T> source, int sliceSize)
        {
            return  source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / sliceSize)
                .Select(x => x.Select(v => v.Value));
        }

        public static IEnumerable<(T1 x, T2 y)> Matrix<T1,T2>(IEnumerable<T1> set1, IEnumerable<T2> set2)
        {
            foreach (T1 x in set1)
            {
                foreach (T2 y in set2)
                {
                    yield return (x,y);
                }
            }
        }

        public static IEnumerable<(int x, int y)> Matrix(int maxX, int maxY) => Matrix<int,int>(Enumerable.Range(0, maxX), Enumerable.Range(0, maxY));

        public static string GetInput(IPuzzle puzzle) => System.IO.File.ReadAllText(System.IO.Path.Combine("Data",puzzle.Name+".txt")).Replace("\r","");   

        public static string GetInput<T>() where T : IPuzzle, new() => GetInput(new T());

        public static IEnumerable<int> Forever(int start=0)
        {
            int i=start;
            while(true) { yield return i++; }
        }

        public static IEnumerable<int> RepeatForever(IEnumerable<int> input)
        {
            while (true)
            {
                foreach (var i in input)
                {
                    yield return i;
                }
            }
        }

        public static IEnumerable<int> DuplicateDigits(IEnumerable<int> input, int repeats)
        {
            foreach (var i in input)
            {
                for (int j=0; j<repeats; ++j)
                {
                    yield return i;
                }
            }
        }

        public static void Test<T>(T actual, T expected)
        {
            if (!EqualityComparer<T>.Default.Equals(actual, expected))
            {
                throw new Exception($"Expected {expected} but got {actual}" );
            }
            Console.WriteLine(actual);
        }

        public static int[] ExtractNumbers(string input) => input.Where(c => (c==' ' || c=='-' || (c>='0' && c<='9'))).AsString().Trim().Split(" ").Where(w => !string.IsNullOrEmpty(w)).Select(w => int.Parse(w)).ToArray();

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
            var bits = Split(val);
            Component = bits.Select(s => int.Parse(s)).ToArray();
        }

        public int[] Component = null;

        public int ComponentCount { get { return Component.Length; } }

        public override string ToString()
        {
            return string.Join(",", Component);
        }

        private string[] Split(string val)
        {
            const string keep = "0192345678,-";
            StringBuilder sb = new StringBuilder();
            foreach (var c in val)
            {
               if (keep.Contains(c)) sb.Append(c);
            }
            return sb.ToString().Split(",");
        }

        public void Set(params int [] newVal)
        {
            if (newVal.Length != ComponentCount) throw new System.Exception("Incompatible vectors");
            Component = newVal;
        }

        public void Offset(params int [] offset)
        {
            if (offset.Length != ComponentCount) throw new System.Exception("Incompatible vectors");
            for (int i=0; i<offset.Length; ++i)
            {
                Component[i] += offset[i];
            }
        }

        protected void Offset(ManhattanVectorN offset)
        {
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

        public int Distance(params int[] other)
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
        public ManhattanVector2(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2");
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

        public void Offset(Direction2 dir, int multiple=1)
        {
            Offset(dir.DX*multiple, dir.DY*multiple);
        }

        public static ManhattanVector2 operator+ (ManhattanVector2 a, ManhattanVector2 b) => new ManhattanVector2((ManhattanVectorN)a + (ManhattanVectorN)b);

        public static ManhattanVector2 operator- (ManhattanVector2 a, ManhattanVector2 b) => new ManhattanVector2((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static ManhattanVector2 Zero = new ManhattanVector2(0,0);
    }

    public class ManhattanVector3 : ManhattanVectorN
    {
        public ManhattanVector3(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector2");
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

        public static ManhattanVector3 operator+ (ManhattanVector3 a, ManhattanVector3 b) => new ManhattanVector3(a + (ManhattanVectorN)b);
        public static ManhattanVector3 operator- (ManhattanVector3 a, ManhattanVector3 b) => new ManhattanVector3((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static ManhattanVector3 Zero = new ManhattanVector3(0,0,0);
    }

    public class ManhattanVector4 : ManhattanVectorN
    {
        public ManhattanVector4(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4");
        }

        public ManhattanVector4(string val)
            : base(val)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4");   
        }

        public ManhattanVector4(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4"); 
        }

        public int X { get{ return Component[0]; } set { Component[0] = value;} }
        public int Y { get{ return Component[1]; } set { Component[1] = value;} }
        public int Z { get{ return Component[2]; } set { Component[2] = value;} }
        public int W { get{ return Component[3]; } set { Component[3] = value;} }

        public static ManhattanVector4 operator+ (ManhattanVector4 a, ManhattanVector4 b) => new ManhattanVector4(a + (ManhattanVectorN)b);
        public static ManhattanVector4 operator- (ManhattanVector4 a, ManhattanVector4 b) => new ManhattanVector4((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static ManhattanVector4 Zero = new ManhattanVector4(0,0,0,0);
    }

    public class Direction2
    {
        public int DX {get;set;} = 0;
        public int DY {get;set;} = 0;

        public Direction2(int dx, int dy)
        {
            DX = dx;
            DY = dy;
        }

        public void SetDirection(int dirx, int diry)
        {
            DX = dirx;
            DY = diry;
        }

        public void TurnLeft()
        {
            // up :  0,-1 ->  -1,0;
            // left: -1,0 -> 0,1
            // down: 0,1 -> 1,0
            // right: 1,0 -> 0,-1

            if (DX==0 && DY == -1) SetDirection(-1,0);
            else if (DX==-1 && DY==0) SetDirection(0,1);
            else if (DX==0 && DY==1) SetDirection(1,0);
            else if (DX==1 && DY==0) SetDirection(0,-1);

            else throw new Exception("Unrecognised train direction: "+DX+","+DY);
        }

        public void TurnRight()
        {
            // up :  0,-1 ->  1,0;
            // right: 1,0 -> 0,1
            // down: 0,1 -> -1,0
            // left: -1,0 -> 0,-1

            if (DX==0 && DY == -1) SetDirection(1,0);
            else if (DX==1 && DY==0) SetDirection(0,1);
            else if (DX==0 && DY==1) SetDirection(-1,0);
            else if (DX==-1 && DY==0) SetDirection(0,-1);

            else throw new Exception("Unrecognised train direction :"+DX+","+DY);
        }

            public char AsChar() 
            {
                if (DX > 0) return '>';
                if (DX < 0) return '<';
                if (DY < 0) return '^';
                if (DY > 0) return 'v';

                throw new Exception("Unknown direction state");
            }
    }

    public class AutoArray<DataType> : IEnumerable<DataType>
    {
        DataType[] data;

        public AutoArray(IEnumerable<DataType> input) => data = input.ToArray();

        DataType Get(int key)
        {
            if (key>=data.Length) return default(DataType);
            return data[key];
        }

        void Set(int key, DataType value)
        {
            if (key>=data.Length) 
            {
                Console.Write($"Resize from {data.Length} to ");
                Reserve(data.Length + ((key-data.Length+1)*250));
                Console.WriteLine($"{data.Length}");
            }
            data[key]=value;
        }

        public void Reserve(int memorySize)
        {
            if (memorySize > data.Length) Array.Resize(ref data, memorySize);
        }

        public IEnumerator<DataType> GetEnumerator()
        {
            foreach (var item in data)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public DataType this[int key]
        {
            get => Get(key);
            set => Set(key, value);
        }
    }

    public class TextBuffer : System.IO.TextWriter
    {
        StringBuilder builder = new StringBuilder();
        public override void Write(char value)
        {
            builder.Append(value);
        }

        public override Encoding Encoding
        {
            get => System.Text.Encoding.UTF8;
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }

    public static class HashBreaker
    {
        static public byte[] GetHash(int num, string baseStr)
        {
            string hashInput = baseStr + num.ToString();
            return hashInput.GetMD5(); 
        }

        public static IEnumerable<char> GetHashChars(int num, string baseStr)
        {
            string hashInput = baseStr + num.ToString();
            return hashInput.GetMD5Chars();  
        }

        static bool IsHash(int num, string baseStr, int numZeroes)
        {
            var hashed = GetHash(num, baseStr).AsNybbles().Take(numZeroes);       
            return hashed.Where(b => b!=0).Count() == 0;
        }
 
        const int blockSize = 1000;
        public static int FindHash(string baseStr, int numZeroes, int start=0)
        {            
            while (true)
            {
                var block = Util.Forever(start).Take(blockSize);

                var hashes = block.AsParallel().Where(n => IsHash(n, baseStr, numZeroes));
                if (hashes.Any())
                {
                    return hashes.First();
                }

                start += blockSize;
            }
        }
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

        public static long TimeRun(this IPuzzle puzzle,  System.IO.TextWriter buffer)
        {
            var watch = new System.Diagnostics.Stopwatch();        
            watch.Start();
            buffer.WriteLine(puzzle.Name);
            var input = Util.GetInput(puzzle);
            puzzle.Run(input, buffer);
            return watch.ElapsedMilliseconds;
        }

        static T Add<T>(T x, T y) => Add((dynamic)x,(dynamic)y);
        static int Add(int a, int b) => a+b;
        static long Add(long a, long b) => a+b;
        static ulong Add(ulong a, ulong b) => a+b;

        public static void IncrementAtIndex<T,V>(this Dictionary<T,V> dict, T key, V val)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = Add(dict[key], val);
            }
            else
            {
                dict[key] = val;
            }
        }

        public static void IncrementAtIndex<T>(this Dictionary<T,int> dict, T key, int val=1) => IncrementAtIndex<T, int>(dict, key, val);

        public static void PutObjKey<T>(this Dictionary<string, T> dict, object key, T value)
        {
            PutStrKey(dict, key.ToString(), value);
        }

        public static void PutStrKey<T>(this Dictionary<string, T> dict, string key, T value)
        {
            dict[key] = value;
        }

        public static T GetStrKey<T>(this Dictionary<string, T> dict, string key)
        {
            if (dict.TryGetValue(key, out T val)) return val;
            return default(T);
        }

        public static T GetObjKey<T>(this Dictionary<string, T> dict, object key)
        {
            var k = key.ToString();
            return GetStrKey(dict, k);
        }

        public static byte[] GetSHA256(this string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetSHA256String(this string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetSHA256(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetMD5(this string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetMD5String(this string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetMD5(inputString))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        public static IEnumerable<char> GetMD5Chars(this string inputString)
        {
            foreach (byte b in GetMD5(inputString))
            {
                foreach (var c in b.ToString("X2"))
                {
                    yield return c;
                }
            }
        }

        public static string AsString(this IEnumerable<char> input)
        {
            return String.Join("", input);
        }

        public static IEnumerable<byte> AsNybbles(this byte[] bytes)
        {
            foreach (var x in bytes)
            {
                yield return (byte)((x & 0xF0) >> 4);
                yield return (byte) (x & 0x0F);             
            }
        }


        private static string vowels = "aeiouAEIOU";

        public static bool IsVowel(this char c)
        {
            return vowels.Contains(c);
        }

        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }

        public static string ReplaceAtIndex(this string str, int index, string from, string replacement)
        {
            var sb = new StringBuilder(str);
            sb.Remove(index, from.Length);
            sb.Insert(index, replacement);
            return sb.ToString();
        }

        // iterate over bits, returns sequence like 1,2,4,8 (only returning set bits in input)
        public static IEnumerable<int> BitSequence(this int v)
        {
            for(int k=1; k<=v; k<<=1)
            {
                if ((v & k)>0) 
                    yield return k;
            }
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> set, IEnumerable<T> subset = null)
        {
            if (subset == null) subset = new T[] { };
            if (!set.Any()) yield return subset;

            for (var i = 0; i < set.Count(); i++)
            {
                var newSubset = set.Take(i).Concat(set.Skip(i + 1));
                foreach (var permutation in Permutations(newSubset, subset.Concat(set.Skip(i).Take(1))))
                {
                    yield return permutation;
                }
            }
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> items, T first)
        {
            yield return first;
            foreach (T item in items)
            {
                yield return item;
            }
        }
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> items)
        {
            if (!items.Any())
            {
                yield return items;
            }
            else
            {
                var head = items.First();
                var tail = items.Skip(1);
                foreach (var sequence in tail.Combinations())
                {
                    yield return sequence; // Without first
                    yield return sequence.Prepend(head);
                }
            }
        }

        public static int LevenshteinDistance(this string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++);
            for (int j = 1; j <= m; d[0, j] = j++);

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

        public static IEnumerable<T> Values<T>(this T[,] array2d)
        {
            for (int row = 0; row < array2d.GetLength(0); row++)
            {
                for (int col = 0; col < array2d.GetLength(1); col++)
                {
                    yield return array2d[row, col];
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }
    }
}
