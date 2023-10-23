using AoC.Utils;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
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
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => typeof(IPuzzle).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (IPuzzle)Activator.CreateInstance(x))
                .Where(p => p.ShouldRun())
                .OrderBy(x => x.Name).ToArray();
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

        internal static int WrapIndex(int v, int length) => v % length;

        public static T Create<T>(string line) => (T)Activator.CreateInstance(typeof(T), new object[] { line });

        public static List<T> Parse<T>(IEnumerable<string> input) => input.Select(Create<T>).ToList();

        public static List<T> Parse<T, C>(string input, C cache, string splitter = "\n")
        {
            return Parse<T, C>(input.Split(splitter)
                                 .WithoutNullOrWhiteSpace(), cache);
        }

        public static List<T> Parse<T, C>(IEnumerable<string> input, C cache)
        {
            return input.Select(line => (T)Activator.CreateInstance(typeof(T), new object[] { line, cache }))
                        .ToList();
        }

        public static List<T> Parse<T>(string input, string splitter = "\n")
        {
            return Parse<T>(input.Split(splitter)
                                 .WithoutNullOrWhiteSpace());
        }

        public static IEnumerable<(ConstructorInfo tc, RegexAttribute attr)> EnumeratePotentialConstructors<T>()
        {
            return EnumeratePotentialConstructors(typeof(T));
        }

        public static IEnumerable<(ConstructorInfo tc, RegexAttribute attr)> EnumeratePotentialConstructors(Type t)
        {
            if (t.GetCustomAttribute(typeof(RegexAttribute)) is RegexAttribute ownAttr)
            {
                var typeConstructor = t.GetConstructors().First();
                yield return (typeConstructor, ownAttr);
            }
            else
            {
                foreach (var typeConstructor in t.GetConstructors())
                {
                    if (typeConstructor?.GetCustomAttributes(typeof(RegexAttribute), true)
                        .FirstOrDefault() is not RegexAttribute attribute) continue; // skip constructor if it doesn't have attr

                    yield return (typeConstructor, attribute);
                }
            }
        }

        public static T RegexCreate<T>(string line, (ConstructorInfo tc, RegexAttribute attr)[] potentialConstructors = null)
        {
            potentialConstructors ??= EnumeratePotentialConstructors<T>().ToArray();
            foreach (var (tc, attr) in potentialConstructors)
            {
                if (RegexCreateInternal(line, tc, attr, out T result)) return result;
            }
            throw new Exception($"RegexParse failed to find suitable constructor for '{line}' in {typeof(T).Name}");
        }

        public static object RegexCreate(Type t, string line, (ConstructorInfo tc, RegexAttribute attr)[] potentialConstructors = null)
        {
            potentialConstructors ??= EnumeratePotentialConstructors(t).ToArray();
            foreach (var (tc, attr) in potentialConstructors)
            {
                if (RegexCreateInternal(t, line, tc, attr, out object result)) return result;
            }
            throw new Exception($"RegexParse failed to find suitable constructor for '{line}' in {t.Name}");
        }

        static bool RegexCreateInternal(Type t, string line, ConstructorInfo typeConstructor, RegexAttribute attr, out object result)
        {
            var matches = attr.Regex.Matches(line); // match this constructor against the input line

            if (matches.Count == 0)
            {
                result = default;
                return false;
            }// Try other constructors to see if they match

            var paramInfo = typeConstructor.GetParameters();

            object[] convertedParams = ConstructParams(matches, paramInfo);
            result = Activator.CreateInstance(t, convertedParams);

            return true;
        }

        static bool RegexCreateInternal<T>(string line, ConstructorInfo typeConstructor, RegexAttribute attr, out T result)
        {
            var matches = attr.Regex.Matches(line); // match this constructor against the input line

            if (matches.Count == 0)
            {
                result = default;
                return false;
            }// Try other constructors to see if they match

            var paramInfo = typeConstructor.GetParameters();

            object[] convertedParams = ConstructParams(matches, paramInfo);
            result = (T)Activator.CreateInstance(typeof(T), convertedParams);

            return true;
        }


        private static object ConvertFromString(Type type, string input)
        {
            if (type.IsArray)
            {
                var elementType = type.GetElementType();

                if (elementType == typeof(string)) return Split(input);
                if (elementType == typeof(int)) return ParseNumbers<int>(input).ToArray();
                if (elementType == typeof(long)) return ParseNumbers<long>(input).ToArray();

                if (elementType.IsClass)
                {
                    var arr = RegexParse(elementType, Split(input)).ToList();
                    var dat = arr.Select(o => Convert.ChangeType(o, elementType)).ToArray();
                    return Convert.ChangeType(arr, type);
                }

                throw new Exception("Can't convert array type " + type);
            }
            else
            {

                if (type == typeof(string)) return input;
                if (type == typeof(int)) return int.Parse(input);
                if (type == typeof(long)) return long.Parse(input);

                var conv = TypeDescriptor.GetConverter(type);
                if (conv.CanConvertFrom(typeof(string))) return conv.ConvertFromString(input);

                return RegexCreate(type, input);
            }
        }

        private static object[] ConstructParams(MatchCollection matches, ParameterInfo[] paramInfo)
        {
            var regexValues = matches[0]
                .Groups.Values.Where(v => !string.IsNullOrWhiteSpace(v.Value))
                .Skip(1).Select(g => g.Value).ToArray();

            if (regexValues.Length != paramInfo.Length) throw new Exception("RegexParse couldn't match constructor param count");

            var instanceParams = Enumerable.Zip(paramInfo, regexValues); // collate parameter types and matched substrings

            return instanceParams
                .Select(kvp => ConvertFromString(kvp.First.ParameterType, kvp.Second)).ToArray(); // convert substrings to match constructor input
        }

        public static IEnumerable<T> RegexParse<T>(IEnumerable<string> input)
        {
            (ConstructorInfo tc, RegexAttribute attr)[] constructors = EnumeratePotentialConstructors<T>().ToArray();
            return input.WithoutNullOrWhiteSpace()
                 .Select(line => RegexCreate<T>(line, constructors));//.ToList();
        }

        public static IEnumerable<object> RegexParse(Type t, IEnumerable<string> input)
        {
            (ConstructorInfo tc, RegexAttribute attr)[] constructors = EnumeratePotentialConstructors(t).ToArray();
            return input.WithoutNullOrWhiteSpace()
                 .Select(line => RegexCreate(t, line, constructors));//.ToList();
        }

        public static IEnumerable<T> RegexParse<T>(string input, string splitter = "\n") =>
            RegexParse<T>(input.Split(splitter));


        public static T RegexFactoryCreate<T, FT>(string line, FT factory) where FT : class
        {
            foreach (var func in typeof(FT).GetMethods())
            {
                if (func.GetCustomAttributes(typeof(RegexAttribute), true)
                    .FirstOrDefault() is not RegexAttribute attribute) continue; // skip function if it doesn't have attr

                var matches = attribute.Regex.Matches(line); // match this constructor against the input line

                if (matches.Count == 0) continue; // Try other constructors to see if they match

                var paramInfo = func.GetParameters();

                object[] convertedParams = ConstructParams(matches, paramInfo);

                return (T)func.Invoke(factory, convertedParams);

            }
            throw new Exception($"RegexFactory failed to find suitable factory function for {line}");
        }

        public static void RegexFactoryPerform<FT>(string line, FT factory) where FT : class
        {
            foreach (var func in typeof(FT).GetMethods())
            {
                if (func.GetCustomAttributes(typeof(RegexAttribute), true)
                    .FirstOrDefault() is not RegexAttribute attribute) continue; // skip function if it doesn't have attr

                var matches = attribute.Regex.Matches(line); // match this constructor against the input line

                if (matches.Count == 0) continue; // Try other constructors to see if they match

                var paramInfo = func.GetParameters();

                object[] convertedParams = ConstructParams(matches, paramInfo);

                func.Invoke(factory, convertedParams);
                return;

            }
            throw new Exception($"RegexFactory failed to find suitable factory function for {line}");
        }

        public static IEnumerable<T> RegexFactory<T, FT>(IEnumerable<string> input, FT factory) where FT : class =>
            input.WithoutNullOrWhiteSpace()
                 .Select(line => RegexFactoryCreate<T, FT>(line, factory));

        public static IEnumerable<T> RegexFactory<T, FT>(string input, FT factory = null, string splitter = "\n") where FT : class =>
            RegexFactory<T, FT>(input.Split(splitter), factory);


        public static FT RegexFactory<FT>(string input, string splitter = "\n") where FT : class, new()
        {
            FT factory = new();

            foreach (var line in input.Split(splitter).WithoutNullOrWhiteSpace())
            {
                RegexFactoryPerform(line, factory);
            }

            return factory;
        }

        public static List<T> CreateMultiple<T>(int count) where T : new() => Enumerable.Repeat(0, count).Select(_ => new T()).ToList();

        public static T[] ParseNumbers<T>(string input, char splitChar = '\0', System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Any) where T : INumberBase<T>
            => ParseNumbers<T>(Split(input, splitChar), style);

        public static T[] ParseNumbers<T>(IEnumerable<string> input, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Any) where T : INumberBase<T>
            => input.WithoutNullOrWhiteSpace().Select(line => T.Parse(line, style, null)).ToArray();



        public interface IConvertomatic
        {
            public abstract object Convert(char c);
            public abstract bool ShouldConvert(char c);
        }

        public static class Convertomatic
        {
            public class ConvertInt : IConvertomatic
            {
                public object Convert(char c) => c - '0';

                public bool ShouldConvert(char c) => true;
            }
            public class ConvertByte : IConvertomatic
            {
                public object Convert(char c) => (byte)(c - '0');
                public bool ShouldConvert(char c) => true;
            }
            public class ConvertBool : IConvertomatic
            {
                public object Convert(char c) => c == '#';
                public bool ShouldConvert(char c) => c == '#';
            }
            public class ConvertChar : IConvertomatic
            {
                public object Convert(char c) => c;
                public bool ShouldConvert(char c) => true;
            }

            public class SkipSpaces : IConvertomatic
            {
                readonly char toSkip;
                public SkipSpaces(char ch = ' ') => toSkip = ch;
                public object Convert(char c) => c;
                public bool ShouldConvert(char c) => c != toSkip;
            }
        }


        static IConvertomatic GetConverter<T>()
        {
            if (typeof(T) == typeof(int)) return new Convertomatic.ConvertInt();
            if (typeof(T) == typeof(byte)) return new Convertomatic.ConvertByte();
            if (typeof(T) == typeof(bool)) return new Convertomatic.ConvertBool();
            if (typeof(T) == typeof(char)) return new Convertomatic.ConvertChar();

            throw new NotImplementedException(typeof(T).FullName);
        }

        public static T[,] ParseMatrix<T>(string input) => ParseMatrix<T>(Split(input));

        public static T[,] ParseMatrix<T>(IEnumerable<string> input)
        {
            int height = input.Count();
            int width = input.First().Length;
            var mtx = new T[width, height];

            var converter = GetConverter<T>();

            input.WithIndex().ForEach(line => line.Value.WithIndex().Where(ch => converter.ShouldConvert(ch.Value)).ForEach(ch => mtx[ch.Index, line.Index] = (T)converter.Convert(ch.Value)));

            return mtx;
        }

        public static Dictionary<(int x, int y), T> ParseSparseMatrix<T>(string input, IConvertomatic converter = null) => ParseSparseMatrix<T>(Split(input), converter);

        public static Dictionary<(int x, int y), T> ParseSparseMatrix<T>(IEnumerable<string> input, IConvertomatic converter = null)
        {
            int height = input.Count();
            int width = input.First().Length;
            var dic = new Dictionary<(int x, int y), T>();

            converter ??= GetConverter<T>();

            input.WithIndex().ForEach(line => line.Value.WithIndex().Where(ch => converter.ShouldConvert(ch.Value)).ForEach(ch => dic[(ch.Index, line.Index)] = (T)converter.Convert(ch.Value)));

            return dic;
        }

        public static IEnumerable<(T1 item1, T2 item2)> Matrix<T1, T2>(IEnumerable<T1> set1, IEnumerable<T2> set2)
        {
            return set1.SelectMany(x => set2.Select(y => (x, y)));
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

        public static IEnumerable<(int x, int y)> Range2DExclusive((int minY, int maxY, int minX, int maxX) range)
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

        public static IEnumerable<int> RangeBetween(int start, int end) => Enumerable.Range(start, end - start);

        public static IEnumerable<int> RangeInclusive(int start, int end) => Enumerable.Range(start, end - start + 1);

        public static IEnumerable<(int x, int y)> Matrix(int maxX, int maxY) => Matrix(Enumerable.Range(0, maxX), Enumerable.Range(0, maxY));

        static string sessionCookie = null;
        static object DataLock = new object();

        public static string Download(string url)
        {
            sessionCookie ??= File.ReadAllText(".session");

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", sessionCookie));
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler);

            var content = client.GetStringAsync(url).Result;
            return content;
        }

        public static string GetInput(IPuzzle puzzle)
        {
            lock (DataLock)
            {
                var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AoCData");
                if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
                var expectedFile = Path.Combine(dataPath, puzzle.Name + ".txt");

                Console.WriteLine(expectedFile);

                if (!File.Exists(expectedFile))
                {
                    string puzzleData = Download(puzzle.DataURL());
                    File.WriteAllText(expectedFile, puzzleData);
                    return puzzleData;
                }

                return File.ReadAllText(expectedFile).Replace("\r", "");
            }
        }

        public static string GetInput<T>() where T : IPuzzle, new() => GetInput(new T());

        public static IEnumerable<int> Forever(int start = 0) => Enumerable.Range(start, int.MaxValue - start);


        public static IEnumerable<T> Repeat<T>(Func<T> generator, int count)
        {
            for (int i = 0; i < count; ++i) yield return generator();
        }

        public static IEnumerable<X> For<T, X>(T start, T end, T step, Func<T, X> action) where T : IBinaryInteger<T>
        {
            for (T i = start; i < end; i += step) yield return action(i);
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

        public static T[] Values<T>(params T[] input) => input;

        public static int CountTrue(params bool[] input) => input.Count(v => v);

        public static int CountTrue(Func<int, bool> check, Action action = null)
        {
            int i = 0;
            while (check(i))
            {
                action?.Invoke();
                i++;
            }
            return i;
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


        public static void SetBit(ref long value, int i) => value |= (1L) << i;

        public static void ClearBit(ref long value, int i) => value &= ~(1L << i);

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

        public static IEnumerable<(T1, T2)> Combinations<T1, T2>(IEnumerable<T1> set1, IEnumerable<T2> set2)
        {
            foreach (var item1 in set1)
                foreach (var item2 in set2)
                    yield return (item1, item2);
        }

        public static ushort MakeTwoCC(string name) => name.Length == 1 ? (ushort)(name[0]) : (ushort)(name[0] + (name[1] << 8));
        public static ushort MakeTwoCC(char c1, char c2) => (ushort)(c1 + (c2 << 8));

        public static uint MakeFourCC(string name) => name[0] + ((uint)name[1] << 8) + ((uint)name[2] << 16) + ((uint)name[3] << 24);

        public static (T min, T max) MinMax<T>(params T[] input) where T : IBinaryInteger<T>
        {
            return (input.Min(), input.Max());
        }

        public static T GCD<T>(T a, T b) where T : IBinaryInteger<T>
        {
            while (b != T.Zero)
            {
                (a, b) = (b, a % b);
            }
            return a;
        }

        public static T LCM<T>(T a, T b) where T : IBinaryInteger<T> => a * b / GCD(a, b);

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
            get => Encoding.UTF8;
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }

    public static class HashBreaker
    {
        public static (int foundIndex, IEnumerable<char> foundHash) FindHash(string baseStr, int numZeroes, int start = 0)
        {
            if (numZeroes > 6 || numZeroes < 5) throw new NotImplementedException("numZeroes currently only 5 or 6");
            byte[] inputBuffer = Encoding.Default.GetBytes($"{baseStr}{start}");
            byte[] outputBuffer = GC.AllocateUninitializedArray<byte>(MD5.HashSizeInBytes);
            var inputSpan = new ReadOnlySpan<byte>(inputBuffer);
            var outputSpan = outputBuffer.AsSpan();

            int index = start;
            int digits = index == 0 ? 0 : (int)Math.Log10(index);
            while (true)
            {
                MD5.TryHashData(inputSpan, outputSpan, out var _);

                //if (result.AsNybbles().Take(numZeroes).All(v => v == 0)) return index;
                if (outputBuffer[0] == 0 && outputBuffer[1] == 0 && (numZeroes == 5 && ((outputBuffer[2] & 0xf0) == 0) || numZeroes == 6 && outputBuffer[2] == 0)) return (index, outputBuffer.GetHexChars());

                index++;
                int newDigits = (int)Math.Log10(index);
                if (newDigits != digits)
                {
                    digits = newDigits;
                    inputBuffer = Encoding.Default.GetBytes($"{baseStr}{index}");
                    inputSpan = new ReadOnlySpan<byte>(inputBuffer);
                }
                else
                {
                    int j = inputBuffer.Length - 1;
                    while (++inputBuffer[j] > '9')
                    {
                        inputBuffer[j--] = (byte)'0';
                    }
                }
            }
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

    public class Accumulator2D<T> where T : INumber<T>, IMinMaxValue<T>
    {
        public Accumulator<T> X = new();
        public Accumulator<T> Y = new();

        public void Reset()
        {
            X.Reset();
            Y.Reset();
        }

        public void Add(T x, T y)
        {
            X.Add(x);
            Y.Add(y);
        }

        public IEnumerable<(T x, T y)> RangeBuffered(T buffer)
        {
            foreach (var y in Y.RangeBuffered(buffer))
                foreach (var x in X.RangeBuffered(buffer))
                    yield return (x, y);
        }

    }

    public class Accumulator<T> where T : INumber<T>, IMinMaxValue<T>
    {
        public Accumulator(T initial)
        {
            Min = initial;
            Max = initial;
            Sum = initial;
        }

        public Accumulator() => Reset();

        public void Reset()
        {
            Min = T.MaxValue;
            Max = T.MinValue;
            Sum = T.Zero;
        }

        public void Add(T val)
        {
            Sum += val;
            Min = T.Min(Min, val);
            Max = T.Max(Max, val);
        }

        public IEnumerable<T> RangeInclusive()
        {
            for (var i = Min; i <= Max; ++i) yield return i;
        }

        public IEnumerable<T> RangeBuffered(T buffer)
        {
            for (var i = Min - buffer; i <= Max + buffer; ++i) yield return i;
        }

        public T Min { get; private set; }
        public T Max { get; private set; }
        public T Sum { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct)]
    public class RegexAttribute : Attribute
    {
        public RegexAttribute(string pattern)
        {
            if (cache.TryGetValue(pattern, out var regex)) Regex = regex;
            else
            {
                Regex = new Regex(pattern);
                cache.TryAdd(pattern, Regex);
            }
        }
        public Regex Regex { get; private set; }

        static readonly ConcurrentDictionary<string, Regex> cache = new();

    }

    public class Boxed<T>
    {
        readonly T value;
        public Boxed(T v) => value = v;
        public static implicit operator T(Boxed<T> boxed) => boxed.value;
        public static implicit operator Boxed<T>(T value) => new(value);

        public override string ToString() => value.ToString();
    }

    public static class Extensions
    {
        public static IEnumerable<TOut> While<TIn, TOut>(this TIn val, Func<TIn, bool> cont, Func<TIn, (TIn, TOut)> func)
        {
            while (cont(val))
            {
                (val, var res) = func(val);
                yield return res;
            }
        }
    }

}
