namespace AoC.Utils.AdaptorTypes;

[Regex("^[a-z]{1,6}$")]
public class StringInt32(string input)
{
    public int Value { get; } = input.Select(c => c - 'a').Aggregate(0, (a, b) => (a << 5) + b + 1);

    private string Decode()
    {
        var v = Value;
        List<char> str = [];
        while (v > 0)
        {
            str.Add((char)('a' + ((v & 31) - 1)));
            v >>= 5;
        }

        str.Reverse();
        return str.AsString();
    }

    public static implicit operator string(StringInt32 value) => value.Decode();
    public static implicit operator StringInt32(string value) => new(value);

    public override string ToString() => $"{Decode()} [{Value}]";
    public override int GetHashCode() => Value;
}
