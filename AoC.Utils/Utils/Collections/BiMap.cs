namespace AoC.Utils.Collections
{
    public class BiMap<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
    {
        readonly Dictionary<TKey, TValue> Dictionary = dictionary;
        readonly Dictionary<TValue, TKey> ReverseDict = dictionary.Invert().ToDictionary(val => val.Key, val => val.Value.First());

        public BiMap(IEnumerable<(TKey key, TValue value)> elements) : this(elements.ToDictionary(el => el.key, el => el.value))
        { }

        public void Add(TKey key, TValue value)
        {
            Dictionary[key] = value;
            ReverseDict[value] = key;
        }

        public bool Contains(TKey key) => Dictionary.ContainsKey(key);
        public bool Contains(TValue value) => ReverseDict.ContainsKey(value);

        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set
            {
                Dictionary[key] = value;
                ReverseDict[value] = key;
            }
        }

        public TKey this[TValue val]
        {
            get => ReverseDict[val];
            set
            {
                ReverseDict[val] = value;
                Dictionary[value] = val;
            }
        }

        public int Count => Dictionary.Count;

        public bool TryGet(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);
        public bool TryGet(TValue value, out TKey key) => ReverseDict.TryGetValue(value, out key);

        public IEnumerable<KeyValuePair<TKey, TValue>> Entries() => Dictionary.AsEnumerable();
        public IEnumerable<TKey> Keys => Dictionary.Keys;
        public IEnumerable<TValue> Values => Dictionary.Values;
        public IEnumerable<TValue> ValuesNonNull => ReverseDict.Keys;

    }

    public static class Extensions
    {
        public static BiMap<TKey, TValue> ToBiMap<TOther, TKey, TValue>(this IEnumerable<TOther> input, Func<TOther, TKey> keySelector, Func<TOther, TValue> valueSelector) => new(input.Select(item => (keySelector(item), valueSelector(item))));
    }
}
