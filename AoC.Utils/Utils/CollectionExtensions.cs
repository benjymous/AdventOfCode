using System.Collections;

namespace AoC.Utils
{
    public static class CollectionExtensions
    {
        public static void IncrementAtIndex<T>(this IDictionary<T, int> dict, T key, int val = 1) => IncrementAtIndex<T, int>(dict, key, val);
        public static void IncrementAtIndex<T>(this IDictionary<T, long> dict, T key, long val = 1) => IncrementAtIndex<T, long>(dict, key, val);

        public static void IncrementAtIndex<T, V>(this IDictionary<T, V> dict, T key, V val) where V : INumber<V>
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

        public static void Move<T1, T2>(this IDictionary<T1, T2> dict, T1 from, T1 to)
        {
            if (!EqualityComparer<T1>.Default.Equals(from, to))
            {
                T2 element = dict[from];
                dict[to] = element;
                dict.Remove(from);
            }
        }

        public static T2 GetOrDefault<T1, T2>(this IDictionary<T1, T2> dict, T1 key) => dict.TryGetValue(key, out T2 val) ? val : default;

        public static T GetOrCalculate<K, T>(this IDictionary<K, T> dict, K key, Func<K, T> predicate)
        {
            if (!dict.TryGetValue(key, out T val))
            {
                dict[key] = val = predicate(key);
            }
            return val;
        }

        public static T GetOrCreate<K, T>(this IDictionary<K, T> dict, K key) where T : new()
        {
            if (!dict.TryGetValue(key, out T val))
            {
                dict[key] = val = new();
            }
            return val;
        }

        public static T GetIndexBit<K, T>(this IDictionary<K, T> dict, K key) where T : INumber<T> => dict.GetOrCalculate(key, _ => T.CreateChecked(1 << dict.Count));

        public static void RemoveRange<K, T>(this IDictionary<K, T> dict, IEnumerable<K> keys)
        {
            foreach (var k in keys) dict.Remove(k);
        }

        public static IEnumerable<TKey> KeysWithValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue val) => dict.Where(kvp => kvp.Value.Equals(val)).Select(kvp => kvp.Key);

