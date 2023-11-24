namespace AoC.Utils.Vectors
{
    // https://www.redblobgames.com/grids/hexagons/
    public abstract class HexVector(int x, int y, int z)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public int Z { get; set; } = z;

        public void Translate((int x, int y, int z) dir)
        {
            X += dir.x;
            Y += dir.y;
            Z += dir.z;
        }

        private static (int X, int Y, int Z) Subtract(HexVector a, HexVector b) => (a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static int Distance(HexVector a, HexVector b)
        {
            var vec = Subtract(a, b);
            return (Math.Abs(vec.X) + Math.Abs(vec.Y) + Math.Abs(vec.Z)) / 2;
        }

        public int Distance(HexVector other) => Distance(this, other);

        public override bool Equals(object obj)
        {
            return obj.GetType() == GetType() &&
                   obj is HexVector vector &&
                   X == vector.X &&
                   Y == vector.Y &&
                   Z == vector.Z;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    }

    // east/west sides
    public class HexVectorPointy(int x = 0, int y = 0, int z = 0) : HexVector(x, y, z)
    {
        public static readonly Dictionary<string, (int x, int y, int z)> Directions = new()
        {
            { "ne", (+1,-1,  0) },
            { "e",  (+1, 0, -1) },
            { "se", (0, +1, -1) },
            { "sw", (-1, +1, 0) },
            { "w",  (-1, 0, +1) },
            { "nw", (0, -1, +1) }
        };
        public HexVectorPointy TranslateHex(string dir) { Translate(Directions[dir]); return this; }

        public static implicit operator HexVectorPointy((int x, int y, int z) v) => new(v.x, v.y, v.z);
    };

    // flat topped north
    public class HexVectorFlat(int x = 0, int y = 0, int z = 0) : HexVector(x, y, z)
    {
        public static readonly Dictionary<string, (int x, int y, int z)> Directions = new()
        {
            { "ne", (+1,-1,  0) },
            { "se", (+1, 0, -1) },
            { "s",  (0, +1, -1) },
            { "sw", (-1, +1, 0) },
            { "nw", (-1, 0, +1) },
            { "n",  (0, -1, +1) }
        };

        public HexVectorFlat TranslateHex(string dir) { Translate(Directions[dir]); return this; }

        public static implicit operator HexVectorFlat((int x, int y, int z) v) => new(v.x, v.y, v.z);
    };

}
