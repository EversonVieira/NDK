using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NDK.Core.Utility
{
    public static class GenericUtility
    {
        public static T Clone<T>(this T obj) where T:class
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj)) ?? throw new InvalidOperationException("Unable to clone the provided object.");
        }
    }
}
