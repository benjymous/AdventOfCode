using System.Collections.Concurrent;

namespace AoC.Utils.Memoize;

public static class MemoizerExtension
{
    internal static ConditionalWeakTable<object, ConcurrentDictionary<string, object>> _weakCache = [];

    public static TResult Memoize<T1, TResult>(this object context, T1 arg, Func<T1, TResult> f, [CallerMemberName] string cacheKey = null) where T1 : notnull
    {
        var objCache = _weakCache.GetOrCreateValue(context);

        var methodCache = (ConcurrentDictionary<T1, TResult>)objCache.GetOrAdd(cacheKey, _ => new ConcurrentDictionary<T1, TResult>());

        return methodCache.GetOrAdd(arg, f);
    }
}
