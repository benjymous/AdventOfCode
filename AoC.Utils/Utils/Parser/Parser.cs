using System.ComponentModel;
using System.Reflection;

namespace AoC.Utils.Parser
{
    public static class Parser
    {
        public static T FromString<T>(string input) => (T)ConvertFromString(typeof(T), input);

        public static IEnumerable<T> Parse<T>(IEnumerable<string> input)
        {
            var constructors = GetPotentialConstructors(typeof(T));
            return input.WithoutNullOrWhiteSpace()
                 .Select(line => RegexCreate<T>(line, constructors));
        }

        public static IEnumerable<T> Parse<T>(string input, string splitter = "\n")
            => Parse<T>(input.Split(splitter));

        public static IEnumerable<T> Factory<T, FT>(IEnumerable<string> input, FT factory) where FT : class
            => input.WithoutNullOrWhiteSpace()
                 .Select(line => RegexFactoryCreate<T, FT>(line, factory));

        public static IEnumerable<T> Factory<T, FT>(string input, FT factory) where FT : class, IAutoSplit
            => Factory<T, FT>(Split<FT>(input), factory);

        public static IEnumerable<T> Factory<T, FT>(string input, FT factory = null, string splitter = "\n") where FT : class
            => Factory<T, FT>(input.Split(splitter), factory);

        public static FT Factory<FT>(string input, string splitter = "\n") where FT : class, new()
            => Factory<FT>(input.Split(splitter).WithoutNullOrWhiteSpace());

        public static FT Factory<FT>(IEnumerable<string> input) where FT : class, new()
        {
            FT factory = new();

            Factory(input, factory);

            return factory;
        }

        public static void Factory<FT>(string input, FT factory, string splitter = "\n") where FT : class
            => Factory(input.Split(splitter).WithoutNullOrWhiteSpace(), factory);

        public static void Factory<FT>(IEnumerable<string> input, FT factory) where FT : class
        {
            foreach (var line in input)
            {
                RegexFactoryCreate<object, FT>(line, factory);
            }
        }

        public class AutoArray<T>(string data) : IEnumerable<T>
        {
            readonly T[] Resolved = [.. Parse<T>(data)];

            public int Length => Resolved.Length;

            public T this[int index] => Resolved[index];

            public static implicit operator AutoArray<T>(string s) => new(s);
            public static implicit operator T[](AutoArray<T> a) => a.Resolved;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Resolved).GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Resolved.GetEnumerator();

            public int IndexOf(T item) => Array.IndexOf(Resolved, item);
        }

        public class AutoArray<DT, FT>(string data) : IEnumerable<DT> where FT : class
        {
            public DT[] Resolved = [.. Factory<DT, FT>(data)];

            public int Length => Resolved.Length;

            public DT this[int index] => Resolved[index];

            public IEnumerator<DT> GetEnumerator() => ((IEnumerable<DT>)Resolved).GetEnumerator();
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Resolved.GetEnumerator();

            public static implicit operator AutoArray<DT, FT>(string s) => new(s);
            public static implicit operator DT[](AutoArray<DT, FT> a) => a.Resolved;

            public int IndexOf(DT item) => Array.IndexOf(Resolved, item);
        }

        public interface IAutoSplit { }

        #region Internals

        static List<(MethodInfo func, RegexAttribute attr)> GetRegexMethods<FT>()
        {
            return Memoizer.Memoize(typeof(FT), _ =>
            {
                var res = new List<(MethodInfo, RegexAttribute)>();
                foreach (var func in typeof(FT).GetMethods())
                {
                    if (func.GetCustomAttributes(typeof(RegexAttribute), true)
                        .FirstOrDefault() is RegexAttribute attribute) res.Add((func, attribute));
                }
                return res;
            });
        }

        static IEnumerable<string> Split<FT>(string input) where FT : IAutoSplit
        {
            var combinedRegex = $"({string.Join("|", GetRegexMethods<FT>().Select(v => v.attr.Regex.ToString()))})";

            return Regex.Matches(input, combinedRegex).Select(m => m.Groups[0].Captures[0].Value);
        }

        static T RegexFactoryCreate<T, FT>(string line, FT factory) where FT : class
        {
            foreach (var (func, attr) in GetRegexMethods<FT>())
            {
                var matches = attr.Regex.Matches(line); // match this function against the input line

                if (matches.Count == 0) continue; // Try other functions to see if they match

                var paramInfo = func.GetParameters();

                object[] convertedParams = ConstructParams(matches, paramInfo);

                var res = func.Invoke(factory, convertedParams);
                return res != null ? (T)res : default;
            }
            throw new Exception($"RegexFactory failed to find suitable factory function for {line}");
        }

