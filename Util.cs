using AoC.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public static List<T> Parse<T>(IEnumerable<string> input)
        {
            return input.Select(line => (T)Activator.CreateInstance(typeof(T), new object[] { line }))
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

        static T RegexCreate<T>(string line)
        {
            foreach (var typeConstructor in typeof(T).GetConstructors())
            {
                if (!(typeConstructor?.GetCustomAttributes(typeof(RegexAttribute), true)
                    .FirstOrDefault() is RegexAttribute attribute)) continue;

                try
                {
                    return (T)Activator.CreateInstance
                    (
                        typeof(T), Enumerable.Zip(
                            typeConstructor.GetParameters(),
                            Regex.Matches(line, attribute.Pattern)[0]
                                .Groups.Values.Where(v => !string.IsNullOrWhiteSpace(v.Value))
                                .Skip(1).Select(g => g.Value)
                        )
                        .Select(kvp => TypeDescriptor.GetConverter(kvp.First.ParameterType).ConvertFromString(kvp.Second)).ToArray()
                    );
                }
                catch { }
            }
            return default(T);
        }

        public static IEnumerable<T> RegexParse<T>(IEnumerable<string> input) =>
            input.Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(line => RegexCreate<T>(line));

        public static IEnumerable<T> RegexParse<T>(string input, string splitter = "\n") =>
            RegexParse<T>(input.Split(splitter));


        public static int[] Parse32(string input, char splitChar = '\0') => Parse32(Split(input, splitChar));
        public static uint[] ParseU32(string input, char splitChar = '\0') => ParseU32(Split(input, splitChar));
        public static Int64[] Parse64(string input, char splitChar = '\0') => Parse64(Split(input, splitChar));
        public static UInt64[] ParseU64(string input, char splitChar = '\0') => ParseU64(Split(input, splitChar));

        public static int[] Parse32(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();
        public static uint[] ParseU32(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => UInt32.Parse(s)).ToArray();
        public static Int64[] Parse64(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int64.Parse(s)).ToArray();
        public static UInt64[] ParseU64(IEnumerable<string> input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => UInt64.Parse(s)).ToArray();

        interface Convertomatic
        {
            public abstract object Convert(char c);
        }

        class ConvertInt : Convertomatic
        {
            public object Convert(char c) => c - '0';
        }
        class ConvertByte : Convertomatic
        {
            public object Convert(char c) => (byte)(c - '0');
        }

        static Convertomatic GetConverter<T>()
        {
            if (typeof(T) == typeof(int)) return new ConvertInt();
            if (typeof(T) == typeof(byte)) return new ConvertByte();

            throw new NotImplementedException(typeof(T).FullName);
        }

        public static T[,] ParseMatrix<T>(string input) => ParseMatrix<T>(Util.Split(input));

        public static T[,] ParseMatrix<T>(IEnumerable<string> input)
        {
            int height = input.Count();
            int width = input.First().Count();
            var mtx = new T[width, height];

            var converter = GetConverter<T>();

            input.WithIndex().ForEach(line => line.Value.WithIndex().ForEach(ch => mtx[ch.Index, line.Index] = (T)converter.Convert(ch.Value)));

            return mtx;
        }

        public static Dictionary<(int x, int y), T> ParseSparseMatrix<T>(string input) => ParseSparseMatrix<T>(Util.Split(input));

        public static Dictionary<(int x, int y),T> ParseSparseMatrix<T>(IEnumerable<string> input)
        {
            int height = input.Count();
            int width = input.First().Count();
            var dic = new Dictionary<(int x, int y), T>();

            var converter = GetConverter<T>();

            input.WithIndex().ForEach(line => line.Value.WithIndex().ForEach(ch => dic[(ch.Index, line.Index)] = (T)converter.Convert(ch.Value)));

            return dic;
        }

        public static IEnumerable<IEnumerable<T>> Slice<T>(IEnumerable<T> source, int sliceSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / sliceSize)
                .Select(x => x.Select(v => v.Value));
        }

        public static IEnumerable<(T1 x, T2 y)> Matrix<T1, T2>(IEnumerable<T1> set1, IEnumerable<T2> set2)
        {
            foreach (T1 x in set1)
            {
                foreach (T2 y in set2)
                {
                    yield return (x, y);
                }
            }
        }

        public static IEnumerable<(int x, int y)> Matrix(int maxX, int maxY) => Matrix<int, int>(Enumerable.Range(0, maxX), Enumerable.Range(0, maxY));

        public static string GetInput(IPuzzle puzzle) => System.IO.File.ReadAllText(System.IO.Path.Combine("Data", puzzle.Name + ".txt")).Replace("\r", "");

        public static string GetInput<T>() where T : IPuzzle, new() => GetInput(new T());

        public static IEnumerable<int> Forever(int start = 0) => Enumerable.Range(start, int.MaxValue);

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
            for(int i=0; i<count; ++i) yield return generator();
        }

        public static IEnumerable<T> RepeatWhile<T>(Func<T> generator, Func<T,bool> shouldContinue)
        {
            bool cont = true;
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

        public static int[] ExtractNumbers(IEnumerable<char> input) => input.Where(c => (c == ' ' || c == '-' || (c >= '0' && c <= '9'))).AsString().Trim().Split(" ").Where(w => !string.IsNullOrEmpty(w)).Select(w => int.Parse(w)).ToArray();

        public static void SetBit(ref Int64 value, int i)
        {
            value |= (1L) << i;
        }

        public static void ClearBit(ref Int64 value, int i)
        {
            value &= ~(1L << i);
        }

        public static uint BinarySearch(uint min, uint max, Func<uint, bool> test)
        {
            while (max - min > 1)
            {
                var mid = (max + min) / 2;
                if (test(mid))
                {
                    max = mid;
                }
                else
                {
                    min = mid;
                }
            }
            return max;
        }
    }


    public class AutoArray<DataType> : IEnumerable<DataType>
    {
        DataType[] data;

        public AutoArray(IEnumerable<DataType> input) => data = input.ToArray();

        DataType Get(int key)
        {
            if (key >= data.Length) return default(DataType);
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
            return hashed.Where(b => b != 0).Count() == 0;
        }

        const int blockSize = 100000;
        public static int FindHash(string baseStr, int numZeroes, int start = 0)
        {
            while (true)
            {
                var hashes = Enumerable.Range(start, blockSize).Where(n => IsHash(n, baseStr, numZeroes));
                if (hashes.Any())
                {
                    return hashes.First();
                }

                start += blockSize;
            }
        }
    }

    public class TimeLogger : ILogger
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        System.IO.TextWriter output;

        public TimeLogger(System.IO.TextWriter tw)
        {
            output = tw;
            sw.Start();
        }

        public void WriteLine(string log = null)
        {
            if (log != null)
            {
                output.Write($"[{sw.ElapsedMilliseconds,6}] ");
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

    public class RegexAttribute : Attribute
    {
        public RegexAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; private set; }
    }

}
