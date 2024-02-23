using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.Core.Extensions
{
    public static class DictionaryHelper
    {
        public static TResult? GetValueByKeyOrDefault<TInput, TResult>(this Dictionary<TInput, TResult> obj, TInput key)
        {
            if (obj is null) return default;

            if (obj.ContainsKey(key)) return obj[key];

            return default;
        }
    }
}