        static List<(ConstructorInfo tc, RegexAttribute attr)> GetPotentialConstructors(Type t)
        {
            return Memoizer.Memoize(t, _ =>
            {
                if (t.GetCustomAttribute<RegexAttribute>() is RegexAttribute ownAttr)
                {
                    return [(t.GetConstructors().First(), ownAttr)];
                }
                else
                {
                    var res = new List<(ConstructorInfo tc, RegexAttribute attr)>();
                    foreach (var typeConstructor in t.GetConstructors())
                    {
                        if (typeConstructor?.GetCustomAttributes(typeof(RegexAttribute), true)
                            .FirstOrDefault() is RegexAttribute attribute) res.Add((typeConstructor, attribute));
                    }
                    return res;
                }
            });
        }

        static T RegexCreate<T>(string line, List<(ConstructorInfo tc, RegexAttribute attr)> potentialConstructors = null) => (T)RegexCreate(typeof(T), line, potentialConstructors);

        static object RegexCreate(Type t, string line, List<(ConstructorInfo tc, RegexAttribute attr)> potentialConstructors = null)
        {
            var conv = TypeDescriptor.GetConverter(t);
            if (conv.CanConvertFrom(typeof(string)))
            {
                return conv.ConvertFromString(line);
            }
            else if (t.IsArray || t.Name.StartsWith("ValueTuple`"))
            {
                return ConvertFromString(t, line);
            }
            else
            {
                potentialConstructors ??= GetPotentialConstructors(t);
                foreach (var (tc, attr) in potentialConstructors)
                {
                    if (RegexCreateInternal(t, line, tc, attr, out object result)) return result;
                }
                throw new Exception($"RegexCreate failed to find suitable constructor for '{line}' in {t.Name}");
            }
        }

        static object RegexCreate(Type t, string line, RegexAttribute paramAttr)
        {
            var tc = t.GetConstructors().First();
            if (RegexCreateInternal(t, line, tc, paramAttr, out object result)) return result;

            throw new Exception($"RegexCreate failed to construct param for '{line}' in {t.Name}");
        }

        static bool RegexCreateInternal(Type t, string line, ConstructorInfo typeConstructor, RegexAttribute attr, out object result)
        {
            var matches = attr.Regex.Matches(line); // match this constructor against the input line

            if (matches.Count == 0)
            {
                result = default;
                return false; // Try other constructors to see if they match
            }

            var paramInfo = typeConstructor.GetParameters();

            object[] convertedParams = ConstructParams(matches, paramInfo);
            result = Activator.CreateInstance(t, convertedParams);

            return true;
        }

        static Array ConvertArray(List<object> input, Type desiredType)
        {
            var arr = Array.CreateInstance(desiredType, input.Count);
            for (int i = 0; i < input.Count; ++i)
            {
                arr.SetValue(input[i], i);
            }
            return arr;
        }

        static object[] CreateObjectArray(Type[] paramDef, string input, SplitAttribute splitAttr = null)
        {
            string[] data = [];
            if (splitAttr != null && splitAttr.KvpMatchRegex != null)
            {
                var matches = splitAttr.KvpMatchRegex.Matches(input);
                if (matches.Count != 1)
                {
                    throw new Exception("Couldn't match kvp regex");
                }

                string[] expectedKeys = ["key", "value"];

                data = [.. expectedKeys.Select(k => matches[0].Groups[k].Value)];
            }
            else
            {
                data = [input];
            }

            var tupleParams = paramDef.Zip(data);
            return [.. tupleParams.Select(kvp => ConvertFromString(kvp.First, kvp.Second.Trim()))]; // convert substrings to match kvp types
        }

        static IEnumerable<object[]> ParseCollectionElements(Type[] paramDef, string[] lines, SplitAttribute splitAttr = null) => lines.Select(line => CreateObjectArray(paramDef, line, splitAttr));

        static object ConvertGenericCollection(Type destinationType, IEnumerable<object[]> kvpdata)
        {
            var collectionType = destinationType.GetGenericTypeDefinition();
            var paramDef = destinationType.GetGenericArguments();

            var collection = Activator.CreateInstance(collectionType.MakeGenericType(paramDef));

