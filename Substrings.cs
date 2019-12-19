using System;
using System.Collections.Generic;
using System.Linq;

public static class Substrings
{
    public static List<ArraySegment<char>> Get(char[] value)
    {
        var substrings = new List<ArraySegment<char>>(capacity: value.Length * (value.Length + 1) / 2 - 1);
        for (int length = 1; length < value.Length; length++)
            for (int start = 0; start <= value.Length - length; start++)
                substrings.Add(new ArraySegment<char>(value, start, length));
        return substrings;
    }
}