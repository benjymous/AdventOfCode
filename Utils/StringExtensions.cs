using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AoC.Utils
{
    public static class StringExtensions
    {
        public static byte[] GetSHA256(this string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetSHA256String(this string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetSHA256(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetMD5(this string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetMD5String(this string inputString, bool lowerCase=false)
        {
            return string.Concat(Array.ConvertAll(GetMD5(inputString),
                                       h => h.ToString(lowerCase ? "x2" : "X2")));
        }

        public static IEnumerable<char> GetMD5Chars(this string inputString)
        {
            foreach (byte b in GetMD5(inputString))
            {
                foreach (var c in b.ToString("X2"))
                {
                    yield return c;
                }
            }
        }

        private static string vowels = "aeiouAEIOU";

        public static bool IsVowel(this char c)
        {
            return vowels.Contains(c);
        }

        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
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
            return ReplaceAtIndex(str, pos, from, replacement);
        }

        public static string ReplaceLast(this string str, string from, string replacement)
        {
            int pos = str.LastIndexOf(from);
            if (pos < 0)
            {
                return str;
            }
            return ReplaceAtIndex(str, pos, from, replacement);
        }


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
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    int min1 = d[i - 1, j] + 1;
                    int min2 = d[i, j - 1] + 1;
                    int min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            return d[n, m];
        }

        public static string Reversed(this string str)
        {
            return new string(str.Reverse().ToArray());
        }

        public static IEnumerable<bool> BitSequenceFromHex(this string str)
        {
            foreach (var ch in str)
            {
                var b = Convert.ToString(Convert.ToInt32(ch.ToString(), 16), 2).PadLeft(4, '0');
                foreach (var bch in b)
                {
                    yield return bch == '1';
                }
            }
        }


    }


}