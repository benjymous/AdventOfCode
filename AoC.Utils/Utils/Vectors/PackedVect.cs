namespace AoC.Utils.Vectors
{
    public interface ICoordinatePacker2<TNum>
    {
        static abstract TNum GetX(TNum v);
        static abstract TNum GetY(TNum v);
        static abstract TNum Set((TNum x, TNum y) v);
    }

    public interface ICoordinatePacker3<TNum>
    {
        static abstract TNum GetX(TNum v);
        static abstract TNum GetY(TNum v);
        static abstract TNum GetZ(TNum v);
        static abstract TNum Set((TNum x, TNum y, TNum z) v);
    }

    public class Pack16_16 : ICoordinatePacker2<int>
    {
        public static int GetX(int v) => v & 0xffff;
        public static int GetY(int v) => v >> 16;
        public static int Set((int x, int y) v) => v.x + ((v.y) << 16);
    }

    public class Pack8_8_16 : ICoordinatePacker3<int>
    {
        public static int GetX(int v) => v & 0xff;
        public static int GetY(int v) => (v >> 8) & 0xff;
        public static int GetZ(int v) => v >> 16;
        public static int Set((int x, int y, int z) v) => v.x + ((v.y) << 8) + ((v.z) << 16);
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

    public record struct PackedVect3<TNum, TPacker>(TNum V) : IComparable<PackedVect3<TNum, TPacker>> where TNum : IBinaryInteger<TNum> where TPacker : ICoordinatePacker3<TNum>
    {
        public readonly TNum X => TPacker.GetX(V);
        public readonly TNum Y => TPacker.GetY(V);
        public readonly TNum Z => TPacker.GetZ(V);
        public void Set(TNum X, TNum Y, TNum Z) => V = TPacker.Set((X, Y, Z));
        public static implicit operator TNum(PackedVect3<TNum, TPacker> p) => p.V;
        public static implicit operator PackedVect3<TNum, TPacker>(TNum p) => new(p);
        public static implicit operator PackedVect3<TNum, TPacker>((TNum x, TNum y, TNum z) v) => new(TPacker.Set(v));

        public static PackedVect3<TNum, TPacker> operator +(PackedVect3<TNum, TPacker> lhs, PackedVect3<TNum, TPacker> rhs) => new(lhs.V + rhs.V);
        public static PackedVect3<TNum, TPacker> operator -(PackedVect3<TNum, TPacker> lhs, PackedVect3<TNum, TPacker> rhs) => new(lhs.V - rhs.V);

        public static PackedVect3<TNum, TPacker> operator +(PackedVect3<TNum, TPacker> lhs, TNum rhs) => new(lhs.V + rhs);
        public static PackedVect3<TNum, TPacker> operator -(PackedVect3<TNum, TPacker> lhs, TNum rhs) => new(lhs.V - rhs);

        public static object Convert((TNum, TNum, TNum) p) => new PackedVect3<TNum, TPacker>(TPacker.Set(p));

        readonly int IComparable<PackedVect3<TNum, TPacker>>.CompareTo(PackedVect3<TNum, TPacker> other) => V.CompareTo(other.V);

        public override readonly int GetHashCode() => V.GetHashCode();
    }

    public static class Extensions
    {
        public static int Distance(this PackedPos32 a, PackedPos32 b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
