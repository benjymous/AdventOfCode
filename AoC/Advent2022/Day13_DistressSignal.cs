namespace AoC.Advent2022;
public class Day13 : IPuzzle
{
    public class Element : IComparable<Element>
    {
        [Regex("(.+)")]
        public Element(string data)
        {
            Data = data;
            if (int.TryParse(data, out var v)) Value = v;
        }

        private readonly string Data;
        private readonly int Value = -1;

        private Element[] _children;
        private Element[] Children => _children ??= [.. Tokenize(Data)];

        private bool IsList => Value == -1;
        public bool IsMarker { get; init; }

        private static IEnumerable<Element> Tokenize(string data)
        {
            StringBuilder token = new(data.Length);
            int depth = 0;
            foreach (var c in data[1..^1]) // strip outer []s
            {
                if (c == '[') // open bracket, so we're a level deeper
                    depth++;
                else if (c == ']') // close bracket, so back up a level
                    depth--;

                if (c == ',' && depth == 0) // comma at the top level, so return next element
                    yield return new(token.Pop());
                else // append to our current token
                    token.Append(c);
            }
            if (token.Length > 0) yield return new(token.Pop());
        }

        public Element Promote() => new($"[{Value}]");

        public int CompareTo(Element other) => (IsList, other.IsList) switch
        {
            (true, true) => CompareList(Children, other.Children), // both lists
            (false, false) => Value - other.Value,                 // both numbers
            (true, false) => CompareTo(other.Promote()),           // list, non-list
            (false, true) => Promote().CompareTo(other),           // non-list, list
        };

        private static int CompareList(Element[] lhs, Element[] rhs)
        {
            for (int i = 0; true; ++i)
            {
                if (lhs.Length == i || rhs.Length == i) return lhs.Length - rhs.Length;
                var compare = lhs[i].CompareTo(rhs[i]);
                if (compare != 0) return compare;
            }
        }

        public override string ToString() => Data;
    }

    public static int Part1(string input)
    {
        return input.SplitSections()
                    .Select(group => Parser.Parse<Element>(group).ToArray().TakePair())
                    .Index(1)
                    .Where(e => e.Item.Item1.CompareTo(e.Item.Item2) < 0)
                    .Sum(e => e.Index);
    }

    public static int Part2(string input)
    {
        return (int)Parser.Parse<Element>(input.Replace("\n\n", "\n"))
                        .AppendMultiple(new("[[2]]") { IsMarker = true }, new("[[6]]") { IsMarker = true })
                        .Order()
                        .Index(1)
                        .Where(e => e.Item.IsMarker)
                        .Product(e => e.Index);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}