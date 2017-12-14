using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnumerableUtils
{
    public static IEnumerable<T> Generate<T>(T seed, Func<T, T> mutate)
    {
        T accum = seed;
        while (true)
        {
            yield return accum;
            accum = mutate(accum);
        }
    }

    public static IEnumerable<int> CountForever()
    {
        return Generate(0, a => a + 1);
    }

    public static T Best<T>(this IEnumerable<T> collection, Func<T, T, T> bestOf)
    {
        var first = collection.FirstOrDefault();
        if (first == null) return default(T);
        return collection.Skip(1).Aggregate(first, bestOf);

    }

    public static T Log<T>(T obj, string pre = "", string sub = "")
    {
        Debug.Log(pre + obj.ToString() + sub);
        return obj;
    }

    public static List<T> Shuffle<T>(this IList<T> from)
    {
        var list = from.ToList();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

}
