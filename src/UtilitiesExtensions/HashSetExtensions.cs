using System.Collections.Generic;
using UtilitiesExtensions.Utility;

namespace System.Linq
{

    internal static class HashSetExtensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            DebugCheck.NotNull(set);
            DebugCheck.NotNull(items);

            foreach (var i in items)
            {
                set.Add(i);
            }
        }
    }
}
