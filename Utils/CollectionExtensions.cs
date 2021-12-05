using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils
{
    public static class CollectionExtensions
    {
        public static void IncrementAtIndex<T>(this Dictionary<T, int> dict, T key, int val = 1) => IncrementAtIndex<T, int>(dict, key, val);

        static T Add<T>(T x, T y) => Add((dynamic)x, (dynamic)y);
        static int Add(int a, int b) => a + b;
        static long Add(long a, long b) => a + b;
        static ulong Add(ulong a, ulong b) => a + b;

        public static void IncrementAtIndex<T, V>(this Dictionary<T, V> dict, T key, V val)
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

        public static string AsString(this IEnumerable<char> input)
        {
            return String.Join("", input);
        }

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

        public static IEnumerable<IEnumerable<int>> Windows(this IEnumerable<int> input, int count)
        {
            int i = 0;
            while (true)
            {
                var vals = input.Skip(i++).Take(count);
                if (vals.Count() < count) break;
                yield return vals;
            }
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
            for (int row = 0; row < array2d.GetLength(0); row++)
            {
                for (int col = 0; col < array2d.GetLength(1); col++)
                {
                    yield return array2d[row, col];
                }
            }
        }

        public static IEnumerable<T> Row<T>(this T[,] array2d, int row)
        {
            for (int col = 0; col < array2d.GetLength(1); col++)
            {
                yield return array2d[row, col];
            }
        }

        public static IEnumerable<T> Column<T>(this T[,] array2d, int col)
        {
            for (int row = 0; row < array2d.GetLength(0); row++)
            {
                yield return array2d[row, col];
            }
        }

        public static Int64 Product(this IEnumerable<int> vals)
        {
            return vals.Aggregate((Int64)1, (total, val) => total * val);
        }

        public static int Xor(this IEnumerable<int> vals)
        {
            return vals.Aggregate(0, (total, val) => total ^ val);
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
            T maxValue = default(T); // Immediately overwritten anyway

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

        public static int CombinedHashCode<T>(this IEnumerable<T> sequence)
        {
            HashCode hc = new HashCode();
            foreach (var v in sequence)
            {
                hc.Add(v);
            }
            return hc.ToHashCode();
        }

        public static IEnumerable<(int Index, T Value)> WithIndex<T>(this IEnumerable<T> sequence)
        {
            int i = 0;
            return from v in sequence
                   select (i++, v);
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> sequence) => new Queue<T>(sequence);

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> sequence)
        {
            foreach (var t in sequence) queue.Enqueue(t);
        }

    }


}