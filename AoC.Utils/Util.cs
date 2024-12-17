using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace AoC;

public enum QuestionPart
{
    Part1 = 1,
    Part2 = 2,
}

public static class QuestionPartExtensions
{
    public static bool One(this QuestionPart part) => part == QuestionPart.Part1;
    public static bool Two(this QuestionPart part) => part == QuestionPart.Part2;

}

// coming soon, maybe?
//public implicit extension QuestionPartExtension for QuestionPart
//{
//    public bool One
//        => this == QuestionPart.Part1
//}

public partial class Util
{
    public static IEnumerable<IPuzzle> GetPuzzles()
    {
        return Assembly.GetCallingAssembly().GetTypes()
            .Where(x => typeof(IPuzzle).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Where(t => PuzzleHelpers.ShouldRun(t))
            .Select(x => (IPuzzle)Activator.CreateInstance(x))
            .OrderBy(x => x.Name()).ToArray();
    }

    public static string[] Split(string input, string splitter = null)
    {
        if (splitter == null)
        {
            int commaCount = input.Count(c => c == ',');
            int linefeedCount = input.Count(c => c == '\n');
            return linefeedCount > commaCount
                ? input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray()
                : input.Split(",").Select(e => e.Replace("\n", "")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
        else
        {
            return input.Split(splitter).Select(e => e.Replace("\n", "")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex SplitNumbersAndWordsRegex();
    public static IEnumerable<(string word, int number)> SplitNumbersAndWords(string input)
    {
        return SplitNumbersAndWordsRegex().Split(input).WithoutNullOrWhiteSpace().Select(entry =>
        int.TryParse(entry, out var v) ? (null, v) : (entry, 0));
    }

    public static List<T> CreateMultiple<T>(int count) where T : new() => Enumerable.Repeat(0, count).Select(_ => new T()).ToList();

    public static T[] ParseNumbers<T>(string input, string splitter = null, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Any) where T : INumberBase<T>
        => ParseNumbers<T>(Split(input, splitter), style);

    public static T[] ParseNumbers<T>(IEnumerable<string> input, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Any) where T : INumberBase<T>
        => input.WithoutNullOrWhiteSpace().Select(line => T.Parse(line, style, null)).ToArray();

    public static T[][] ParseNumberList<T>(string input) where T : INumberBase<T>
        => Split(input).Select(v => ParseNumbers<T>(v, " ")).ToArray();

    public interface IConvertomatic
    {
        public abstract object Convert(char c);
        public abstract bool ShouldConvert(char c);
    }

    public static class Convertomatic
    {
        public class ConvertInt : IConvertomatic
        {
            public object Convert(char c) => c.AsDigit();

            public bool ShouldConvert(char c) => true;
        }
        public class ConvertByte : IConvertomatic
        {
            public object Convert(char c) => (byte)c.AsDigit();
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

        public class SkipChars(char ch = ' ') : IConvertomatic
        {
            readonly char toSkip = ch;

            public object Convert(char c) => c;
            public bool ShouldConvert(char c) => c != toSkip;
        }

        public static object DefaultKeys((int x, int y) v) => v;
    }

    static IConvertomatic GetValueConverter<T>()
    {
        if (typeof(T) == typeof(int)) return new Convertomatic.ConvertInt();
        if (typeof(T) == typeof(byte)) return new Convertomatic.ConvertByte();
        if (typeof(T) == typeof(bool)) return new Convertomatic.ConvertBool();
        if (typeof(T) == typeof(char)) return new Convertomatic.ConvertChar();

        throw new NotImplementedException(typeof(T).FullName);
    }

    static Func<(int, int), object> GetKeyConverter<TKey>()
    {
        if (typeof(TKey) == typeof((int, int))) return Convertomatic.DefaultKeys;
        if (typeof(TKey) == typeof(PackedPos32)) return PackedPos32.Convert;

        throw new NotImplementedException(typeof(TKey).FullName);
    }

    public static T[,] ParseMatrix<T>(string input) => ParseMatrix<T>(Split(input));

    public static T[,] ParseMatrix<T>(IEnumerable<string> input)
    {
        int height = input.Count();
        int width = input.First().Length;
        var mtx = new T[width, height];

        var converter = GetValueConverter<T>();

        input.Index().ForEach(line => line.Item.Index().Where(ch => converter.ShouldConvert(ch.Item)).ForEach(ch => mtx[ch.Index, line.Index] = (T)converter.Convert(ch.Item)));

        return mtx;
    }

    public static SparseMatrix<(int x, int y), T> ParseSparseMatrix<T>(string input) => ParseSparseMatrix<(int, int), T>(input, Convertomatic.DefaultKeys, null);

    public static SparseMatrix<(int x, int y), T> ParseSparseMatrix<T>(string input, IConvertomatic valueConverter) => ParseSparseMatrix<(int, int), T>(input, Convertomatic.DefaultKeys, valueConverter);

    public static SparseMatrix<(int x, int y), T> ParseSparseMatrix<T>(IEnumerable<string> input, IConvertomatic valueConverter) => ParseSparseMatrix<(int x, int y), T>(input, Convertomatic.DefaultKeys, valueConverter);

    public static SparseMatrix<TKey, TVal> ParseSparseMatrix<TKey, TVal>(string input) => ParseSparseMatrix<TKey, TVal>(input, null, null);

    public static SparseMatrix<TKey, TVal> ParseSparseMatrix<TKey, TVal>(string input, IConvertomatic valueConverter) => ParseSparseMatrix<TKey, TVal>(input, null, valueConverter);

    public static SparseMatrix<TKey, TVal> ParseSparseMatrix<TKey, TVal>(string input, Func<(int, int), object> keyConverter, IConvertomatic valueConverter) => ParseSparseMatrix<TKey, TVal>(Split(input), keyConverter, valueConverter);

    public static SparseMatrix<TKey, TVal> ParseSparseMatrix<TKey, TVal>(IEnumerable<string> input, Func<(int, int), object> keyConverter, IConvertomatic valueConverter)
    {
        var mtx = new SparseMatrix<TKey, TVal>
        {
            Width = input.First().Length - 1,
            Height = input.Count() - 1,
            Dict = []
        };

        keyConverter ??= GetKeyConverter<TKey>();
        valueConverter ??= GetValueConverter<TVal>();

        input.Index().ForEach(line => line.Item.Index().Where(ch => valueConverter.ShouldConvert(ch.Item)).ForEach(ch => mtx.Dict[(TKey)keyConverter((ch.Index, line.Index))] = (TVal)valueConverter.Convert(ch.Item)));

        return mtx;
    }

    public class SparseMatrix<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>, ISerializable, IDeserializationCallback where TKey : notnull
    {
        public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)Dict)[key]; set => ((IDictionary<TKey, TValue>)Dict)[key] = value; }
        public object this[object key] { get => ((IDictionary)Dict)[key]; set => ((IDictionary)Dict)[key] = value; }

        public Dictionary<TKey, TValue> Dict { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsInside((int x, int y) pos)
        {
            return pos.x >= 0 && pos.y >= 0 && pos.x <= Width && pos.y <= Height;
        }

        public IEnumerable<TKey> Keys => Dict.Keys;
        public IEnumerable<TValue> Values => Dict.Values;

        public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).IsReadOnly;

        public bool IsFixedSize => ((IDictionary)Dict).IsFixedSize;

        public bool IsSynchronized => ((ICollection)Dict).IsSynchronized;

        public object SyncRoot => ((ICollection)Dict).SyncRoot;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => ((IDictionary<TKey, TValue>)Dict).Keys;

        ICollection IDictionary.Keys => ((IDictionary)Dict).Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => ((IDictionary<TKey, TValue>)Dict).Values;

        ICollection IDictionary.Values => ((IDictionary)Dict).Values;

        public void Add(TKey key, TValue value) => ((IDictionary<TKey, TValue>)Dict).Add(key, value);
        public void Add(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).Add(item);
        public void Add(object key, object value) => ((IDictionary)Dict).Add(key, value);
        public void Clear() => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).Contains(item);
        public bool Contains(object key) => ((IDictionary)Dict).Contains(key);
        public bool ContainsKey(TKey key) => ((IDictionary<TKey, TValue>)Dict).ContainsKey(key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).CopyTo(array, arrayIndex);
        public void CopyTo(Array array, int index) => ((ICollection)Dict).CopyTo(array, index);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)Dict).GetEnumerator();
#pragma warning disable SYSLIB0050 // Type or member is obsolete
        public void GetObjectData(SerializationInfo info, StreamingContext context) => ((ISerializable)Dict).GetObjectData(info, context);
#pragma warning restore SYSLIB0050 // Type or member is obsolete
        public void OnDeserialization(object sender) => ((IDeserializationCallback)Dict).OnDeserialization(sender);
        public bool Remove(TKey key) => ((IDictionary<TKey, TValue>)Dict).Remove(key);
        public bool Remove(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)Dict).Remove(item);
        public void Remove(object key) => ((IDictionary)Dict).Remove(key);
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => ((IDictionary<TKey, TValue>)Dict).TryGetValue(key, out value);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dict).GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary)Dict).GetEnumerator();

        //public static implicit operator Factory(string data) => Parser.RegexFactory<Factory>(data);
        public static implicit operator Dictionary<TKey, TValue>(SparseMatrix<TKey, TValue> data) => data.Dict;
    }

    public static IEnumerable<(T1 item1, T2 item2)> Matrix<T1, T2>(IEnumerable<T1> set1, IEnumerable<T2> set2) => set1.SelectMany(x => set2.Select(y => (x, y)));
    public static IEnumerable<(T1 item1, T1 item2)> Matrix2<T1>(IEnumerable<T1> set) => set.SelectMany(x => set.Select(y => (x, y)));

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

    public static IEnumerable<PackedVect2<TNum, TPacker>> Range2DInclusive<TNum, TPacker>((TNum minY, TNum maxY, TNum minX, TNum maxX) range) where TNum : IBinaryInteger<TNum> where TPacker : ICoordinatePacker2<TNum>
    {
        for (TNum y = range.minY; y <= range.maxY; ++y)
        {
            for (TNum x = range.minX; x <= range.maxX; ++x)
            {
                yield return (x, y);
            }
        }
    }

    public static IEnumerable<(int x, int y)> Range2DExclusive((int minY, int maxY, int minX, int maxX) range)
    {
        for (int y = range.minY; y < range.maxY; ++y)
        {
            for (int x = range.minX; x < range.maxX; ++x)
            {
                yield return (x, y);
            }
        }
    }

    public static IEnumerable<PackedVect2<TNum, TPacker>> Range2DExclusive<TNum, TPacker>((TNum minY, TNum maxY, TNum minX, TNum maxX) range) where TNum : IBinaryInteger<TNum> where TPacker : ICoordinatePacker2<TNum>
    {
        for (TNum y = range.minY; y < range.maxY; ++y)
        {
            for (TNum x = range.minX; x < range.maxX; ++x)
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

    public static IEnumerable<PackedVect3<TNum, TPacker>> Range3DInclusive<TNum, TPacker>((TNum minZ, TNum maxZ, TNum minY, TNum maxY, TNum minX, TNum maxX) range) where TNum : IBinaryInteger<TNum> where TPacker : ICoordinatePacker3<TNum>
    {
        for (TNum z = range.minZ; z <= range.maxZ; ++z)
        {
            for (TNum y = range.minY; y <= range.maxY; ++y)
            {
                for (TNum x = range.minX; x <= range.maxX; ++x)
                {
                    yield return (x, y, z);
                }
            }
        }
    }

    public static IEnumerable<(int x, int y, int z)> Range3DExclusive((int minZ, int maxZ, int minY, int maxY, int minX, int maxX) range)
    {
        for (int z = range.minZ; z < range.maxZ; ++z)
        {
            for (int y = range.minY; y < range.maxY; ++y)
            {
                for (int x = range.minX; x < range.maxX; ++x)
                {
                    yield return (x, y, z);
                }
            }
        }
    }

    public static IEnumerable<PackedVect3<TNum, TPacker>> Range3DExclusive<TNum, TPacker>((TNum minZ, TNum maxZ, TNum minY, TNum maxY, TNum minX, TNum maxX) range) where TNum : IBinaryInteger<TNum> where TPacker : ICoordinatePacker3<TNum>
    {
        for (TNum z = range.minZ; z < range.maxZ; ++z)
        {
            for (TNum y = range.minY; y < range.maxY; ++y)
            {
                for (TNum x = range.minX; x < range.maxX; ++x)
                {
                    yield return (x, y, z);
                }
            }
        }
    }

    public static IEnumerable<int> RangeBetween(int start, int end) => Enumerable.Range(start, end - start);

    public static IEnumerable<int> RangeInclusive(int start, int end) => Enumerable.Range(start, end - start + 1);

    public static IEnumerable<(int x, int y)> Matrix(int maxX, int maxY) => Matrix(Enumerable.Range(0, maxX), Enumerable.Range(0, maxY));

    public static IEnumerable<int> Sequence(int start, int delta)
    {
        int val = start;
        while (true)
        {
            yield return val;
            val += delta;
        }
    }

    static string sessionCookie = null;

    public static string Download(string url)
    {
        sessionCookie ??= File.ReadAllText(".session");

        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Uri("https://adventofcode.com"), new Cookie("session", sessionCookie));
        using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; github.com/benjymous/adventofcode)");

        var content = client.GetStringAsync(url).Result;
        return content;
    }

    public static string GetInput<T>() where T : IPuzzle, new() => PuzzleHelpers.GetInput(typeof(T));
    public static string GetInput(IPuzzle puzzle) => PuzzleHelpers.GetInput(puzzle.GetType());

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

    public static void RepeatWhile(Func<bool> func)
    {
        while (func()) ;
    }

    public static void RepeatWhile(Func<bool> func, Action action)
    {
        while (func()) action();
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

    public static int[] ExtractNumbers(IEnumerable<char> input) => ExtractNumbers<int>(input);

    public static T[] ExtractNumbers<T>(IEnumerable<char> input) where T : IBinaryInteger<T> => input.Where(c => c is ' ' or '-' or (>= '0' and <= '9')).AsString().Trim().Split(" ").Where(w => !string.IsNullOrEmpty(w)).Select(s => T.Parse(s, System.Globalization.NumberStyles.Any, null)).ToArray();

    public static void SetBit(ref long value, int i) => value |= (1L) << i;

    public static void ClearBit(ref long value, int i) => value &= ~(1L << i);

    public static (TInput input, TResult result) BinarySearch<TInput, TResult>(TInput start, Func<TInput, (bool success, TResult res)> test) where TInput : IBinaryInteger<TInput>
        => BinarySearch(start, TInput.Zero, test);

    public static (TInput input, TResult result) BinarySearch<TInput, TResult>(TInput min, TInput max, Func<TInput, (bool success, TResult res)> test) where TInput : IBinaryInteger<TInput>
    {
        TInput two = TInput.CreateChecked(2);
        TInput three = TInput.CreateChecked(3);

        Dictionary<TInput, TResult> results = [];

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

            TInput mid = (max + min) / two;
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

    public static TInput BinarySearch<TInput>(TInput min, TInput max, Func<TInput, bool> test) where TInput : IBinaryInteger<TInput> => BinarySearch<TInput, object>(min, max, input => (test(input), null)).input;

    public static int TwoPowi(int i) => i switch
    {
        0 => 1,
        1 => 2,
        2 => 4,
        3 => 8,
        4 => 16,
        5 => 32,
        6 => 64,
        7 => 128,
        8 => 256,
        9 => 512,
        10 => 1024,
        11 => 2048,
        12 => 4096,
        13 => 8192,
        14 => 16384,
        _ => throw new ArgumentException("not implemented"),
    };

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
        return ms < 1000
            ? $"     .{span:fff}"
            : (int)span.TotalHours > 0
                ? span.ToString(@"hh\:mm\:ss.fff")
                : (int)span.TotalMinutes > 0 ? $"{span:mm\\:ss\\.fff}" : $"   {span:ss\\.fff}";
    }

    public static IEnumerable<(T1, T2)> Combinations<T1, T2>(IEnumerable<T1> set1, IEnumerable<T2> set2)
    {
        foreach (var item1 in set1)
            foreach (var item2 in set2)
                yield return (item1, item2);
    }

    public static ushort MakeTwoCC(string name) => name.Length == 1 ? name[0] : (ushort)(name[1] + (name[0] << 8));
    public static ushort MakeTwoCC(char c1, char c2) => (ushort)(c1 + (c2 << 8));
    public static string UnMakeTwoCC(ushort val) => ((char[])[(char)((val >> 8) & 0xff), (char)(val & 0xff)]).AsString();

    public static uint MakeFourCC(string name) => (name.Length > 3 ? name[3] : 0U) + ((uint)name[2] << 8) + ((uint)name[1] << 16) + ((uint)name[0] << 24);
    public static string UnMakeFourCC(uint val) => ((char[])[(char)((val >> 24) & 0xff), (char)((val >> 16) & 0xff), (char)((val >> 8) & 0xff), (char)((val) & 0xff)]).AsString();

    public static (T min, T max) MinMax<T>(params T[] input) where T : IBinaryInteger<T> => (input.Min(), input.Max());

    public static T GCD<T>(T a, T b) where T : IBinaryInteger<T>
    {
        while (b != T.Zero)
        {
            (a, b) = (b, a % b);
        }
        return a;
    }

    public static T LCM<T>(T a, T b) where T : IBinaryInteger<T> => a * b / GCD(a, b);

    public static T TimeFunction<T>(Func<T> fn)
    {
        Stopwatch sw = Stopwatch.StartNew();
        var res = fn();
        sw.Stop();

        Console.WriteLine($"{sw.Elapsed.TotalMilliseconds}ms");
        return res;
    }

    public static (double X, double Y)? GetIntersectionPoint<T>((T X, T Y)[] l1, (T X, T Y)[] l2) where T : IBinaryInteger<T>
    {
        T denominator = ((l2[1].Y - l2[0].Y) * (l1[1].X - l1[0].X)) - ((l2[1].X - l2[0].X) * (l1[1].Y - l1[0].Y));
        T numerator1 = ((l2[1].X - l2[0].X) * (l1[0].Y - l2[0].Y)) - ((l2[1].Y - l2[0].Y) * (l1[0].X - l2[0].X));
        T numerator2 = ((l1[1].X - l1[0].X) * (l1[0].Y - l2[0].Y)) - ((l1[1].Y - l1[0].Y) * (l1[0].X - l2[0].X));

        if (denominator == T.Zero)
        {
            return numerator1 == T.Zero && numerator2 == T.Zero ? (Convert.ToDouble(l1[0].X), Convert.ToDouble(l1[0].Y)) : null;
        }

        double r = Convert.ToDouble(numerator1) / Convert.ToDouble(denominator);

        return (
                Convert.ToDouble(l1[0].X) + (r * Convert.ToDouble(l1[1].X - l1[0].X)),
                Convert.ToDouble(l1[0].Y) + (r * Convert.ToDouble(l1[1].Y - l1[0].Y))
               );
    }

    public static TStateType FindCycle<TFingerprintType, TStateType>(int expectedRounds, TStateType initialState, Func<TStateType, TFingerprintType> GetFingerprint, Func<TStateType, TStateType> PerformCycle)
    {
        int targetRound = expectedRounds;

        var seen = new Dictionary<TFingerprintType, int>();

        TStateType state = initialState;

        for (int round = 0; round <= targetRound; round++)
        {
            state = PerformCycle(state);

            if (targetRound == expectedRounds && seen.FindCycle(GetFingerprint(state), round, targetRound, out var shortcut))
            {
                targetRound = shortcut;
            }
        }

        return state;
    }

    public static TNum RoundUpToNextMultiple<TNum>(TNum n, TNum mult) where TNum : IBinaryNumber<TNum> => (n + (mult - TNum.One)) / mult * mult;
}