        public static TKey SingleWithValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TValue val) => dict.KeysWithValue(val).Single();
        public static void RemoveRange<T>(this HashSet<T> dict, IEnumerable<T> values)
        {
            foreach (var v in values) dict.Remove(v);
        }

        public static string AsString(this IEnumerable<char> input) => string.Concat(input);
        public static string AsString(this IEnumerable<string> input) => string.Concat(input);

        public static T AsNumber<T>(this IEnumerable<bool> input) where T : IBinaryInteger<T>
        {
            T res = T.Zero;

            foreach (var v in input)
                res = res.PushBit(v);

            return res;
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

        public static IEnumerable<(T, T)> UniquePairs<T>(this IEnumerable<T> set)
        {
            foreach (var (i1, el1) in set.Index())
            {
                foreach (var el2 in set.Skip(i1 + 1))
                {
                    yield return (el1, el2);
                }
            }
        }

        public static (T, T) TakePair<T>(this IEnumerable<T> set) => set.Pairs().First();

        public static (T, T) TakeTwo<T>(this Queue<T> queue) => (queue.Dequeue(), queue.Dequeue());

        public static (T, T, T) TakeThree<T>(this Queue<T> queue) => (queue.Dequeue(), queue.Dequeue(), queue.Dequeue());

        public static IEnumerable<T> Sandwich<T>(this IEnumerable<T> items, T firstlast)
        {
            yield return firstlast;
            foreach (T item in items)
            {
                yield return item;
            }
            yield return firstlast;
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> items)
        {
            if (!items.Any()) yield return Array.Empty<T>();
            else
            {
                var head = items.First();
                var tail = items.Skip(1);
                foreach (var sequence in tail.Combinations())
                {
                    yield return sequence; // Without first
                    yield return [head, .. sequence];
                }
            }
        }

        public static IEnumerable<T[]> Windows<T>(this IEnumerable<T> input, int count)
        {
            Queue<T> queue = [];
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

        public static IEnumerable<(T first, T second)> OverlappingPairs<T>(this IEnumerable<T> input) => input.Windows(2).Select(win => (win[0], win[1]));

        public static IEnumerable<IEnumerable<T>> DuplicateSequence<T>(this IEnumerable<T> input)
        {
            while (true)
            {
                yield return input;
            }
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> input, int repeats)
        {
            for (int i = 0; i < repeats; ++i)
            {
                foreach (var v in input) yield return v;
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

        public static IEnumerable<((int x, int y) Key, T Value)> Entries<T>(this T[,] array2d)
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

        public static IEnumerable<IEnumerable<T>> Rows<T>(this T[,] array2d)
        {
            for (int row = 0; row < array2d.Height(); row++)
                yield return Row(array2d, row);
        }

        public static IEnumerable<IEnumerable<T>> Columns<T>(this T[,] array2d)
        {
            for (int col = 0; col < array2d.Width(); col++)
                yield return Column(array2d, col);
        }

        public static string AsString<T>(this T[,] array2d, Func<T, string> displayFunc) => string.Join("\n", array2d.Rows().Select(row => string.Concat(row.Select(v => displayFunc(v)))));

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

        public static T GetOrDefault<T>(this T[,] array2d, int col, int row) => row >= array2d.Height() || col >= array2d.Width() || row < 0 || col < 0 ? default : array2d[col, row];

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

        public static Dictionary<TKey, TValue> Move<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey from, TKey to, TValue value)
        {
            dict.Remove(from);
            dict.Add(to, value);
            return dict;
        }

        public static SortedDictionary<TKey, TValue> ToSortedDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable/*, IComparer<TKey> comparer*/)
        {
            var dict = new SortedDictionary<TKey, TValue>(/*comparer*/);

            foreach (var kvp in enumerable)
            {
                dict.Add(kvp.Key, kvp.Value);
            }

            return dict;
        }

        public static int Height<T>(this T[,] array2d) => array2d.GetLength(1);
        public static int Width<T>(this T[,] array2d) => array2d.GetLength(0);

        public static bool Contains<T>(this T[,] array2d, int x, int y) => x >= 0 && y >= 0 && x < array2d.Width() && y < array2d.Height();
        public static bool Contains<T>(this T[,] array2d, (int x, int y) pos) => pos.x >= 0 && pos.y >= 0 && pos.x < array2d.Width() && pos.y < array2d.Height();

        public static (int, int) Dimensions<T>(this T[,] array2d) => (array2d.Width(), array2d.Height());

        public static long Product(this IEnumerable<int> vals) => vals.Aggregate((long)1, (total, val) => total * val);

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) => source.Aggregate((long)1, (total, el) => total * selector(el));

        public static long Product(this IEnumerable<long> vals) => vals.Aggregate((long)1, (total, val) => total * val);

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) => source.Aggregate((long)1, (total, el) => total * selector(el));

        public static int Xor(this IEnumerable<int> vals) => vals.Aggregate(0, (total, val) => total ^ val);

        public static ulong Sum(this IEnumerable<ulong> vals) => vals.Aggregate(0UL, (x, y) => x + y);

        public static int Sum(this IEnumerable<byte> vals) => vals.Aggregate(0, (x, y) => x + y);

        public static bool ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            bool didSomething = false;
            foreach (T element in source)
            {
                action(element);
                didSomething = true;
            }
            return didSomething;
        }

        public static T ApplyAll<T>(this IEnumerable<Func<T, T>> actions, T input)
        {
            foreach (var action in actions)
            {
                input = action(input);
            }
            return input;
        }

        public static Dictionary<K, V> Resolve<K, V>(this Dictionary<K, V> source, Action<(K Key, V Value, Dictionary<K, V> Collection)> action)
        {
            foreach (var element in source)
            {
                action((element.Key, element.Value, source));
            }
            return source;
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

        public static IEnumerable<(int Index, T Item)> Index<T>(this IEnumerable<T> sequence, int startFrom) => sequence.Select((v, i) => (i + startFrom, v));

        public static void Add<T>(this Queue<T> queue, T item) => queue.Enqueue(item);

        public static Queue<T> ToQueue<T>(this IEnumerable<T> sequence) => [.. sequence];
        public static Stack<T> ToStack<T>(this IEnumerable<T> sequence) => new(sequence);

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> sequence)
        {
            foreach (var t in sequence) queue.Enqueue(t);
        }

        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> sequence)
        {
            foreach (var t in sequence) stack.Push(t);
        }

        public static T Pop<T>(this IEnumerator<T> enumerator)
        {
            T val = enumerator.Current;
            enumerator.MoveNext();
            return val;
        }

        public static TResult Operate<TElement, TPriority, TResult>(this PriorityQueue<TElement, TPriority> queue, TResult initial, Func<TElement, TResult, (TResult res, IEnumerable<(TElement state, TPriority priority)> newStates)> action, Func<TResult, TResult, TResult> filter)
        {
            TResult best = initial;
            while (queue.TryDequeue(out var element, out var _))
            {
                var res = action(element, best);
                best = filter(best, res.res);
                if (res.newStates != null) foreach (var (state, priority) in res.newStates)
                    {
                        queue.Enqueue(state, priority);
                    }
            }
            return best;
        }

        public static void Operate<TElement, TPriority>(this PriorityQueue<TElement, TPriority> queue, Action<TElement> action)
        {
            while (queue.TryDequeue(out var element, out var _))
            {
                action(element);
            }
        }

        public static void Operate<TElement, TPriority>(this PriorityQueue<TElement, TPriority> queue, Action<TElement, TPriority> action)
        {
            while (queue.TryDequeue(out var element, out var priority))
            {
                action(element, priority);
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
            while (set.Count != 0)
            {
                var element = set.First();
                set.Remove(element);
                action(element);
            }
        }

        public static bool IsUnseen<TElement>(this HashSet<TElement> set, TElement element)
        {
            if (set.Contains(element)) return false;
            set.Add(element); return true;
        }

        public static IEnumerable<T> InsertRangeAt<T>(this IEnumerable<T> into, IEnumerable<T> elements, int pos) => into.Take(pos).Union(elements).Union(into.Skip(pos));

        public static int GetCombinedHashCode(this IEnumerable<int> collection)
        {
            unchecked
            {
                int hash = 17;
                foreach (var v in collection)
                {
                    hash = (hash * 31) + v;
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
                    hash = (hash * 31) + v;
                }
                return hash;
            }
        }

        public static int GetCombinedHashCode(this IEnumerable<char> collection)
        {
            unchecked
            {
                int hash = 17;
                foreach (var v in collection)
                {
                    hash = (hash * 31) + v;
                }
                return hash;
            }
        }

        public static int GetCombinedHashCode(this IEnumerable<bool> collection)
        {
            unchecked
            {
                int hash = 17;
                foreach (var v in collection)
                {
                    hash = (hash * 31) + (v ? 1 : 0);
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

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> items, int partitionSize) => new PartitionHelper<T>(items, partitionSize);

        private sealed class PartitionHelper<T> : IEnumerable<IEnumerable<T>>
        {
            readonly IEnumerable<T> Items;
            readonly int ChunkSize;
            bool ItemsRemaining;

            internal PartitionHelper(IEnumerable<T> items, int chunkSize)
            {
                Items = items;
                ChunkSize = chunkSize;
            }

            public IEnumerator<IEnumerable<T>> GetEnumerator()
            {
                var enumerator = Items.GetEnumerator();
                ItemsRemaining = enumerator.MoveNext();
                while (ItemsRemaining)
                    yield return GetNextBatch(enumerator).ToArray();
            }

            IEnumerable<T> GetNextBatch(IEnumerator<T> enumerator)
            {
                for (int i = 0; i < ChunkSize; ++i)
                {
                    yield return enumerator.Current;
                    ItemsRemaining = enumerator.MoveNext();
                    if (!ItemsRemaining)
                        yield break;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public static Dictionary<TVal, IEnumerable<TKey>> Invert<TKey, TVal>(this Dictionary<TKey, TVal> dict) => dict.Where(kvp => kvp.Value != null).GroupBy(kvp => kvp.Value).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key));

        public static FrozenDictionary<TVal, IEnumerable<TKey>> InvertFrozen<TKey, TVal>(this Dictionary<TKey, TVal> dict) => dict.Where(kvp => kvp.Value != null).GroupBy(kvp => kvp.Value).ToFrozenDictionary(g => g.Key, g => g.Select(kvp => kvp.Key));

        public static FrozenDictionary<TVal, IEnumerable<TKey>> Invert<TKey, TVal>(this FrozenDictionary<TKey, TVal> dict) => dict.Where(kvp => kvp.Value != null).GroupBy(kvp => kvp.Value).ToFrozenDictionary(g => g.Key, g => g.Select(kvp => kvp.Key));

        public static Dictionary<TVal, TKey> InvertSolo<TKey, TVal>(this Dictionary<TKey, TVal> dict) => dict.Where(kvp => kvp.Value != null).GroupBy(kvp => kvp.Value).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key).First());

        public static Dictionary<TVal, IEnumerable<(int x, int y)>> Invert<TVal>(this TVal[,] dict) => dict.Entries().GroupBy(kvp => kvp.Value).ToDictionary(g => g.Key, g => g.Select(kvp => kvp.Key));

        public static Dictionary<TVal, TCount> CountUniqueElements<TVal, TCount>(this IEnumerable<TVal> values) where TCount : IBinaryInteger<TCount> => values.GroupBy(v => v).ToDictionary(g => g.Key, g => TCount.CreateChecked(g.Count()));

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

        public static IEnumerable<(int x, int y)> KeysWithValue<TVal>(this TVal[,] array2d, TVal val) => array2d.Entries().Where(kvp => kvp.Value.Equals(val)).Select(kvp => kvp.Key);

        public static bool SmallerThanSeen(this IDictionary<int, int> cache, object key, int newValue)
        {
            var keyval = key.GetHashCode();
            if (cache.TryGetValue(keyval, out var val))
            {
                if (newValue >= val) return false;
            }
            cache[keyval] = newValue;
            return true;
        }

        public static IEnumerable<TSource> AppendMultiple<TSource>(this IEnumerable<TSource> source, params TSource[] elements) => source.Concat(elements);

        public static IEnumerable<T> Forever<T>(this IEnumerable<T> input)
        {
            while (true)
            {
                foreach (var i in input)
                {
                    yield return i;
                }
            }
        }

        public static (T First, T Second) Decompose2<T>(this IEnumerable<T> input) => (input.FirstOrDefault(), input.Skip(1).FirstOrDefault());

        public static (T First, T Second, T Third) Decompose3<T>(this IEnumerable<T> input) => (input.FirstOrDefault(), input.Skip(1).FirstOrDefault(), input.Skip(2).FirstOrDefault());

        public static IEnumerable<T> GetUniqueItems<T>(this IEnumerable<T> array)
        {
            var uniques = new HashSet<T>();
            var duplicates = new HashSet<T>();
            foreach (var item in array)
            {
                if (duplicates.Contains(item)) continue;
                if (uniques.Contains(item))
                {
                    duplicates.Add(item);
                    uniques.Remove(item);
                    continue;
                }
                uniques.Add(item);
            }
            return uniques;
        }

        public static IEnumerable<T> GetDuplicatedItems<T>(this IEnumerable<T> array)
        {
            var seen = new HashSet<T>();
            var duplicates = new HashSet<T>();
            foreach (var item in array)
            {
                if (seen.Contains(item))
                {
                    duplicates.Add(item);
                    continue;
                }
                seen.Add(item);
            }
            return duplicates;
        }

        public static IEnumerable<(TFirst, TSecond)> ZipLongest<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            using IEnumerator<TFirst> e1 = first.GetEnumerator();
            using IEnumerator<TSecond> e2 = second.GetEnumerator();
            var has1 = e1.MoveNext();
            var has2 = e2.MoveNext();
            while (has1 || has2)
            {
                yield return (has1 ? e1.Current : default, has2 ? e2.Current : default);
                has1 = e1.MoveNext();
                has2 = e2.MoveNext();
            }
        }

        public static (TResult min, TResult max) MinMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) where TResult : struct, IBinaryInteger<TResult>
        {
            TResult min, max;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                e.MoveNext();
                min = max = selector(e.Current);
                while (e.MoveNext())
                {
                    TResult x = selector(e.Current);
                    if (x < min)
                    {
                        min = x;
                    }
                    if (x > max)
                    {
                        max = x;
                    }
                }
            }

            return (min, max);
        }

        public static (T min, T max) MinMax<T>(this IEnumerable<T> source) where T : struct, IBinaryInteger<T>
        {
            T min, max;
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                e.MoveNext();
                min = max = e.Current;
                while (e.MoveNext())
                {
                    T x = e.Current;
                    if (x < min)
                    {
                        min = x;
                    }
                    if (x > max)
                    {
                        max = x;
                    }
                }
            }

            return (min, max);
        }

        public static T GetMax<T, U>(this IEnumerable<T> data, Func<T, U> f) where U : IComparable => data.Aggregate((i1, i2) => f(i1).CompareTo(f(i2)) > 0 ? i1 : i2);

        public static IEnumerable<TResult> SelectMultiple<TSource, TResult>(this IEnumerable<TSource> source, params Func<TSource, TResult>[] selectors)
        {
            foreach (var el in source)
            {
                foreach (var sel in selectors)
                {
                    yield return sel(el);
                }
            }
        }

        public static IEnumerable<string> WithoutNullOrWhiteSpace(this IEnumerable<string> input) => input.Where(line => !string.IsNullOrWhiteSpace(line));

        public static T[] SwapIndices<T>(this T[] array, int idx1, int idx2)
        {
            (array[idx1], array[idx2]) = (array[idx2], array[idx1]);
            return array;
        }

        public static List<T> MoveIndex<T>(this List<T> list, int idx1, int idx2)
        {
            T item = list[idx1];
            list.RemoveAt(idx1);
            list.Insert(idx2, item);
            return list;
        }

        public static T[] ToArray<T>(this (T, T) pair) => [pair.Item1, pair.Item1];

        public static void AddValues<T>(this List<T> list, params T[] input) => list.AddRange(input);

        public static IEnumerable<(int x, int y, char c)> Chars(this string[] lines)
        {
            for (int y = 0; y < lines.Length; ++y)
            {
                for (int x = 0; x < lines[y].Length; ++x)
                {
                    yield return (x, y, lines[y][x]);
                }
            }
        }

        public static (T value, int count) MostCommon<T>(this IEnumerable<T> vals)
        {
            var result = vals.GroupBy(v => v).MaxBy(g => g.Count());
            return (result.Key, result.Count());
        }

        public static (T value, int count) LeastCommon<T>(this IEnumerable<T> vals)
        {
            var result = vals.GroupBy(v => v).MinBy(g => g.Count());
            return (result.Key, result.Count());
        }

        public static IEnumerable<(int x, int y)> WithinBounds(this IEnumerable<(int x, int y)> input, int minX, int maxX, int minY, int maxY)
            => input.Where(p => p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY);

        public static IEnumerable<(T, T)> ZipTwo<T>(this IEnumerable<IEnumerable<T>> values)
        {
            if (values.Count() != 2) throw new ArgumentException("Outer array must have length 2");

            return Enumerable.Zip(values.First(), values.Skip(1).First());

        }

        public static int FindNthIndex<T>(this IEnumerable<T> enumerable, Predicate<T> match, int count)
        {
            var index = 0;

            foreach (var item in enumerable)
            {
                if (match.Invoke(item))
                    count--;
                if (count == 0)
                    return index;
                index++;
            }

            return -1;
        }

        public static Dictionary<TKey, TValue> WithReplacement<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey swapKey, TValue swapVal)
        {
            var newDict = source.ToDictionary();
            newDict[swapKey] = swapVal;
            return newDict;
        }

        public static TValue[] WithReplacement<TValue>(this TValue[] source, int swapKey, TValue swapVal)
        {
            var newArr = source.ToArray();
            newArr[swapKey] = swapVal;
            return newArr;
        }

        public static bool FindCycle<TFingerPrint>(this IDictionary<TFingerPrint, int> dict, TFingerPrint fingerprint, int currentIteration, int finalIteration, out int shortcutIteration)
        {
            if (dict.TryGetValue(fingerprint, out var previousSeen))
            {
                int cycle = currentIteration - previousSeen;
                shortcutIteration = ((finalIteration - previousSeen) % cycle) + currentIteration - 1;
                return true;
            }
            else
            {
                dict[fingerprint] = currentIteration;
                shortcutIteration = 0;
                return false;
            }
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<(TKey key, TValue val)> values)
        {
            foreach (var (key, val) in values)
            {
                dict.Add(key, val);
            }
        }

        public static IEnumerable<TSource> If<TSource>(this IEnumerable<TSource> source, bool condition, Func<IEnumerable<TSource>, IEnumerable<TSource>> branch) => condition ? branch(source) : source;

        public static (IEnumerable<T> True, IEnumerable<T> False) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var trueList = new List<T>();
            var falseList = new List<T>();

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    trueList.Add(item);
                }
                else
                {
                    falseList.Add(item);
                }
            }

            return (trueList, falseList);
        }
    }
}