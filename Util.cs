using AoC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC
{
    public enum QuestionPart
    {
        Part1 = 1,
        Part2 = 2
    }

    public static class QuestionPartExtensions
    {
        public static bool One(this QuestionPart part) => part == QuestionPart.Part1;
        public static bool Two(this QuestionPart part) => part == QuestionPart.Part2;
    }

    public class Util
    {
        public static IEnumerable<IPuzzle> GetPuzzles()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IPuzzle).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (IPuzzle)Activator.CreateInstance(x))
                .Where(p => p.ShouldRun())
                .OrderBy(x => x.Name);
        }

        public static string[] Split(string input, char splitChar = '\0')
        {
            if (splitChar == '\0')
            {
                int commaCount = input.Count(c => c == ',');
                int linefeedCount = input.Count(c => c == '\n');
                if (linefeedCount > commaCount)
                {
                    return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
                }
                else
                {
                    return input.Split(",").Select(e => e.Replace("\n", "")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                }
            }
            else
            {
                return input.Split(splitChar).Select(e => e.Replace("\n", "")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }

        internal static int WrapIndex(int v, int length)
        {
            return v % length;
        }

        public static T Create<T>(string line)
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { line });
        }

        public static List<T> Parse<T>(IEnumerable<string> input)
        {
            return input.Select(Create<T>)
                        .ToList();
        }

        public static List<T> Parse<T, C>(string input, C cache, string splitter = "\n")
        {
            return Parse<T, C>(input.Split(splitter)
                                 .Where(x => !string.IsNullOrWhiteSpace(x)), cache);
        }

        public static List<T> Parse<T, C>(IEnumerable<string> input, C cache)
        {
            return input.Select(line => (T)Activator.CreateInstance(typeof(T), new object[] { line, cache }))
                        .ToList();
        }

        public static List<T> Parse<T>(string input, string splitter = "\n")
        {
            return Parse<T>(input.Split(splitter)
                                 .Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public static T RegexCreate<T>(string line)
        {
            foreach (var typeConstructor in typeof(T).GetConstructors())
            {
                if (typeConstructor?.GetCustomAttributes(typeof(RegexAttribute), true)
                    .FirstOrDefault() is not RegexAttribute attribute) continue; // skip constructor if it doesn't have attr

                var matches = Regex.Matches(line, attribute.Pattern); // match this constructor against the input line

                if (!matches.Any()) continue; // Try other constructors to see if they match

                var paramInfo = typeConstructor.GetParameters();

                object[] convertedParams = ConstructParams(matches, paramInfo);

                return (T)Activator.CreateInstance(typeof(T), convertedParams);
            }
            throw new Exception("RegexParse failed to find suitable constructor for " + typeof(T).Name);
        }

        private static object[] ConstructParams(MatchCollection matches, ParameterInfo[] paramInfo)
        {
            var regexValues = matches[0]
                .Groups.Values.Where(v => !string.IsNullOrWhiteSpace(v.Value))
                .Skip(1).Select(g => g.Value);

            if (regexValues.Count() != paramInfo.Length) throw new Exception("RegexParse couldn't match constructor param count");

            var instanceParams = Enumerable.Zip(paramInfo, regexValues); // collate parameter types and matched substrings

            var convertedParams = instanceParams
                .Select(kvp => TypeDescriptor.GetConverter(kvp.First.ParameterType).ConvertFromString(kvp.Second)).ToArray(); // convert substrings to match constructor input
            return convertedParams;
        }

        public static IEnumerable<T> RegexParse<T>(IEnumerable<string> input) =>
            input.Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(RegexCreate<T>);//.ToList();

        public static IEnumerable<T> RegexParse<T>(string input, string splitter = "\n") =>
            RegexParse<T>(input.Split(splitter));


        public static T RegexFactoryCreate<T>(string line, object factory)
        {
            foreach (var func in factory.GetType().GetMethods())
            {
                if (func.GetCustomAttributes(typeof(RegexAttribute), true)
                    .FirstOrDefault() is not RegexAttribute attribute) continue; // skip function if it doesn't have attr

                var matches = Regex.Matches(line, attribute.Pattern); // match this constructor against the input line

                if (!matches.Any()) continue; // Try other constructors to see if they match

                var paramInfo = func.GetParameters();

                object[] convertedParams = ConstructParams(matches, paramInfo);

                return (T)func.Invoke(factory, convertedParams);

            }
            throw new Exception($"RegexFactory failed to find suitable factory function for {line}");
        }

        public static IEnumerable<T> RegexFactory<T>(IEnumerable<string> input, object factory) => 
            input.Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(line => RegexFactoryCreate<T>(line, factory));

        public static IEnumerable<T> RegexFactory<T>(string input, object factory, string splitter = "\n") =>
            RegexFactory<T>(input.Split(splitter), factory);


        public static int[] Parse32(string input, char splitChar = '\0') => Parse32(Split(input, splitChar));
        public static uint[] ParseU32(string input, char splitChar = '\0') => ParseU32(Split(input, splitChar));
        public static Int64[] Parse64(string input, char splitChar = '\0') => Parse64(Split(input, splitChar));
        public static UInt64[] ParseU64(string input, char splitChar = '\0') => ParseU64(Split(input, splitChar));

        public static int[] Parse32(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(Int32.Parse).ToArray();
        public static uint[] ParseU32(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(UInt32.Parse).ToArray();
        public static Int64[] Parse64(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(Int64.Parse).ToArray();
        public static UInt64[] ParseU64(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(UInt64.Parse).ToArray();

        interface IConvertomatic
        {
            public abstract object Convert(char c);
            public abstract bool ShouldConvert(char c);
        }

        class ConvertInt : IConvertomatic
        {
            public object Convert(char c) => c - '0';

            public bool ShouldConvert(char c) => true;
        }
        class ConvertByte : IConvertomatic
        {
            public object Convert(char c) => (byte)(c - '0');
            public bool ShouldConvert(char c) => true;
        }
        class ConvertBool : IConvertomatic
        {
            public object Convert(char c) => c == '#';
            public bool ShouldConvert(char c) => c == '#';
        }
        class ConvertChar : IConvertomatic
        {
            public object Convert(char c) => c;
            public bool ShouldConvert(char c) => true;
        }


        static IConvertomatic GetConverter<T>()
        {
            if (typeof(T) == typeof(int)) return new ConvertInt();
            if (typeof(T) == typeof(byte)) return new ConvertByte();
            if (typeof(T) == typeof(bool)) return new ConvertBool();
            if (typeof(T) == typeof(char)) return new ConvertChar();

            throw new NotImplementedException(typeof(T).FullName);
        }

        public static T[,] ParseMatrix<T>(string input) => ParseMatrix<T>(Util.Split(input));

        public static T[,] ParseMatrix<T>(IEnumerable<string> input)
        {
            int height = input.Count();
            int width = input.First().Length;
            var mtx = new T[width, height];

            var converter = GetConverter<T>();

            input.WithIndex().ForEach(line => line.Value.WithIndex().Where(ch => converter.ShouldConvert(ch.Value)).ForEach(ch => mtx[ch.Index, line.Index] = (T)converter.Convert(ch.Value)));

            return mtx;
        }

        public static Dictionary<(int x, int y), T> ParseSparseMatrix<T>(string input) => ParseSparseMatrix<T>(Util.Split(input));

        public static Dictionary<(int x, int y), T> ParseSparseMatrix<T>(IEnumerable<string> input)
        {
            int height = input.Count();
            int width = input.First().Length;
            var dic = new Dictionary<(int x, int y), T>();

            var converter = GetConverter<T>();

            input.WithIndex().ForEach(line => line.Value.WithIndex().Where(ch => converter.ShouldConvert(ch.Value)).ForEach(ch => dic[(ch.Index, line.Index)] = (T)converter.Convert(ch.Value)));

            return dic;
        }

        public static IEnumerable<IEnumerable<T>> Slice<T>(IEnumerable<T> source, int sliceSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / sliceSize)
                .Select(x => x.Select(v => v.Value));
        }

        public static IEnumerable<(T1 item1, T2 item2)> Matrix<T1, T2>(IEnumerable<T1> set1, IEnumerable<T2> set2)
        {
            foreach (T1 x in set1)
            {
                foreach (T2 y in set2)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int x, int y)> Range2DInclusive((int minY, int maxY, int minX, int maxX) range)
        {
            for (int y = range.minY; y <= range.maxY; ++y)
            {
                for (int x = range.minX; x <= range.maxX; ++x)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int x, int y, int z)> Range3DInclusive((int minZ, int maxZ, int minY, int maxY, int minX, int maxX) range)
        {
            for (int z = range.minZ; z <= range.maxZ; ++z)
            {
                for (int y = range.minY; y <= range.maxY; ++y)
                {
                    for (int x = range.minX; x <= range.maxX; ++x)
                    {
                        yield return (x, y, z);
                    }
                }
            }
        }

        public static IEnumerable<int> RangeBetween(int start, int end)
        {
            for (int i = start; i < end; i++) yield return i;
        }

        public static IEnumerable<int> RangeInclusive(int start, int end)
        {
            for (int i = start; i <= end; i++) yield return i;
        }

        public static IEnumerable<(int x, int y)> Matrix(int maxX, int maxY) => Matrix<int, int>(Enumerable.Range(0, maxX), Enumerable.Range(0, maxY));

        public static string GetInput(IPuzzle puzzle) => System.IO.File.ReadAllText(System.IO.Path.Combine("Data", puzzle.Name + ".txt")).Replace("\r", "");

        public static string GetInput<T>() where T : IPuzzle, new() => GetInput(new T());

        public static IEnumerable<int> Forever(int start = 0) => Enumerable.Range(start, int.MaxValue - start);

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

        public static IEnumerable<T> Repeat<T>(Func<T> generator, int count)
        {
            for (int i = 0; i < count; ++i) yield return generator();
        }

        public static IEnumerable<T> RepeatWhile<T>(Func<T> generator, Func<T, bool> shouldContinue)
        {
            bool cont;
            do
            {
                T v = generator();
                yield return v;
                cont = shouldContinue(v);
            } while (cont);
        }

        public static IEnumerable<int> DuplicateDigits(IEnumerable<int> input, int repeats)
        {
            foreach (var i in input)
            {
                for (int j = 0; j < repeats; ++j)
                {
                    yield return i;
                }
            }
        }

        public static IEnumerable<T> Values<T>(params T[] input)
        {
            return input;
        }

        public static void Test<T>(T actual, T expected)
        {
            if (!EqualityComparer<T>.Default.Equals(actual, expected))
            {
                throw new Exception($"Expected {expected} but got {actual}");
            }
            Console.WriteLine(actual);
        }

        public static int[] ExtractNumbers(IEnumerable<char> input) => input.Where(c => (c == ' ' || c == '-' || (c >= '0' && c <= '9'))).AsString().Trim().Split(" ").Where(w => !string.IsNullOrEmpty(w)).Select(int.Parse).ToArray();

        public static long[] ExtractLongNumbers(IEnumerable<char> input) => input.Where(c => (c == ' ' || c == '-' || (c >= '0' && c <= '9'))).AsString().Trim().Split(" ").Where(w => !string.IsNullOrEmpty(w)).Select(long.Parse).ToArray();


        public static void SetBit(ref Int64 value, int i)
        {
            value |= (1L) << i;
        }

        public static void ClearBit(ref Int64 value, int i)
        {
            value &= ~(1L << i);
        }

        public static (TInput input, TResult result) BinarySearch<TInput, TResult>(TInput start, Func<TInput, (bool success, TResult res)> test) where TInput : IBinaryInteger<TInput>
            => BinarySearch(start, TInput.Zero, test);

        public static (TInput input, TResult result) BinarySearch<TInput, TResult>(TInput min, TInput max, Func<TInput, (bool success, TResult res)> test) where TInput : IBinaryInteger<TInput>
        {
            TInput two = TInput.One + TInput.One;
            TInput three = TInput.One + TInput.One + TInput.One;

            Dictionary<TInput, TResult> results = new();

            if (max == TInput.Zero)
            {
                max = min + TInput.One;
                bool success = false;

                while (!success)
                {
                    var res = test(max);
                    success = res.success;

                    if (success)
                    {
                        results[max] = res.res;
                    }
                    else
                    {
                        min = max;
                        max *= three;
                    }
                }
            }

            while (max - min > TInput.One)
            {
                var mid = (max + min) / two;
                var res = test(mid);
                if (res.success)
                {
                    max = mid;
                    results[mid] = res.res;
                }
                else
                {
                    min = mid;
                }
            }
            return (max, results[max]);
        }

        public static int Log2i(ulong num) => num switch
        {
            1 => 0,
            2 => 1,
            4 => 2,
            8 => 3,
            16 => 4,
            32 => 5,
            64 => 6,
            128 => 7,
            256 => 8,
            512 => 9,
            1024 => 10,
            2048 => 11,
            4096 => 12,
            8192 => 13,
            16384 => 14,
            32768 => 15,
            65536 => 16,
            131072 => 17,
            262144 => 18,
            524288 => 19,
            1048576 => 20,
            2097152 => 21,
            4194304 => 22,
            8388608 => 23,
            16777216 => 24,
            33554432 => 25,
            67108864 => 26,
            134217728 => 27,
            268435456 => 28,
            536870912 => 29,
            1073741824 => 30,
            2147483648 => 31,
            4294967296 => 32,
            8589934592 => 33,
            17179869184 => 34,
            34359738368 => 35,
            68719476736 => 36,
            137438953472 => 37,
            274877906944 => 38,
            549755813888 => 39,
            1099511627776 => 40,
            2199023255552 => 41,
            4398046511104 => 42,
            8796093022208 => 43,
            17592186044416 => 44,
            35184372088832 => 45,
            70368744177664 => 46,
            140737488355328 => 47,
            281474976710656 => 48,
            562949953421312 => 49,
            1125899906842624 => 50,
            2251799813685248 => 51,
            4503599627370496 => 52,
            9007199254740992 => 53,
            18014398509481984 => 54,
            36028797018963968 => 55,
            72057594037927936 => 56,
            144115188075855872 => 57,
            288230376151711744 => 58,
            576460752303423488 => 59,
            1152921504606846976 => 60,
            2305843009213693952 => 61,
            4611686018427387904 => 62,
            9223372036854775808 => 63,
            _ => throw new ArgumentException("input out of range")
        };

        public static string FormatMs(long ms)
        {
            var span = TimeSpan.FromMilliseconds(ms);
            if (ms < 1000)
            {
                return $"     .{span:fff}";
            }
            else
            {

                if ((int)span.TotalHours > 0)
                {
                    return span.ToString(@"hh\:mm\:ss.fff");
                }
                if ((int)span.TotalMinutes > 0)
                {
                    return $"{span:mm\\:ss\\.fff}";
                }
                else
                {
                    return $"   {span:ss\\.fff}";
                }
            }
        }
    }


    public class AutoArray<DataType> : IEnumerable<DataType>
    {
        DataType[] data;

        public AutoArray(IEnumerable<DataType> input) => data = input.ToArray();

        DataType Get(int key)
        {
            if (key >= data.Length) return default;
            return data[key];
        }

        void Set(int key, DataType value)
        {
            if (key >= data.Length)
            {
                Console.Write($"Resize from {data.Length} to ");
                Reserve(data.Length + ((key - data.Length + 1) * 250));
                Console.WriteLine($"{data.Length}");
            }
            data[key] = value;
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
        readonly StringBuilder builder = new();
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
            return $"{baseStr}{num}".GetMD5();
        }

        public static IEnumerable<char> GetHashChars(int num, string baseStr)
        {
            return $"{baseStr}{num}".GetMD5Chars();
        }

        static bool IsHash(int num, string baseStr, int numZeroes)
        {
            return GetHash(num, baseStr).AsNybbles().Take(numZeroes).All(v => v == 0);
        }

        public static int FindHash(string baseStr, int numZeroes, int start = 0)
        {
            return Util.Forever(start).Where(n => IsHash(n, baseStr, numZeroes)).First();
        }
    }

    public class TimeLogger : ILogger
    {
        readonly System.Diagnostics.Stopwatch sw = new();
        readonly System.IO.TextWriter output;

        public TimeLogger(System.IO.TextWriter tw)
        {
            output = tw;
            sw.Start();
        }

        public void WriteLine(string log = null)
        {
            if (log != null)
            {
                output.Write($"[{Util.FormatMs(sw.ElapsedMilliseconds)}] ");
                output.WriteLine(log);
            }
            else
            {
                output.WriteLine();
            }
        }

        public override string ToString()
        {
            return output.ToString();
        }
    }

    public class ConsoleOut : TimeLogger
    {
        public ConsoleOut() : base(Console.Out)
        {
        }
    }

    public class Accumulator
    {
        public Accumulator(Int64 initial)
        {
            Min = initial;
            Max = initial;
            Sum = initial;
        }

        public Accumulator()
        {
            Min = Int64.MaxValue;
            Max = Int64.MinValue;
            Sum = 0;
        }

        public void Reset()
        {
            Min = Int64.MaxValue;
            Max = Int64.MinValue;
            Sum = 0;
        }

        public void Add(Int64 val)
        {
            Sum += val;
            Min = Math.Min(Min, val);
            Max = Math.Max(Max, val);
        }

        public IEnumerable<Int64> RangeInclusive()
        {
            for (var i = Min; i <= Max; ++i) yield return i;
        }

        public IEnumerable<Int64> RangeBuffered(Int64 buffer)
        {
            for (var i = Min - buffer; i <= Max + buffer; ++i) yield return i;
        }


        public Int64 Min { get; private set; }
        public Int64 Max { get; private set; }
        public Int64 Sum { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
    public class RegexAttribute : Attribute
    {
        public RegexAttribute(string pattern)
        {
            Pattern = pattern;
        }
        public string Pattern { get; private set; }
    }

    public class Boxed<T>
    {
        readonly T value;
        public Boxed(T v) => value = v;
        public static implicit operator T(Boxed<T> boxed) => boxed.value;
        public static implicit operator Boxed<T>(T value) => new(value);
    }

}
