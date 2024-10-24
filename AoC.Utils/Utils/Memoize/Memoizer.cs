using System.Collections.Concurrent;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace AoC.Utils;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class Memoizer
{
    internal static ConditionalWeakTable<object, ConcurrentDictionary<string, object>> _weakCache = [];

    internal static TResult Memoize<T1, TResult>(object context, T1 arg, Func<T1, TResult> f, string cacheKey = null) where T1 : notnull
    {
        var objCache = _weakCache.GetOrCreateValue(context);

        var methodCache = (ConcurrentDictionary<T1, TResult>)objCache.GetOrAdd(cacheKey, _ => new ConcurrentDictionary<T1, TResult>());

        return methodCache.GetOrAdd(arg, f);
    }

    public static TResult Memoize<T1, TResult>(T1 arg, Func<T1, TResult> f, [CallerFilePath] string cacheFile = null, [CallerMemberName] string cacheKey = null)
        => Memoize(cacheFile, arg, f, cacheKey);
}

public static class MemoizerExtension
{
    public static TResult Memoize<T1, TResult>(this object context, T1 arg, Func<T1, TResult> f, [CallerMemberName] string cacheKey = null) where T1 : notnull
        => Memoizer.Memoize(context, arg, f, cacheKey);
}
