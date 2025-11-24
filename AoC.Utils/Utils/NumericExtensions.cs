namespace AoC.Utils
{
    public static class NumericExtensions
    {
        public static string ToHex(this int v)
        {
            string res = $"{v:x}";
            if (res.Length == 1)
                return $"0{res}";

            return res;
        }

        // iterate over bits, returns sequence like 1,2,4,8 (only returning set bits in input)
        public static IEnumerable<T> BitSequence<T>(this T v) where T : IBinaryInteger<T>
        {
            for (T k = T.One; k <= v; k <<= 1)
                if ((v & k) > T.Zero)
                    yield return k;
        }

        public static IEnumerable<byte> BitSequence(this byte v)
        {
            if ((v & 1) > 0) yield return 1;
            if ((v & 2) > 0) yield return 2;
            if ((v & 4) > 0) yield return 4;
            if ((v & 8) > 0) yield return 8;
            if ((v & 16) > 0) yield return 16;
            if ((v & 32) > 0) yield return 32;
            if ((v & 64) > 0) yield return 64;
            if ((v & 128) > 0) yield return 128;
        }

        public static IEnumerable<T> BinarySequence<T>(this T v, T max = default) where T : IBinaryInteger<T>
        {
            max ??= v;
            for (T k = T.One; k <= max; k <<= 1)
                yield return ((v & k) > T.Zero) ? T.One : T.Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountBits(this uint num) => (int)System.Runtime.Intrinsics.X86.Popcnt.PopCount(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountBits(this ulong num) => (int)System.Runtime.Intrinsics.X86.Popcnt.X64.PopCount(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountBits(this int num) => int.PopCount(num);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PushBit<T>(this T num, bool bit) where T : IBinaryInteger<T>
        {
            num <<= 1;
            if (bit) num += T.One;
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ModWrap<T>(this ref T num, T rhs) where T : struct, IBinaryInteger<T>
        {
            num %= rhs;
            while (num < T.Zero) num += rhs;
        }

        public static IEnumerable<T> BinarySequenceBigEndian<T>(this T v, T start) where T : IBinaryInteger<T>
        {
            for (T i = start; i > T.Zero; i >>= 1)
                yield return (v & i) > T.Zero ? T.One : T.Zero;
        }

        public static string ToEngineeringNotation(this double d)
        {
            string fmt = "N2";
            double exponent = Math.Log10(Math.Abs(d));
            if (Math.Abs(d) >= 1)
            {
                return (int)Math.Floor(exponent) switch
                {
                    0 or 1 or 2 => d.ToString(),
                    3 or 4 or 5 => (d / 1e3).ToString(fmt) + "k",
                    6 or 7 or 8 => (d / 1e6).ToString(fmt) + "M",
                    9 or 10 or 11 => (d / 1e9).ToString(fmt) + "G",
                    12 or 13 or 14 => (d / 1e12).ToString(fmt) + "T",
                    15 or 16 or 17 => (d / 1e15).ToString(fmt) + "P",
                    18 or 19 or 20 => (d / 1e18).ToString(fmt) + "E",
                    21 or 22 or 23 => (d / 1e21).ToString(fmt) + "Z",
                    _ => (d / 1e24).ToString(fmt) + "Y",
                };
            }
            else if (Math.Abs(d) > 0)
            {
                return d.ToString(fmt);
                // switch ((int)Math.Floor(exponent))
                // {
                //     case -1: case -2: case -3:
                //         return (d * 1e3).ToString(fmt) + "m";
                //     case -4: case -5: case -6:
                //         return (d * 1e6).ToString(fmt) + "μ";
                //     case -7: case -8: case -9:
                //         return (d * 1e9).ToString(fmt) + "n";
                //     case -10: case -11: case -12:
                //         return (d * 1e12).ToString(fmt) + "p";
                //     case -13: case -14: case -15:
                //         return (d * 1e15).ToString(fmt) + "f";
                //     case -16: case -17: case -18:
                //         return (d * 1e18).ToString(fmt) + "a";
                //     case -19: case -20: case -21:
                //         return (d * 1e21).ToString(fmt) + "z";
                //     default:
                //         return (d * 1e24).ToString() + "y";
                // }
            }
            else
            {
                return "0";
            }
        }

        public static bool IsInteger(this double d) => (long)d == d;

        public static int SumOfFactors(this int n)
        {

            // Traversing through all prime factors.
            int res = 1;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {

                int curr_sum = 1;
                int curr_term = 1;

                while (n % i == 0)
                {

                    // THE BELOW STATEMENT MAKES
                    // IT BETTER THAN ABOVE METHOD
                    // AS WE REDUCE VALUE OF n.
                    n /= i;

                    curr_term *= i;
                    curr_sum += curr_term;
                }

                res *= curr_sum;
            }

            // This condition is to handle
            // the case when n is a prime
            // number greater than 2
            if (n > 2)
                res *= 1 + n;

            return res;
        }

        public static int ParseHex(this char c) => c switch
        {
            >= '0' and <= '9' => c - '0',
            >= 'A' and <= 'F' => c - 'A' + 10,
            >= 'a' and <= 'f' => c - 'a' + 10,
            _ => throw new Exception("Bad hex")
        };

        public static int Distance(this (int x, int y) a, (int x, int y) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        public static int Distance(this (int x, int y) a, int x, int y) => Math.Abs(a.x - x) + Math.Abs(a.y - y);
        public static int Distance(this (int x, int y, int z) a, (int x, int y, int z) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
        public static int Distance(this (int x, int y, int z) a, int x, int y, int z) => Math.Abs(a.x - x) + Math.Abs(a.y - y) + Math.Abs(a.z - z);
        public static int Distance(this (int x, int y, int z, int w) a, (int x, int y, int z, int w) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z) + Math.Abs(a.w - b.w);
        public static int Distance(this (int x, int y, int z, int w) a, int x, int y, int z, int w) => Math.Abs(a.x - x) + Math.Abs(a.y - y) + Math.Abs(a.z - z) + Math.Abs(a.w - w);
        public static int Length(this (int x, int y) v) => Math.Abs(v.x) + Math.Abs(v.y);
        public static int Length(this (int x, int y, int z) v) => Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z);
        public static int Length(this (int x, int y, int z, int w) v) => Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z) + +Math.Abs(v.w);

        public static (int x, int y) OffsetBy(this (int x, int y) left, (int x, int y) right) => (left.x + right.x, left.y + right.y);
        public static (int x, int y, int z) OffsetBy(this (int x, int y, int z) left, (int x, int y, int z) right) => (left.x + right.x, left.y + right.y, left.z + right.z);

        public static (int x, int y) OffsetBy(this (int x, int y) left, (int x, int y) right, int count) => (left.x + (right.x * count), left.y + (right.y * count));

        public static (int x, int y) OffsetBy(this (int x, int y) left, Direction2 right) => (left.x + right.DX, left.y + right.DY);
        public static (int x, int y) OffsetBy(this (int x, int y) left, Direction2 right, int count) => (left.x + (right.DX * count), left.y + (right.DY * count));

        public static (int x, int y) Subtract(this (int x, int y) left, (int x, int y) right) => (left.x - right.x, left.y - right.y);

        public static (int x, int y, int z) Subtract(this (int x, int y, int z) left, (int x, int y, int z) right) => (left.x - right.x, left.y - right.y, left.z - right.z);

        public static T Sum<T>(this IEnumerable<T> elements) where T : ISummable<T> => elements.Skip(1).Aggregate(elements.First(), (total, val) => total + val);

        public static TResult Sum<TSource, TResult>(this IEnumerable<TSource> elements, Func<TSource, TResult> selector) where TResult : ISummable<TResult> => elements.Skip(1).Aggregate(selector(elements.First()), (total, val) => total + selector(val));

        public static uint Sum(this IEnumerable<uint> elements) => elements.Skip(1).Aggregate(elements.First(), (total, val) => total + val);

        public static BigInteger Sum(this IEnumerable<BigInteger> elements) => elements.Skip(1).Aggregate(elements.First(), (total, val) => total + val);

    }

    public interface ISummable<TSelf> where TSelf : ISummable<TSelf>
    {
        public abstract static TSelf operator +(TSelf lhs, TSelf rhs);
    }
}