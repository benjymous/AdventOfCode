using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