            var addFunc = collection.GetType().GetMethod("Add", paramDef);
            foreach (var row in kvpdata)
                addFunc.Invoke(collection, row);

            return collection;
        }

        static IEnumerable<object> Parse(Type t, IEnumerable<string> input)
        {
            List<(ConstructorInfo tc, RegexAttribute attr)> constructors = GetPotentialConstructors(t);
            return input.WithoutNullOrWhiteSpace()
                 .Select(line => RegexCreate(t, line, constructors));
        }

        static object ConvertFromString(Type destinationType, string input, Dictionary<Type, Attribute> attrs = null)
        {
            if (string.IsNullOrEmpty(input)) return Activator.CreateInstance(destinationType); // appropriate empty value

            if (destinationType.IsArray || destinationType.Namespace == "System.Collections.Generic")
            {
                var splitAttr = attrs.Get<SplitAttribute>();

                string[] data = splitAttr != null ? [.. input.Split(splitAttr.Splitter).WithoutNullOrWhiteSpace()] : Util.Split(input);

                if (destinationType.Namespace == "System.Collections.Generic")
                {
                    var arr = ParseCollectionElements(destinationType.GetGenericArguments(), data, splitAttr).ToArray();
                    return ConvertGenericCollection(destinationType, arr);
                }
                else
                {
                    var elementType = destinationType.GetElementType();

                    if (elementType == typeof(string)) return data;

                    var arr = TypeDescriptor.GetConverter(elementType).CanConvertFrom(typeof(string))
                        ? data.Select(item => ConvertFromString(elementType, item, attrs)).ToList()
                        : [.. Parse(elementType, data)];
                    return ConvertArray(arr, elementType);
                }
            }
            else
            {
                var paramAttr = attrs.Get<RegexAttribute>();

                if (destinationType == typeof(string)) return input;

                if (destinationType == typeof(int))
                {
                    var baseAttr = attrs.Get<BaseAttribute>();

                    return baseAttr != null ? Convert.ToInt32(input, baseAttr.NumberBase) : int.Parse(input);
                }

                if (destinationType.IsEnum)
                {
                    input = input.Replace(" ", "_");
                }

                if (destinationType == typeof(bool))
                {
                    return input.Trim()[0].AsBool();
                }

                if (paramAttr == null && destinationType.Name.StartsWith("ValueTuple`"))
                {
                    var parts = Util.Split(input, ",");
                    var typeConstructor = destinationType.GetConstructors().First();
                    var paramInfo = typeConstructor.GetParameters();

                    object[] convertedParams = ConstructParams(parts, paramInfo);
                    return Activator.CreateInstance(destinationType, convertedParams);
                }

                var nullableType = Nullable.GetUnderlyingType(destinationType);
                if (nullableType != null)
                {
                    return ConvertFromString(nullableType, input, attrs);
                }

                var conv = TypeDescriptor.GetConverter(destinationType);
                if (conv.CanConvertFrom(typeof(string)))
                {
                    return conv.ConvertFromString(input);
                }

                return paramAttr != null ? RegexCreate(destinationType, input, paramAttr) : RegexCreate(destinationType, input);
            }
        }

        static object[] ConstructParams(MatchCollection matches, ParameterInfo[] paramInfo)
        {
            if (paramInfo.Length == 0) return default;
            int skip = matches.Count == 1 && matches[0].Groups.Count == 1 && paramInfo.Length == 1 ? 0 : 1;

            var regexValues = matches[0]
                .Groups.Values
                .Skip(skip).Select(g => g.Value).ToArray();

            if (regexValues.Length != paramInfo.Length) throw new Exception("ConstructParams couldn't match constructor param count");

            return ConstructParams(regexValues, paramInfo);
        }

        static object[] ConstructParams(string[] values, ParameterInfo[] paramInfo)
        {
            if (paramInfo.Length == 0) return default;

            var instanceParams = paramInfo.Zip(values); // collate parameter types and matched substrings

            return [.. instanceParams.Select(kvp => ConvertFromString(kvp.First.ParameterType, kvp.Second, GetAttributeMap(kvp.First)))]; // convert substrings to match constructor input
        }

        static Dictionary<Type, Attribute> GetAttributeMap(ParameterInfo pi)
            => Memoizer.Memoize(pi, _ => pi.GetCustomAttributes().ToDictionary(v => v.GetType(), v => v));

        #endregion

    }
}
