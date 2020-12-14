using Advent.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent
{
    public class Util
    {
        public enum QuestionPart
        {
            Part1 = 1,
            Part2 = 2
        }

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

        public static List<T> Parse<T>(string input, char splitChar = '\n')
        {
            return Parse<T>(input.Split(splitChar)
                                 .Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public static int[] Parse32(string input, char splitChar = '\0') => Parse32(Split(input, splitChar));
        public static uint[] ParseU32(string input, char splitChar = '\0') => ParseU32(Split(input, splitChar));
        public static Int64[] Parse64(string input, char splitChar = '\0') => Parse64(Split(input, splitChar));
        public static UInt64[] ParseU64(string input, char splitChar = '\0') => ParseU64(Split(input, splitChar));

        public static int[] Parse32(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();
        public static uint[] ParseU32(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => UInt32.Parse(s)).ToArray();
        public static Int64[] Parse64(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int64.Parse(s)).ToArray();
        public static UInt64[] ParseU64(string[] input) => input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => UInt64.Parse(s)).ToArray();

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
                var hashes = Enumerable.Range(start, blockSize).AsParallel().Where(n => IsHash(n, baseStr, numZeroes));
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

}
