using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class ISetExtensions
    {
        public static T GetItem<T>(this ISet<T> set, int i)
        {
            return set.Skip(i).FirstOrDefault();
        }
    }
}
