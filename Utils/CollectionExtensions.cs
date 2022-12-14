using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;

namespace AoC.Utils
{
    public static class CollectionExtensions
    {
        public static void IncrementAtIndex<T>(this Dictionary<T, int> dict, T key, int val = 1) => IncrementAtIndex<T, int>(dict, key, val);
        public static void IncrementAtIndex<T>(this Dictionary<T, long> dict, T key, long val = 1) => IncrementAtIndex<T, long>(dict, key, val);

        public static void IncrementAtIndex<T, V>(this Dictionary<T, V> dict, T key, V val) where V : INumber<V>
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += val;
            }
            else
            {
                dict[key] = val;
            }
        }

        public static T2 GetOrDefault<T1, T2>(this Dictionary<T1, T2> dict, T1 key) => dict.TryGetValue(key, out T2 val) ? val : default;


        public static T GetOrCalculate<K, T>(this Dictionary<K, T> dict, K key, Func<K, T> predicate)
        {
            if (!dict.TryGetValue(key, out T val))
            {
                dict[key] = val = predicate(key);
            }
            return val;
        }

        public static string AsString(this IEnumerable<char> input) => string.Concat(input);

        public static IEnumerable<byte> AsNybbles(this IEnumerable<byte> bytes)
        {
            foreach (var x in bytes)
            {
                yield return (byte)((x & 0xF0) >> 4);
                yield return (byte)(x & 0x0F);
            }
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> set, IEnumerable<T> subset = null)
        {
            subset ??= Array.Empty<T>();
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

        public static IEnumerable<(T, T)> Pairs<T>(this IEnumerable<T> set)
        {
            foreach (var el1 in set)
            {
                foreach (var el2 in set)
                {
                    if (!EqualityComparer<T>.Default.Equals(el1, el2))
                    {
                        yield return (el1, el2);
                    }
                }
            }
        }

        public static (T, T) TakePair<T>(this IEnumerable<T> set)
        {
            return set.Pairs().First();
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> items, T first)
        {
            yield return first;
            foreach (T item in items)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> Sandwich<T>(this IEnumerable<T> items, T firstlast)
        {
            yield return firstlast;
            foreach (T item in items)
            {
                yield return item;
            }
            yield return firstlast;
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

        public static IEnumerable<T[]> Windows<T>(this IEnumerable<T> input, int count)
        {
            Queue<T> queue = new();
            var iter = input.GetEnumerator();
            var hasData = iter.MoveNext();
            while (queue.Count < count && hasData)
            {
                queue.Add(iter.Current);
                hasData = iter.MoveNext();
            }
            yield return queue.ToArray();
            while (hasData)
            {
                queue.Dequeue();
                queue.Add(iter.Current);
                hasData = iter.MoveNext();
                yield return queue.ToArray();
            }
        }

        public static IEnumerable<(T first, T second)> OverlappingPairs<T>(this IEnumerable<T> input)
        {
            return input.Windows(2).Select(win => (win[0], win[1]));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            var rnd = new Random();
            return items.OrderBy(_ => rnd.Next());
        }

        public static IEnumerable<IEnumerable<T>> DuplicateSequence<T>(this IEnumerable<T> input)
        {
            while (true)
            {
                yield return input;
            }
        }


        public static IEnumerable<T> Values<T>(this T[,] array2d)
        {
            for (int y = 0; y < array2d.Height(); ++y)
                for (int x = 0; x < array2d.Width(); ++x)
                    yield return array2d[x, y];
        }

        public static IEnumerable<(int x, int y)> Keys<T>(this T[,] array2d)
        {
            for (int y = 0; y < array2d.Height(); ++y)
                for (int x = 0; x < array2d.Width(); ++x)
                    yield return (x, y);
        }

        public static IEnumerable<((int x, int y) key, T value)> Entries<T>(this T[,] array2d)
        {
            for (int y = 0; y < array2d.Height(); ++y)
                for (int x = 0; x < array2d.Width(); ++x)
                    yield return ((x, y), array2d[x, y]);
        }

        public static IEnumerable<T> Row<T>(this T[,] array2d, int row)
        {
            for (int col = 0; col < array2d.Width(); col++)
                yield return array2d[col, row];
        }

        public static IEnumerable<T> Column<T>(this T[,] array2d, int col)
        {
            for (int row = 0; row < array2d.Height(); row++)
                yield return array2d[col, row];
        }

        public static bool TrySet<T>(this T[,] array2d, (int col, int row) pos, T val) => TrySet(array2d, pos.col, pos.row, val);

        public static bool TrySet<T>(this T[,] array2d, int col, int row, T val)
        {
            if (row >= array2d.Height() || col >= array2d.Width() || row < 0 || col < 0)
            {
                return false;
            }

            array2d[col, row] = val;
            return true;
        }

        public static bool TryIncrement(this int[,] array2d, (int col, int row) pos, int val = 1) => TryIncrement(array2d, pos.col, pos.row, val);

        public static bool TryIncrement(this int[,] array2d, int col, int row, int val)
        {
            if (row >= array2d.Height() || col >= array2d.Width() || row < 0 || col < 0)
            {
                return false;
            }

            array2d[col, row] += val;

            return true;
        }

        public static T GetOrDefault<T>(this T[,] array2d, (int col, int row) pos) => GetOrDefault(array2d, pos.col, pos.row);

        public static T GetOrDefault<T>(this T[,] array2d, int col, int row)
        {
            return row >= array2d.Height() || col >= array2d.Width() || row < 0 || col < 0 ? default : array2d[col, row];
        }

        public static Dictionary<TKey, TValue> Minus<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            dict.Remove(key);
            return dict;
        }

        public static Dictionary<TKey, TValue> Plus<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.Add(key, value);
            return dict;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            return enumerable.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey,TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable, IComparer<TKey> comparer)
        {
            var dict = new SortedDictionary<TKey, TValue>(comparer);

            foreach (var kvp in enumerable)
            {
                dict.Add(kvp.Key, kvp.Value);
            }

            return dict;
        }

        public static int Height<T>(this T[,] array2d) => array2d.GetLength(1);
        public static int Width<T>(this T[,] array2d) => array2d.GetLength(0);

        public static long Product(this IEnumerable<int> vals)
        {
            return vals.Aggregate((long)1, (total, val) => total * val);
        }

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Aggregate((long)1, (total, el) => total * selector(el));
        }

        public static long Product(this IEnumerable<long> vals)
        {
            return vals.Aggregate((long)1, (total, val) => total * val);
        }

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            return source.Aggregate((long)1, (total, el) => total * selector(el));
        }

        public static int Xor(this IEnumerable<int> vals)
        {
            return vals.Aggregate(0, (total, val) => total ^ val);
        }

        public static ulong Sum(this IEnumerable<ulong> vals)
        {
            return vals.Aggregate(0UL, (x, y) => x + y);
        }

        public static int Sum(this IEnumerable<byte> vals)
        {
            return vals.Aggregate(0, (x, y) => x + y);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

        public static int MaxIndex<T>(this IEnumerable<T> sequence) where T : IComparable<T>
        {
            int maxIndex = -1;
            T maxValue = default; // Immediately overwritten anyway

            int index = 0;
            foreach (T value in sequence)
            {
                if (value.CompareTo(maxValue) > 0 || maxIndex == -1)
                {
                    maxIndex = index;
                    maxValue = value;
                }
                index++;
            }
            return maxIndex;
        }

        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> sequence, int startFrom = 0)
        {
            return sequence.Select((v, i) => (i + startFrom, v));
        }

        public static void Add<T>(this Queue<T> queue, T item) => queue.Enqueue(item);

        public static Queue<T> ToQueue<T>(this IEnumerable<T> sequence) => new(sequence);
        public static Stack<T> ToStack<T>(this IEnumerable<T> sequence) => new(sequence);

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> sequence)
        {
            foreach (var t in sequence) queue.Enqueue(t);
        }

        public static T Pop<T>(this IEnumerator<T> enumerator)
        {
            T val = enumerator.Current;
            enumerator.MoveNext();
            return val;
        }

        public static void Operate<TElement, TPriority>(this PriorityQueue<TElement, TPriority> queue, Action<TElement> action)
        {
            while (queue.TryDequeue(out var element, out var _))
            {
                action(element);
            }
        }

        public static void Operate<TElement>(this Queue<TElement> queue, Action<TElement> action)
        {
            while (queue.TryDequeue(out var element))
            {
                action(element);
            }
        }

        public static void Operate<TElement>(this HashSet<TElement> set, Action<TElement> action)
        {
            while (set.Any())
            {
                var element = set.First();
                set.Remove(element);
                action(element);
            }
        }

        public static IEnumerable<T> InsertRangeAt<T>(this IEnumerable<T> into, IEnumerable<T> elements, int pos)
        {
            return into.Take(pos).Union(elements).Union(into.Skip(pos));
        }

        public static int GetCombinedHashCode(this IEnumerable<int> collection)
        {
            unchecked
            {
                int hash = 17;
                foreach (var v in collection)
                {
                    hash = hash * 31 + v;
                }
                return hash;
            }
        }

        public static int GetCombinedHashCode(this IEnumerable<byte> collection)
        {
            unchecked
            {
                int hash = 17;
                foreach (var v in collection)
                {
                    hash = hash * 31 + v;
                }
                return hash;
            }
        }

        public static int IndexOf<T>(this IEnumerable<T> collection, T item) where T : IComparable<T>
        {
            int count = 0;
            foreach (var el in collection)
            {
                if (el.CompareTo(item) == 0) return count;
                count++;
            }
            return -1;
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source.Where((x, i) => i % chunkSize == 0).Select((x, i) => source.Skip(i * chunkSize).Take(chunkSize));
        }

        public static Dictionary<TVal, IEnumerable<TKey>> Invert<TKey, TVal>(this Dictionary<TKey, TVal> dict)
        {
            return dict.GroupBy(kvp => kvp.Value).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key));
        }

        public static Dictionary<TVal, IEnumerable<(int x, int y)>> Invert<TVal>(this TVal[,] dict)
        {
            return dict.Entries().GroupBy(kvp => kvp.value).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.key));
        }

        public static bool TryGetValue<TVal>(this TVal[,] dict, (int x, int y) key, out TVal val)
        {
            if (key.x < 0 || key.y < 0 || key.x >= dict.Width() || key.y >= dict.Height())
            {
                val = default;
                return false;
            }

            val = dict[key.x, key.y];
            return true;
        }

        public static IEnumerable<TSource> AppendMultiple<TSource>(this IEnumerable<TSource> source, params TSource[] elements)
        {
            return source.Concat(elements);
        }        
    }
}