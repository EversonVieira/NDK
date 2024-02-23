using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool HasAny<T>(this IEnumerable<T> obj)
        {
            if (obj is null) return false;

            return obj.Count() > 0;
        }
    }
}
