using System.Collections.Generic;
using System.Linq;

public static class Permutations
{
    public static IEnumerable<IEnumerable<T>> Get<T>(IEnumerable<T> set, IEnumerable<T> subset = null)
    {
        if (subset == null) subset = new T[] { };
        if (!set.Any()) yield return subset;

        for (var i = 0; i < set.Count(); i++)
        {
            var newSubset = set.Take(i).Concat(set.Skip(i + 1));
            foreach (var permutation in Get(newSubset, subset.Concat(set.Skip(i).Take(1))))
            {
                yield return permutation;
            }
        }
    }
}