using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Utils.Collections
{
    class UniqueMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public UniqueMap(int count = 0)
        {
            seen = new(count);
            values = new(count);
        }

        readonly HashSet<TKey> seen;
        readonly Dictionary<TKey, TValue> values;

        public bool UniqueAdd(TKey key, TValue value)
        {
            if (seen.Contains(key))
            {
                values.Remove(key);
                return false;
            }
            else
            {
                seen.Add(key);
                values.Add(key, value);
                return true;
            }
        }

        public void Reset()
        {
            seen.Clear();
            values.Clear();
        }

        public bool Any() => values.Any();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IEnumerable<KeyValuePair<TKey, TValue>>)values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)values).GetEnumerator();

        public IEnumerable<TKey> Keys => values.Keys;
        public IEnumerable<TValue> Values => values.Values;
    }
}