public class AutoArray<DataType>(IEnumerable<DataType> input) : IEnumerable<DataType>
{
    DataType[] data = input.ToArray();

    DataType Get(int key) => key >= data.Length ? default : data[key];

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

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public DataType this[int key]
    {
        get => Get(key);
        set => Set(key, value);
    }
}

public class TextBuffer : TextWriter
{
    readonly StringBuilder builder = new();
    public override void Write(char value) => builder.Append(value);

    public override Encoding Encoding => Encoding.UTF8;

    public override string ToString() => builder.ToString();
}

public static class HashBreaker
{
    public static (int foundIndex, IEnumerable<char> foundHash) FindHash(string baseStr, int numZeroes, int start = 0)
    {
        if (numZeroes is > 6 or < 5) throw new NotImplementedException("numZeroes currently only 5 or 6");
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
            if (outputBuffer[0] == 0 && outputBuffer[1] == 0 && ((numZeroes == 5 && ((outputBuffer[2] & 0xf0) == 0)) || (numZeroes == 6 && outputBuffer[2] == 0))) return (index, outputBuffer.GetHexChars());

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
    readonly TextWriter output;

    public TimeLogger(TextWriter tw)
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

    public override string ToString() => output.ToString();
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

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter)]
public class RegexAttribute([StringSyntax("Regex")] string pattern) : Attribute
{
    public Regex Regex { get; private set; } = Memoizer.Memoize(pattern, _ => new Regex(pattern));
}

[AttributeUsage(AttributeTargets.Parameter)]
public class SplitAttribute(string splitter, [StringSyntax("Regex")] string kvpMatchRegex = null) : Attribute
{
    public readonly string Splitter = splitter;
    public Regex KvpMatchRegex { get; private set; } = kvpMatchRegex != null ? Memoizer.Memoize(kvpMatchRegex, _ => new Regex(kvpMatchRegex)) : null;
}

[AttributeUsage(AttributeTargets.Parameter)]
public class BaseAttribute(int numberBase) : Attribute
{
    public readonly int NumberBase = numberBase;
}

public class Boxed<T>(T v)
{
    readonly T value = v;

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

    public static TAttribute Get<TAttribute>(this Dictionary<Type, Attribute> attrs) where TAttribute : Attribute => attrs == null ? default : (TAttribute)attrs.GetOrDefault(typeof(TAttribute));

    public static Boxed<T> Wrap<T>(this T item) => new(item);
}
