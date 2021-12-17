using System;
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
            Component = bits.Select(s => int.Parse(s)).ToArray();
        }

        public int[] Component = null;

        public int ComponentCount { get { return Component.Length; } }

        public override string ToString()
        {
            return string.Join(",", Component);
        }

        private string[] Split(string val)
        {
            const string keep = "0192345678,-";
            StringBuilder sb = new StringBuilder();
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
            if (!(other is ManhattanVectorN)) return Int32.MaxValue;

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
            if (!(other is ManhattanVectorN)) return false;
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

        public int X { get { return Component[0]; } set { Component[0] = value; } }
        public int Y { get { return Component[1]; } set { Component[1] = value; } }

        public (int x, int y) AsSimple() => ( X, Y );

        public static explicit operator ValueTuple<int,int>(ManhattanVector2 v) => v.AsSimple();

        public void Offset(Direction2 dir, int multiple = 1)
        {
            Offset(dir.DX * multiple, dir.DY * multiple);
        }

        public void Offset(ManhattanVector2 dir, int multiple = 1)
        {
            Offset(dir.X * multiple, dir.Y * multiple);
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


        public static ManhattanVector2 operator +(ManhattanVector2 a, ManhattanVector2 b) => new ManhattanVector2((ManhattanVectorN)a + (ManhattanVectorN)b);

        public static ManhattanVector2 operator +(ManhattanVector2 a, Direction2 b)
        {
            var res = new ManhattanVector2(a);
            res.Offset(b);
            return res;
        }

        public static ManhattanVector2 operator -(ManhattanVector2 a, ManhattanVector2 b) => new ManhattanVector2((ManhattanVectorN)a - (ManhattanVectorN)b);



        public static ManhattanVector2 Zero = new ManhattanVector2(0, 0);
    }

    public class ManhattanVector3 : ManhattanVectorN
    {
        public ManhattanVector3(params int[] vals)
            : base(vals)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector2");
        }

        public ManhattanVector3(string val)
            : base(val)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector2");
        }

        public ManhattanVector3(ManhattanVectorN other)
            : base(other.Component)
        {
            if (ComponentCount != 3) throw new Exception("Invalid component count for Vector2");
        }

        public int X { get { return Component[0]; } set { Component[0] = value; } }
        public int Y { get { return Component[1]; } set { Component[1] = value; } }
        public int Z { get { return Component[2]; } set { Component[2] = value; } }

        public static ManhattanVector3 operator +(ManhattanVector3 a, ManhattanVector3 b) => new ManhattanVector3(a + (ManhattanVectorN)b);
        public static ManhattanVector3 operator -(ManhattanVector3 a, ManhattanVector3 b) => new ManhattanVector3((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static ManhattanVector3 Zero = new ManhattanVector3(0, 0, 0);
    }

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

        public static ManhattanVector4 operator +(ManhattanVector4 a, ManhattanVector4 b) => new ManhattanVector4(a + (ManhattanVectorN)b);
        public static ManhattanVector4 operator -(ManhattanVector4 a, ManhattanVector4 b) => new ManhattanVector4((ManhattanVectorN)a - (ManhattanVectorN)b);

        public static ManhattanVector4 Zero = new ManhattanVector4(0, 0, 0, 0);
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

        public void SetDirection(int dirx, int diry)
        {
            DX = dirx;
            DY = diry;
        }

        public void FaceNorth() => SetDirection(0, -1);
        public void FaceSouth() => SetDirection(0, 1);
        public void FaceEast() => SetDirection(1, 0);
        public void FaceWest() => SetDirection(-1, 0);

        public void TurnLeft()
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

            if (DX == 0 && DY == -1) SetDirection(1, 0);
            else if (DX == 1 && DY == 0) SetDirection(0, 1);
            else if (DX == 0 && DY == 1) SetDirection(-1, 0);
            else if (DX == -1 && DY == 0) SetDirection(0, -1);

            else throw new Exception("Unrecognised direction :" + DX + "," + DY);
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

        public char AsChar()
        {
            if (DX > 0) return '>';
            if (DX < 0) return '<';
            if (DY < 0) return '^';
            if (DY > 0) return 'v';

            throw new Exception("Unknown direction state");
        }

        public static Direction2 North = new Direction2(0, -1);
        public static Direction2 South = new Direction2(0, 1);
        public static Direction2 East = new Direction2(1, 0);
        public static Direction2 West = new Direction2(-1, 0);

        public override string ToString()
        {
            return $"{DX},{DY}";
        }
    }
}