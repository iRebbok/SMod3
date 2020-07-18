using System;
using System.Collections.Generic;

namespace SMod3.Core.Misc
{
    public sealed class DuplicateKeyOrderByDescendingComparator<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            // order by descending
            var prioarityComparison = y.CompareTo(x);
            // leave duplicates
            int hashesComparison; // if the priority is the same, then compare hashes
            return prioarityComparison == 0 ? ((hashesComparison = x.GetHashCode().CompareTo(y.GetHashCode())) == 0 ? 1 : hashesComparison) : prioarityComparison;
        }
    }
}
