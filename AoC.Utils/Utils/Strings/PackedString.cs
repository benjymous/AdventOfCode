namespace AoC.Utils.Strings
{
    public interface IStringPacker<TPacked>
    {
        static abstract TPacked Pack(string s);
        static abstract string Unpack(TPacked packed);
    }

    public class PackTwoCC : IStringPacker<ushort>
    {
        public static ushort Pack(string s) => Util.MakeTwoCC(s);
        public static string Unpack(ushort packed) => Util.UnMakeTwoCC(packed);
    }

    public class PackFourCC : IStringPacker<uint>
    {
        public static uint Pack(string s) => Util.MakeFourCC(s);
        public static string Unpack(uint packed) => Util.UnMakeFourCC(packed);
    }

    public class PackAlphaInt6 : IStringPacker<int>
    {
        public static int Pack(string s) => s.Select(c => c - 'a').Aggregate(0, (a, b) => (a << 5) + b + 1);
        public static string Unpack(int packed)
        {
            var v = packed;
            List<char> str = [];
            while (v > 0)
            {
                str.Add((char)('a' + ((v & 31) - 1)));
                v >>= 5;
            }

            str.Reverse();
            return str.AsString();
        }
    }

    public record struct PackedString<TNum, TPacker>(TNum V) : IComparable<PackedString<TNum, TPacker>> where TNum : IBinaryInteger<TNum> where TPacker : IStringPacker<TNum>
    {
        [Regex("(.+)")] public PackedString(string s) : this(TPacker.Pack(s)) { }

        public static implicit operator TNum(PackedString<TNum, TPacker> p) => p.V;
        public static implicit operator PackedString<TNum, TPacker>(TNum p) => new(p);
        public static implicit operator PackedString<TNum, TPacker>(string v) => new(v);

        public readonly TNum Value => V;

        readonly int IComparable<PackedString<TNum, TPacker>>.CompareTo(PackedString<TNum, TPacker> other) => V.CompareTo(other.V);

        public override readonly int GetHashCode() => V.GetHashCode();

        public override readonly string ToString() => TPacker.Unpack(V);
    }
}
