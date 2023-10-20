using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace AoC.Utils.Vectors
{
    public class ManhattanVectorN
    {
        public ManhattanVectorN(int[] components)
        {
            Component = components.ToArray();
        }

        public ManhattanVectorN(string val)
        {
            Component = Util.ParseNumbers<int>(val);
        }

        public int[] Component = null;

        public int ComponentCount { get { return Component.Length; } }

        public override string ToString()
        {
            return string.Join(",", Component);
        }

        public void Set(params int[] newVal)
        {
            if (newVal.Length != ComponentCount) throw new Exception("Incompatible vectors");
            Component = newVal;
        }

        public void Offset(params int[] offset)
        {
            if (offset.Length != ComponentCount) throw new Exception("Incompatible vectors");
            for (int i = 0; i < offset.Length; ++i)
            {
                Component[i] += offset[i];
            }
        }

        protected void Offset(ManhattanVectorN offset)
        {
            Offset(offset.Component);
        }

        public static ManhattanVectorN operator +(ManhattanVectorN a, ManhattanVectorN b)
        {
            if (a.ComponentCount != b.ComponentCount) throw new Exception("Incompatible vectors");
            int[] newVal = (int[])(a.Component.Clone());
            for (int i = 0; i < a.ComponentCount; ++i)
            {
                newVal[i] += b.Component[i];
            }
            return new ManhattanVectorN(newVal);
        }

        public static ManhattanVectorN operator -(ManhattanVectorN a, ManhattanVectorN b)
        {
            if (a.ComponentCount != b.ComponentCount) throw new Exception("Incompatible vectors");
            int[] newVal = (int[])(a.Component.Clone());
            for (int i = 0; i < a.ComponentCount; ++i)
            {
                newVal[i] -= b.Component[i];
            }
            return new ManhattanVectorN(newVal);
        }

        public int Distance(ManhattanVectorN other)
        {
            if (other is null) return int.MaxValue;

            return Distance(other.Component);
        }

        public int Distance(params int[] other)
        {
            if (other.Length != ComponentCount) return int.MaxValue;

            int distance = 0;
            for (var i = 0; i < ComponentCount; ++i)
            {
                distance += Math.Abs(Component[i] - other[i]);
            }
            return distance;
        }

        public int Length
        {
            get
            {
                int distance = 0;
                for (var i = 0; i < ComponentCount; ++i)
                {
                    distance += Math.Abs(Component[i]);
                }
                return distance;
            }
        }

        public static bool operator ==(ManhattanVectorN v1, ManhattanVectorN v2) => v1.Equals(v2);

        public static bool operator !=(ManhattanVectorN v1, ManhattanVectorN v2) => !v1.Equals(v2);

        public override bool Equals(object other) => other is ManhattanVectorN && Distance(other as ManhattanVectorN) == 0;

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < ComponentCount; ++i)
                {
                    hash = hash * 31 + Component[i];
                }
                return hash;
            }
        }

    }

    public class ManhattanVector2 : ManhattanVectorN
    {
        public ManhattanVector2(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2");
        }


        [Regex("(.+)")]
        public ManhattanVector2(string val)
            : base(val)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2");
        }

        public ManhattanVector2(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2");
        }

        public ManhattanVector2((int x, int y) val)
            : base(new int[] { val.x, val.y })
        {
        }

        public int X { get => Component[0]; set => Component[0] = value; }
        public int Y { get => Component[1]; set => Component[1] = value; }

        public (int x, int y) AsSimple() => (X, Y);

        public int Distance((int x, int y) other) => other.Distance(X, Y);

        public static implicit operator ValueTuple<int, int>(ManhattanVector2 v) => v.AsSimple();
        public static implicit operator ManhattanVector2(ValueTuple<int, int> v) => new(v);

        public void Offset(Direction2 dir, int multiple = 1) => Offset(dir.DX * multiple, dir.DY * multiple);

        public void Offset(ManhattanVector2 dir, int multiple = 1) => Offset(dir.X * multiple, dir.Y * multiple);

        public void Offset(ManhattanVector2 dir)
        {
            X += dir.X;
            Y += dir.Y;
        }

        public void Offset(int x, int y)
        {
            X += x;
            Y += y;
        }

        public void TurnLeft() => Set(new int[] { Y, -X });

        public void TurnLeftBy(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            for (var i = 0; i < steps; ++i)
            {
                TurnLeft();
            }
        }

        public void TurnRight() => Set(new int[] { -Y, X });


        public void TurnRightBy(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            for (var i = 0; i < steps; ++i)
            {
                TurnRight();
            }
        }


        public static ManhattanVector2 operator +(ManhattanVector2 a, ManhattanVector2 b) => new((ManhattanVectorN)a + (ManhattanVectorN)b);

        public static ManhattanVector2 operator +(ManhattanVector2 a, Direction2 b)
        {
            var res = new ManhattanVector2(a.X, a.Y);
            res.Offset(b);
            return res;
        }

        public static ManhattanVector2 operator -(ManhattanVector2 a, ManhattanVector2 b) => new((ManhattanVectorN)a - (ManhattanVectorN)b);

        public ManhattanVector2 Delta(ManhattanVector2 other)
        {
            var delta = this - other;
            for (int i = 0; i < 2; ++i)
            {
                delta.Component[i] = Math.Sign(delta.Component[i]);
            }
            return delta;
        }

        public static readonly ManhattanVector2 Zero = new(0, 0);
    }

    public class ManhattanVector3 : ManhattanVectorN
    {
        public ManhattanVector3(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector3");
        }

        public ManhattanVector3((int x, int y, int z) val)
            : base(new int[] { val.x, val.y, val.z })
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector3");
        }

        [Regex("(.+)")]
        public ManhattanVector3(string val)
            : base(val)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector3");
        }

        public ManhattanVector3(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector3");
        }

        public int X { get => Component[0]; set => Component[0] = value; }
        public int Y { get => Component[1]; set => Component[1] = value; }
        public int Z { get => Component[2]; set => Component[2] = value; }

        public (int x, int y, int z) AsSimple() => (X, Y, Z);

        public int Distance((int x, int y, int z) other) => other.Distance(X, Y, Z);

        public static ManhattanVector3 operator +(ManhattanVector3 a, ManhattanVector3 b) => new(a + (ManhattanVectorN)b);
        public static ManhattanVector3 operator -(ManhattanVector3 a, ManhattanVector3 b) => new((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static readonly ManhattanVector3 Zero = new(0, 0, 0);
    }

    public class ManhattanVector4 : ManhattanVectorN
    {
        public ManhattanVector4(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4");
        }

        [Regex("(.+)")]
        public ManhattanVector4(string val)
            : base(val)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4");
        }

        public ManhattanVector4(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4");
        }

        public int X { get => Component[0]; set => Component[0] = value; }
        public int Y { get => Component[1]; set => Component[1] = value; }
        public int Z { get => Component[2]; set => Component[2] = value; }
        public int W { get => Component[3]; set => Component[3] = value; }

        public (int x, int y, int z, int w) AsSimple() => (X, Y, Z, W);

        public int Distance((int x, int y, int z, int w) other) => other.Distance(X, Y, Z, W);

        public static ManhattanVector4 operator +(ManhattanVector4 a, ManhattanVector4 b) => new(a + (ManhattanVectorN)b);
        public static ManhattanVector4 operator -(ManhattanVector4 a, ManhattanVector4 b) => new((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static readonly ManhattanVector4 Zero = new(0, 0, 0, 0);
    }


    public class Direction2
    {
        public int DX { get; set; } = 0;
        public int DY { get; set; } = 0;

        public Direction2(Direction2 dir)
        {
            DX = dir.DX;
            DY = dir.DY;
        }

        public Direction2(int dx, int dy)
        {
            DX = dx;
            DY = dy;
        }

        [Regex("(.)")]
        public Direction2(char dir)
        {
            SetDirection(dir switch
            {
                '>' or 'R' => East,
                '<' or 'L' => West,
                '^' or 'U' => North,
                'v' or 'D' => South,
                _ => throw new Exception("invalid char direction"),
            });
        }

        public Direction2 SetDirection(int dirx, int diry)
        {
            DX = dirx;
            DY = diry;
            return this;
        }

        public Direction2 SetDirection(Direction2 other)
        {
            DX = other.DX;
            DY = other.DY;
            return this;
        }

        public void FaceNorth() => SetDirection(0, -1);
        public void FaceSouth() => SetDirection(0, 1);
        public void FaceEast() => SetDirection(1, 0);
        public void FaceWest() => SetDirection(-1, 0);

        public Direction2 TurnLeft() => SetDirection(DY, -DX);
        public Direction2 TurnRight() => SetDirection(-DY, DX);
        public Direction2 Turn180() => SetDirection(-DX, -DY);

        public Direction2 TurnLeftByDegrees(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            return TurnLeftBySteps(steps);
        }

        public Direction2 TurnLeftBySteps(int steps) => (steps % 4) switch
        {
            0 => this,
            1 or -3 => TurnLeft(),
            2 or -2 => Turn180(),
            3 or -1 => TurnRight(),
            _ => throw new Exception("Shouldn't happen!"),
        };

        public Direction2 TurnRightByDegrees(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            return TurnRightBySteps(steps);
        }

        public Direction2 TurnRightBySteps(int steps) => (steps % 4) switch
        {
            0 => this,
            1 or -3 => TurnRight(),
            2 or -2 => Turn180(),
            3 or -1 => TurnLeft(),
            _ => throw new Exception("Shouldn't happen!"),
        };

        public static bool operator ==(Direction2 v1, Direction2 v2)
        {
            return v1.DX == v2.DX && v1.DY == v2.DY;
        }

        public static bool operator !=(Direction2 v1, Direction2 v2)
        {
            return v1.DX != v2.DX || v1.DY != v2.DY;
        }

        public static Direction2 operator +(Direction2 a, Direction2 b)
        {
            return new Direction2(b).TurnLeftBySteps(a.RotationSteps());
        }

        public static int operator -(Direction2 a, Direction2 b)
        {
            if (a == b) return 0;
            var t = new Direction2(a);
            var i = 0;
            while (t != b)
            {
                i++;
                t.TurnRight();
            }

            return i;
        }

        public static Direction2 operator +(Direction2 a, int b)
        {
            return new Direction2(a).TurnRightBySteps(b);
        }

        public static Direction2 operator -(Direction2 a, int b)
        {
            return new Direction2(a).TurnLeftBySteps(b);
        }

        public char AsChar()
        {
            if (DX > 0) return '>';
            if (DX < 0) return '<';
            if (DY < 0) return '^';
            return 'v';
        }

        public int RotationSteps()
        {
            if (DY < 0) return 0;
            if (DX > 0) return 1;
            if (DY > 0) return 2;
            return 3;
        }

        public static readonly Direction2 Null = new(0, 0);

        public static readonly Direction2 North = new(0, -1);
        public static readonly Direction2 South = new(0, 1);
        public static readonly Direction2 East = new(1, 0);
        public static readonly Direction2 West = new(-1, 0);

        public static readonly Direction2 Up = new(0, -1);
        public static readonly Direction2 Down = new(0, 1);
        public static readonly Direction2 Right = new(1, 0);
        public static readonly Direction2 Left = new(-1, 0);

        public static Direction2 FromChar(char c) => c switch
        {
            '>' or 'R' => East,
            '<' or 'L' => West,
            '^' or 'U' => North,
            'v' or 'D' => South,
            _ => throw new Exception("invalid char direction"),
        };

        public override string ToString()
        {
            return $"[{AsChar()}]{DX},{DY}";
        }

        public override bool Equals(object obj)
        {
            return obj is Direction2 && obj as Direction2 == this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DX, DY);
        }
    }


    public static class VectorCollectionExtensions
    {
        public static (int minZ, int maxZ, int minY, int maxY, int minX, int maxX) GetRange(this IEnumerable<ManhattanVector3> input)
        {
            var (minX, minY, minZ) = (int.MaxValue, int.MaxValue, int.MaxValue);
            var (maxX, maxY, maxZ) = (int.MinValue, int.MinValue, int.MinValue);

            foreach (var e in input)
            {
                maxX = Math.Max(maxX, e.X);
                minX = Math.Min(minX, e.X);
                maxY = Math.Max(maxY, e.Y);
                minY = Math.Min(minY, e.Y);
                maxZ = Math.Max(maxZ, e.Z);
                minZ = Math.Min(minZ, e.Z);
            }

            return (minZ, maxZ, minY, maxY, minX, maxX);
        }
    }

}