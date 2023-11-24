using System.Collections;

namespace AoC.Utils.Collections
{
    public class UniqueMap<TKey, TValue>(int count = 0) : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        readonly HashSet<TKey> seen = new(count);
        readonly Dictionary<TKey, TValue> values = new(count);

        public bool UniqueAdd(TKey key, TValue value)
        {
            if (seen.Add(key))
            {
                // new item
                values.Add(key, value);
                return true;
            }
            else
            {
                // not unique
                values.Remove(key);
                return false;
            }
        }

        public void Reset()
        {
            seen.Clear();
            values.Clear();
        }

        public bool Any() => values.Count != 0;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)values).GetEnumerator();

        public IEnumerable<TKey> Keys => values.Keys;
        public IEnumerable<TValue> Values => values.Values;
    }
}
