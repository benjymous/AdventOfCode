using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AoC.Utils.Vectors
{

    public interface IVec
    {
        int Distance(IVec other);
    }


    public class ManhattanVectorN : IVec
    {
        public ManhattanVectorN(int[] components)
        {
            Component = components.ToArray();
        }

        public ManhattanVectorN(string val)
        {
            var bits = Split(val);
            Component = bits.Select(int.Parse).ToArray();
        }

        public int[] Component = null;

        public int ComponentCount { get { return Component.Length; } }

        public override string ToString()
        {
            return string.Join(",", Component);
        }

        private static string[] Split(string val)
        {
            const string keep = "0192345678,-";
            StringBuilder sb = new();
            foreach (var c in val)
            {
                if (keep.Contains(c)) sb.Append(c);
            }
            return sb.ToString().Split(",");
        }

        public void Set(params int[] newVal)
        {
            if (newVal.Length != ComponentCount) throw new System.Exception("Incompatible vectors");
            Component = newVal;
        }

        public void Offset(params int[] offset)
        {
            if (offset.Length != ComponentCount) throw new System.Exception("Incompatible vectors");
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
            if (a.ComponentCount != b.ComponentCount) throw new System.Exception("Incompatible vectors");
            int[] newVal = (int[])(a.Component.Clone());
            for (int i = 0; i < a.ComponentCount; ++i)
            {
                newVal[i] += b.Component[i];
            }
            return new ManhattanVectorN(newVal);
        }

        public static ManhattanVectorN operator -(ManhattanVectorN a, ManhattanVectorN b)
        {
            if (a.ComponentCount != b.ComponentCount) throw new System.Exception("Incompatible vectors");
            int[] newVal = (int[])(a.Component.Clone());
            for (int i = 0; i < a.ComponentCount; ++i)
            {
                newVal[i] -= b.Component[i];
            }
            return new ManhattanVectorN(newVal);
        }

        public int Distance(IVec other)
        {
            if (other is not ManhattanVectorN) return Int32.MaxValue;

            var man2 = other as ManhattanVectorN;

            return Distance(man2.Component);
        }

        public int Distance(params int[] other)
        {
            if (other.Length != ComponentCount) return Int32.MaxValue;

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

        public static bool operator ==(ManhattanVectorN v1, ManhattanVectorN v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(ManhattanVectorN v1, ManhattanVectorN v2)
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object other)
        {
            if (other is not ManhattanVectorN) return false;
            return Distance(other as ManhattanVectorN) == 0;
        }

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

    [TypeConverter(typeof(ManhattanVector2TypeConverter))]
    public class ManhattanVector2 : ManhattanVectorN
    {
        public ManhattanVector2(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 2) throw new Exception("Invalid component count for Vector2");
        }

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
            : base(new int[] {val.x, val.y})
        {
        }

        public int X { get { return Component[0]; } set { Component[0] = value; } }
        public int Y { get { return Component[1]; } set { Component[1] = value; } }

        public (int x, int y) AsSimple() => (X, Y);

        public static implicit operator ValueTuple<int, int>(ManhattanVector2 v) => v.AsSimple();
        public static implicit operator ManhattanVector2(ValueTuple<int, int> v) => new(v);

        public static bool operator ==(ManhattanVector2 v1, (int x, int y) v2)
        {
            return v1.X == v2.x && v1.Y == v2.y;
        }

        public static bool operator !=(ManhattanVector2 v1, (int x, int y) v2)
        {
            return v1.X != v2.x || v1.Y != v2.y;
        }

        public void Offset(Direction2 dir, int multiple = 1)
        {
            Offset(dir.DX * multiple, dir.DY * multiple);
        }

        public void Offset(ManhattanVector2 dir, int multiple = 1)
        {
            Offset(dir.X * multiple, dir.Y * multiple);
        }

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

        public void TurnLeft()
        {
            // up :  0,-1 ->  -1,0;
            // left: -1,0 -> 0,1
            // down: 0,1 -> 1,0
            // right: 1,0 -> 0,-1
            Set(new int[] { Y, -X });
        }

        public void TurnLeftBy(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            for (var i = 0; i < steps; ++i)
            {
                TurnLeft();
            }
        }

        public void TurnRight()
        {
            // up :  0,-1 ->  1,0;
            // right: 1,0 -> 0,1
            // down: 0,1 -> -1,0
            // left: -1,0 -> 0,-1

            Set(new int[] { -Y, X });
        }


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



        public static readonly ManhattanVector2 Zero = new(0, 0);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [TypeConverter(typeof(ManhattanVector3TypeConverter))]
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

        public int X { get { return Component[0]; } set { Component[0] = value; } }
        public int Y { get { return Component[1]; } set { Component[1] = value; } }
        public int Z { get { return Component[2]; } set { Component[2] = value; } }

        public (int x, int y, int z) AsSimple() => (X, Y, Z);

        public static ManhattanVector3 operator +(ManhattanVector3 a, ManhattanVector3 b) => new(a + (ManhattanVectorN)b);
        public static ManhattanVector3 operator -(ManhattanVector3 a, ManhattanVector3 b) => new((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static readonly ManhattanVector3 Zero = new(0, 0, 0);
    }

    [TypeConverter(typeof(ManhattanVector4TypeConverter))]
    public class ManhattanVector4 : ManhattanVectorN
    {
        public ManhattanVector4(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 4) throw new Exception("Invalid component count for Vector4");
        }

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

        public int X { get { return Component[0]; } set { Component[0] = value; } }
        public int Y { get { return Component[1]; } set { Component[1] = value; } }
        public int Z { get { return Component[2]; } set { Component[2] = value; } }
        public int W { get { return Component[3]; } set { Component[3] = value; } }

        public (int x, int y, int z, int w) AsSimple() => (X, Y, Z, W);

        public static ManhattanVector4 operator +(ManhattanVector4 a, ManhattanVector4 b) => new(a + (ManhattanVectorN)b);
        public static ManhattanVector4 operator -(ManhattanVector4 a, ManhattanVector4 b) => new((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static readonly ManhattanVector4 Zero = new(0, 0, 0, 0);
    }


    #region typeconverters
    public class ManhattanVector2TypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new ManhattanVector2(value as string);
        }
    }

    public class ManhattanVector3TypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new ManhattanVector3(value as string);
        }
    }

    public class ManhattanVector4TypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new ManhattanVector4(value as string);
        }
    }

    #endregion

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

        public void SetDirection(int dirx, int diry)
        {
            DX = dirx;
            DY = diry;
        }

        public void FaceNorth() => SetDirection(0, -1);
        public void FaceSouth() => SetDirection(0, 1);
        public void FaceEast() => SetDirection(1, 0);
        public void FaceWest() => SetDirection(-1, 0);

        public Direction2 TurnLeft()
        {
            // up :  0,-1 ->  -1,0;
            // left: -1,0 -> 0,1
            // down: 0,1 -> 1,0
            // right: 1,0 -> 0,-1

            if (DX == 0 && DY == -1) SetDirection(-1, 0);
            else if (DX == -1 && DY == 0) SetDirection(0, 1);
            else if (DX == 0 && DY == 1) SetDirection(1, 0);
            else if (DX == 1 && DY == 0) SetDirection(0, -1);

            else throw new Exception("Unrecognised direction: " + DX + "," + DY);

            return this;
        }

        public void TurnLeftByDegrees(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            TurnLeftBySteps(steps);
        }
        public void TurnLeftBySteps(int steps)
        {
            for (var i = 0; i < steps; ++i)
            {
                TurnLeft();
            }
        }

        public Direction2 TurnRight()
        {
            // up :  0,-1 ->  1,0;
            // right: 1,0 -> 0,1
            // down: 0,1 -> -1,0
            // left: -1,0 -> 0,-1

            if (DX == 0 && DY == -1) SetDirection(1, 0);
            else if (DX == 1 && DY == 0) SetDirection(0, 1);
            else if (DX == 0 && DY == 1) SetDirection(-1, 0);
            else if (DX == -1 && DY == 0) SetDirection(0, -1);

            else throw new Exception("Unrecognised direction :" + DX + "," + DY);

            return this;
        }

        public Direction2 Turn180()
        {
            SetDirection(-DX, -DY);
            return this;
        }


        public void TurnRightByDegrees(int degrees)
        {
            if (degrees % 90 != 0) throw new Exception("expected multiple of 90 degrees: " + degrees);

            var steps = degrees / 90;

            TurnRightBySteps(steps);
        }

        public void TurnRightBySteps(int steps)
        {
            for (var i = 0; i < steps; ++i)
            {
                TurnRight();
            }
        }

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
            var t1 = new Direction2(a);
            var t2 = new Direction2(b);

            while (t1 != Up)
            {
                t1.TurnRight();
                t2.TurnRight();
            }
            return t2;
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


        public char AsChar()
        {
            if (DX > 0) return '>';
            if (DX < 0) return '<';
            if (DY < 0) return '^';
            if (DY > 0) return 'v';

            return '?';
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

        public override string ToString()
        {
            return $"{DX},{DY}";
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
}