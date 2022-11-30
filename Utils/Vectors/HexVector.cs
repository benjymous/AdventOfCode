using System;
using System.Collections.Generic;

namespace AoC.Utils.Vectors
{
    // https://www.redblobgames.com/grids/hexagons/
    public class HexVector
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;

        public bool Pointy { get; set; } = true;

        public HexVector(int x, int y, int z, bool pointy = true)
        {
            X = x;
            Y = y;
            Z = z;
            Pointy = pointy;
        }

        // east/west sides
        static readonly Dictionary<string, (int x, int y, int z)> directions_pointy = new()
        {
            { "ne", (+1,-1, 0) },
            { "e",  (+1, 0, -1) },
            { "se", (0, +1, -1) },
            { "sw", (-1, +1, 0) },
            { "w",  (-1, 0, +1) },
            { "nw", (0, -1, +1) }
        };

        // flat topped north
        static readonly Dictionary<string, (int x, int y, int z)> directions_flat = new()
        {
            { "ne", (+1,-1, 0) },
            { "se", (+1, 0, -1) },
            { "s",  (0, +1, -1) },
            { "sw", (-1, +1, 0) },
            { "nw", (-1, 0, +1) },
            { "n",  (0, -1, +1) }
        };

        public Dictionary<string, (int x, int y, int z)> Directions
        {
            get => Pointy ? directions_pointy : directions_flat;
        }

        public void Translate((int x, int y, int z) dir)
        {
            X += dir.x;
            Y += dir.y;
            Z += dir.z;
        }

        public void TranslateHex(string dir) => Translate(Directions[dir]);

        private static (int X, int Y, int Z) Subtract(HexVector a, HexVector b) => (a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static int Distance(HexVector a, HexVector b)
        {
            var vec = Subtract(a, b);
            return (Math.Abs(vec.X) + Math.Abs(vec.Y) + Math.Abs(vec.Z)) / 2;
        }

        public int Distance(HexVector other) => Distance(this, other);

        public override bool Equals(object obj)
        {
            return obj is HexVector vector &&
                   X == vector.X &&
                   Y == vector.Y &&
                   Z == vector.Z &&
                   Pointy == vector.Pointy;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y, Z, Pointy);
    }
}
