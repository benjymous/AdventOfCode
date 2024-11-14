using System.Security.Cryptography;

namespace AoC.Utils
{
    public static partial class StringExtensions
    {
        public static string[] SplitSections(this string input) => input.Split("\n\n");
        public static (string section1, string section2) DecomposeSections(this string input) => input.SplitSections().Decompose2();

        public static byte[] GetSHA256(this string inputString) => SHA256.HashData(Encoding.UTF8.GetBytes(inputString));

        public static string GetSHA256String(this string inputString)
        {
            StringBuilder sb = new();
            foreach (byte b in inputString.GetSHA256())
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetMD5(this string inputString) => MD5.HashData(Encoding.Default.GetBytes(inputString));

        public static string GetMD5String(this string inputString, bool lowerCase = false)
        {
            return string.Concat(Array.ConvertAll(inputString.GetMD5(),
                                       h => h.ToString(lowerCase ? "x2" : "X2")));
        }

        public static IEnumerable<char> GetMD5Chars(this string inputString) => Convert.ToHexString(inputString.GetMD5());

        public static IEnumerable<char> GetMD5Chars(this string inputString, int numBytes) => Convert.ToHexString(inputString.GetMD5(), 0, numBytes);

        public static IEnumerable<char> GetHexChars(this byte[] input, bool lowerCase = false)
        {
            foreach (byte b in input)
            {
                foreach (var c in b.ToString(lowerCase ? "x2" : "X2"))
                {
                    yield return c;
                }
            }
        }

        //public static uint GetCRC32(this string input) => Force.Crc32.Crc32CAlgorithm.Compute(Encoding.ASCII.GetBytes(input));

        private static readonly string vowels = "aeiouAEIOU";

        public static bool IsVowel(this char c) => vowels.Contains(c);

        public static bool IsCapitalLetter(this char c) => c is >= 'A' and <= 'Z';

        private static readonly string digits = "0123456789";

        public static bool IsDigit(this char c) => digits.Contains(c);

        public static int AsDigit(this char c) => c - '0';

        private static readonly string hexdigits = "0123456789abcdef";

        public static bool IsHex(this char c) => hexdigits.Contains(c);

        private static readonly string truthy = "yY1tT";
        public static bool AsBool(this char c) => truthy.Contains(c);

        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }

        public static string ReplaceAtIndex(this string str, int index, string from, string replacement)
        {
            var sb = new StringBuilder(str);
            sb.Remove(index, from.Length);
            sb.Insert(index, replacement);
            return sb.ToString();
        }

        public static string ReplaceFirst(this string str, string from, string replacement)
        {
            int pos = str.IndexOf(from);
            if (pos < 0)
            {
                return str;
            }
            return str.ReplaceAtIndex(pos, from, replacement);
        }

        public static string ReplaceLast(this string str, string from, string replacement)
        {
            int pos = str.LastIndexOf(from);
            if (pos < 0)
            {
                return str;
            }
            return str.ReplaceAtIndex(pos, from, replacement);
        }

        public static string WithoutMultipleSpaces(this string str) => MultipleSpaces().Replace(str, " ");

        [GeneratedRegex("\\s+")]
        private static partial Regex MultipleSpaces();

        // returns how similar two strings are - smaller = more similar
        public static int LevenshteinDistance(this string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // initialize the top and right of the table to 0, 1, 2, ...
            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 1; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = t[j - 1] == s[i - 1] ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

        public static string MultipleWithS(this int value, string str) => value == 1 ? str : $"{value} {str}s";

        public static string Reversed(this string str) => new(str.Reverse().ToArray());

        public static string Pop(this StringBuilder sb)
        {
            var res = sb.ToString();
            sb.Clear();
            return res;
        }

        public static bool TryGetIndex(this string str, char ch, out int index)
        {
            index = str.IndexOf(ch);
            return index != -1;
        }

        public static bool TryGetIndex(this string str, string sub, out int index)
        {
            index = str.IndexOf(sub);
            return index != -1;
        }
    }
}