namespace AoC.Utils.Vectors
{
    public interface ICoordinatePacker2<TNum>
    {
        static abstract TNum GetX(TNum v);
        static abstract TNum GetY(TNum v);
        static abstract TNum Set((TNum x, TNum y) v);
    }

    public class Pack16_16 : ICoordinatePacker2<int>
    {
        public static int GetX(int v) => v & 0xffff;
        public static int GetY(int v) => v >> 16;
        public static int Set((int x, int y) v) => v.x + ((v.y) << 16);
    }

    public record struct PackedVect2<TNum, TPacker>(TNum V) : IComparable<PackedVect2<TNum, TPacker>> where TNum : IBinaryInteger<TNum> where TPacker : ICoordinatePacker2<TNum>
    {
        public readonly TNum X => TPacker.GetX(V);
        public readonly TNum Y => TPacker.GetY(V);
        public static implicit operator TNum(PackedVect2<TNum, TPacker> p) => p.V;
        public static implicit operator PackedVect2<TNum, TPacker>(TNum p) => new(p);
        public static implicit operator PackedVect2<TNum, TPacker>((TNum x, TNum y) v) => new(TPacker.Set(v));

        public static PackedVect2<TNum, TPacker> operator +(PackedVect2<TNum, TPacker> lhs, PackedVect2<TNum, TPacker> rhs) => new(lhs.V + rhs.V);
        public static PackedVect2<TNum, TPacker> operator -(PackedVect2<TNum, TPacker> lhs, PackedVect2<TNum, TPacker> rhs) => new(lhs.V - rhs.V);

        public static PackedVect2<TNum, TPacker> operator +(PackedVect2<TNum, TPacker> lhs, TNum rhs) => new(lhs.V + rhs);
        public static PackedVect2<TNum, TPacker> operator -(PackedVect2<TNum, TPacker> lhs, TNum rhs) => new(lhs.V - rhs);

        public static object Convert((TNum, TNum) p) => new PackedVect2<TNum, TPacker>(TPacker.Set(p));

        readonly int IComparable<PackedVect2<TNum, TPacker>>.CompareTo(PackedVect2<TNum, TPacker> other) => V.CompareTo(other.V);

        public override readonly int GetHashCode() => V.GetHashCode();
    }

    public static class Extensions
    {
        public static int Distance(this PackedPos32 a, PackedPos32 b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
