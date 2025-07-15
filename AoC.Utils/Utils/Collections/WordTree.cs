namespace AoC.Utils.Collections
{
    public class WordTree
    {
        class WordNode
        {
            readonly Dictionary<char, WordNode> Children = [];
            bool isWord = false;

            internal bool IsWord(IEnumerable<char> sequence)
            {
                if (!sequence.Any()) return isWord;

                return Children.TryGetValue(sequence.First(), out var child) && child.IsWord(sequence.Skip(1));
            }

            internal IEnumerable<int> GetPrefixes(IEnumerable<char> sequence, int count = 0)
            {
                var rest = sequence.Any() && Children.TryGetValue(sequence.First(), out var child) ?
                    child.GetPrefixes(sequence.Skip(1), count + 1) : [];

                return isWord ? [.. rest, count] : rest;
            }

            internal void Add(IEnumerable<char> sequence)
            {
                if (!sequence.Any())
                {
                    isWord = true;
                }
                else
                {
                    char nextNode = sequence.First();
                    if (!Children.ContainsKey(nextNode)) Children.Add(nextNode, new WordNode());
                    Children[nextNode].Add(sequence.Skip(1));
                }
            }
        }

        readonly WordNode Root = new();
        public int Count { get; private set; } = 0;

        public WordTree() { }

        public WordTree(IEnumerable<string> words)
        {
            foreach (var w in words) AddWord(w);
        }

        public void AddWord(string word)
        {
            Count++;
            Root.Add(word);
        }

        public bool IsWord(string word) => Root.IsWord(word);

        public IEnumerable<int> GetPrefixes(IEnumerable<char> word) => Root.GetPrefixes(word);
    }
}
