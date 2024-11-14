namespace AoC.Advent2022;
public class Day13 : IPuzzle
{
    class Element : IComparable<Element>
    {
        public Element(string data)
        {
            Data = data;
            if (int.TryParse(data, out var v)) Value = v;
        }

        readonly string Data;
        readonly int Value = -1;

        Element[] _children;
        Element[] Children => _children ??= Tokenize(Data).ToArray();

        bool IsList => Value == -1;
        public bool IsMarker { get; init; }

        static IEnumerable<Element> Tokenize(string data)
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

        static int CompareList(Element[] lhs, Element[] rhs)
        {
            for (int i = 0; true; ++i)
            {
                if (lhs.Length == i || rhs.Length == i) return lhs.Length - rhs.Length;
                var compare = lhs[i].CompareTo(rhs[i]);
                if (compare != 0) return compare;
            }
        }
    }

    public static int Part1(string input)
    {
        return input.SplitSections()
                    .Select(group => Util.Parse<Element>(group).TakePair())
                    .WithIndex(1)
                    .Where(e => e.Value.Item1.CompareTo(e.Value.Item2) < 0)
                    .Sum(e => e.Index);
    }

    public static int Part2(string input)
    {
        return (int)Util.Parse<Element>(input.Replace("\n\n", "\n"))
                        .AppendMultiple(new("[[2]]") { IsMarker = true }, new("[[6]]") { IsMarker = true })
                        .Order()
                        .WithIndex(1)
                        .Where(e => e.Value.IsMarker)
                        .Product(e => e.Index);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}