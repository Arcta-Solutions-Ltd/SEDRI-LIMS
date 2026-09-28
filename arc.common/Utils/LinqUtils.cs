using System;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public static class LinqUtils
    {
        //public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, TKey> keySelector)
        //{
        //    HashSet<TKey> seenKeys = new HashSet<TKey>();
        //    foreach (var element in source)
        //    {
        //        if (seenKeys.Add(keySelector(element)))
        //        {
        //            yield return element;
        //        }
        //    }
        //}

        public static IEnumerable<TSource> FilterOutWeeds<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, List<TKey> weeds)
        {
            foreach (var element in source)
            {
                if (!weeds.Contains(keySelector(element)))
                    yield return element;

            }

        }
    }
}
